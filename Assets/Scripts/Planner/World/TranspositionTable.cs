using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class TranspositionTable
    {
        public Dictionary<int, WorldModelEntry> Entries;

        public TranspositionTable(int size)
        {
            Entries = new Dictionary<int, WorldModelEntry>();
        }

        public bool Has(WorldModel model)
        {
            var hash = model.GetHashCode();
            return Entries.ContainsKey(hash) && Entries[hash].Model == model;
        }

        public void Add(WorldModel model, int depth)
        {
            //todo dont shure if everything is passing how it should i.e. by reference or by value
            var hash = model.GetHashCode();
            WorldModelEntry entryByHash;
            if (Entries.TryGetValue(hash, out entryByHash))
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
                Entries.Add(hash, new WorldModelEntry(model, depth));
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