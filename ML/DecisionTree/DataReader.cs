using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace DecisionTree
{
    public class DataReader
    {
        private const string DataFolder = @"data\";
        private const string TrainSuffix = "_train.txt";
        private const string TestSuffix = "_test.txt";

        private const int DataSetsCount = 21;

        public static IList<DataSet> Read()
        {
            return Enumerable.Range(1, DataSetsCount)
                .AsParallel()
                .Select(ReadDataSet)
                .OrderBy(ds => ds.Index)
                .ToArray();
        }
        private static DataSet ReadDataSet(int dataSetNum)
        {
            var prefix = $"{DataFolder}{dataSetNum.ToString().PadLeft(2, '0')}";

            var trainPath = prefix + TrainSuffix;
            var testPath = prefix + TestSuffix;

            var (trainObjects, classesCount) = ReadObjectList(trainPath);

            return new DataSet(dataSetNum, trainObjects, ReadObjectList(testPath).objects, classesCount);
        }
        private static (IList<Object> objects, int classesCount) ReadObjectList(string filePath)
        {
            using var sr = new StreamReader(filePath);

            var classesCount = sr.ReadLine().Split().Last().ToInt();
            var objCount = sr.ReadLine().ToInt();

            var objects = new Object[objCount];
            for (int i = 0; i < objCount; i++)
            {
                var objInfo = sr.ReadLine().Split().Select(int.Parse).ToArray();
                objects[i] = new Object(objInfo.Take(objInfo.Length - 1).ToArray(), objInfo.Last());
            }

            return (objects, classesCount);
        }
    }
}
