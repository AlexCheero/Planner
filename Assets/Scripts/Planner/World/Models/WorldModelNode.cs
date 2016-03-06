namespace GOAP
{
    //it looks like these class were intended to use for implementation of following plan mechanism, with tree structure of plan
    //(different variations of happenings)
    public class WorldModelNode
    {
        public int[] ActionIndexes;
        public WorldModel[] Models;

        public WorldModelNode(WorldModel model)
        {
            ActionIndexes = new int[0];
            Models = new[] { model };
        }

        public WorldModelNode(WorldModel model, int actionIndex, WorldModelNode parentNode)
        {
            ActionIndexes = MakeExtendedArray(parentNode.ActionIndexes, actionIndex);
            Models = MakeExtendedArray(parentNode.Models, model);
        }

        private T[] MakeExtendedArray<T>(T[] array, T lastValue)
        {
            var length = array.Length;
            var extendedArray = new T[length + 1];
            for (var i = 0; i < length; i++)
                extendedArray[i] = array[i];
            extendedArray[length] = lastValue;

            return extendedArray;
        }
    }
}