using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public static class DictionaryExtension
    {
        public static object GetObjByKeyPath(this IDictionary<string, object> dictionary, params string[] keys)
        {
            var tempDict = dictionary;
            while (true)
            {
                if (!tempDict.ContainsKey(keys[0]))
                {
#if UNITY_EDITOR
                    Debug.LogError("key not found");
#endif
                    return null;
                }
                var value = tempDict[keys[0]];
                if (keys.Length <= 1)
                    return value;
                keys = keys.Skip(1).ToArray();
                tempDict = value as IDictionary<string, object>;
                if (tempDict != null)
                    continue;
#if UNITY_EDITOR
                Debug.LogError("can't cast intermediate value to dictionary");
#endif
                return null;
            }
        }

        public static bool CheckEquality(this IDictionary<string, object> dict, IDictionary<string, object> other)
        {
            if (dict.Count != other.Count)
                return false;

            foreach (var pair in dict)
            {
                if (!other.Keys.Contains(pair.Key) || !other[pair.Key].Equals(pair.Value))
                {
//                    Debug.Log("not count: " + (!other.Keys.Contains(pair.Key)) + " " + (other[pair.Key] != pair.Value));
//                    Debug.Log("key: " + pair.Key + ", val: " + pair.Value + ", otherVal: " + other[pair.Key] +
//                              ", equality: " + (other[pair.Key].Equals(pair.Value)));
//                    Debug.Log("type val: " + pair.Value.GetType() + ", other: " + other[pair.Key].GetType());
                    return false;
                }
            }

            return true;
        }
    }
}