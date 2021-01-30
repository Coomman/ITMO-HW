using System.Collections.Generic;

namespace DecisionTree
{
    public class DataSet
    {
        public int Index { get; }

        public IList<Object> Train { get; }
        public IList<Object> Test { get; }

        public int ClassesCount { get; }

        public DataSet(int index, IList<Object> train, IList<Object> test, int classesCount)
        {
            Index = index;
            Train = train;
            Test = test;
            ClassesCount = classesCount;
        }
    }
}
