using System.Collections.Generic;

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
            var hash = model.GetHashCode();
            if (Entries.ContainsKey(hash) && Entries[hash].Model == model)
            {
                
            }
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