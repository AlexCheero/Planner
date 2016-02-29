using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class WMTranspositionTable
    {
        private Dictionary<int, WorldModelEntry> _entries;

        public WMTranspositionTable()
        {
            _entries = new Dictionary<int, WorldModelEntry>();
        }

        public void Clear()
        {
            _entries.Clear();
        }

        public bool Has(WorldModel model)
        {
            var hash = model.GetHashCode();
            return _entries.ContainsKey(hash) && _entries[hash].Model.ApproxEquals(model);
        }

        public void Add(WorldModel model, int depth)
        {
            //todo dont shure if everything is passing how it should i.e. by reference or by value
            var hash = model.GetHashCode();
            WorldModelEntry entryByHash;
            if (_entries.TryGetValue(hash, out entryByHash))
            {
                if (entryByHash.Model.ApproxEquals(model))
                    entryByHash.Depth = Mathf.Min(depth, entryByHash.Depth);
                //probably there is the better way to find out if new model should oberride old one
                else if (depth < entryByHash.Depth && model.Discontentment <= entryByHash.Model.Discontentment)
                {
                    entryByHash.Model = model;
                    entryByHash.Depth = depth;
                }
            }
            else
                _entries.Add(hash, new WorldModelEntry(model, depth));
        }
    }

    public class WorldModelEntry
    {
        public WorldModel Model;
        public int Depth = int.MaxValue;

        public WorldModelEntry(WorldModel model, int depth)
        {
            Model = model;
            Depth = depth;
        }
    }
}