using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class TranspositionTable
    {
        private Dictionary<int, WorldModelEntry> _entries;

        public TranspositionTable()
        {
            _entries = new Dictionary<int, WorldModelEntry>();
        }

        public bool Has(WorldModel model)
        {
            var hash = model.GetHashCode();
            return _entries.ContainsKey(hash) && _entries[hash].Model == model;
        }

        public void Add(WorldModel model, int depth)
        {
            //todo dont shure if everything is passing how it should i.e. by reference or by value
            var hash = model.GetHashCode();
            WorldModelEntry entryByHash;
            if (_entries.TryGetValue(hash, out entryByHash))
            {
                if (entryByHash.Model == model)
                    entryByHash.Depth = Mathf.Min(depth, entryByHash.Depth);
                else if (depth < entryByHash.Depth)
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