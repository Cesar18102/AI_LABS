using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using ScottPlot;

using AI_LAB_1.Preprocessing;
using AI_LAB_1.Preprocessing.AI;
using AI_LAB_1.Preprocessing.Processors;
using AI_LAB_1.Preprocessing.Infrastructure;

namespace AI_LAB_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<string, (bool, int)> files = new Dictionary<string, (bool, int)>()
            {
                { Environment.CurrentDirectory + "/Data/1.txt", (true, 0) }, //нефть
                { Environment.CurrentDirectory + "/Data/4.txt", (false, 1) },
                { Environment.CurrentDirectory + "/Data/5.txt", (false, 2) },
                { Environment.CurrentDirectory + "/Data/2.txt", (true, 0) }, //нефть
                { Environment.CurrentDirectory + "/Data/6.txt", (false, 1) },
                { Environment.CurrentDirectory + "/Data/7.txt", (false, 2) },
                { Environment.CurrentDirectory + "/Data/3.txt", (true, 0) }, //нефть
                { Environment.CurrentDirectory + "/Data/8.txt", (false, 1) },
                { Environment.CurrentDirectory + "/Data/9.txt", (false, 2) },
                { Environment.CurrentDirectory + "/Data/10.txt", (true, 0) }, //нефть
                { Environment.CurrentDirectory + "/Data/11.txt", (false, 1) },
                { Environment.CurrentDirectory + "/Data/12.txt", (false, 2) },
                { Environment.CurrentDirectory + "/Data/13.txt", (true, 0) }, //нефть
                { Environment.CurrentDirectory + "/Data/14.txt", (false, 1) },
                { Environment.CurrentDirectory + "/Data/15.txt", (false, 2) },
            };

            Dictionary<string, (bool, int)> texts = files.ToDictionary(
                file => File.ReadAllText(file.Key),
                file => file.Value
            );

            Dictionary<(string, bool, int), string[]> dataSets = texts.ToDictionary(
                text => (text.Key, text.Value.Item1, text.Value.Item2),
                text => new ProcessingQuery<string>(text.Key)
                    .Apply(new TextToRawWordsPreprocessor())
                    .Apply(new RawWordFormatProcessor().CreateArrayProcessor())
                    .Apply(new ReplaceRareSpecificLettersWithCommonWordProcessor().CreateArrayProcessor())
                    .Apply(new RemoveEndingsWordProcessor().CreateArrayProcessor())
                    .Apply(new SmallWordsFilter())
                    .Context
            );

            Random random = new Random();
            Plot plot = new Plot();
            Dictionary<int, List<double>> correctPercent = new Dictionary<int, List<double>>();

            for (int i = 0; i < 10000; ++i)
            {
                Dictionary<int, double> correctAnswerPercentByTeachSetsCount = new Dictionary<int, double>();

                for (int teachSetsCount = 3; teachSetsCount <= dataSets.Count * 2 / 3.0; ++teachSetsCount)
                {
                    //Console.WriteLine($"Teaching on {teachSetsCount} sets");

                    BaesianClassifier<string> classifier = new BaesianClassifier<string>();
                    Dictionary<(string, bool, int), string[]> dataSetsCopy = dataSets.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                    int left = teachSetsCount;
                    int totalCount = dataSetsCopy.Count;
                    List<IGrouping<int, KeyValuePair<(string, bool, int), string[]>>> groups = dataSetsCopy.GroupBy(set => set.Key.Item3).ToList();

                    for(int k = 0; k < groups.Count; ++k)
                    {
                        Dictionary<(string, bool, int), string[]> groupDictionary = groups[k].ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value
                        );

                        int teachCount = k == groups.Count - 1 ? left : groupDictionary.Count * teachSetsCount / totalCount;
                        left -= teachCount;

                        for (int j = 0; j < teachCount; ++j)
                        {
                            int setId = random.Next(0, groupDictionary.Count);

                            KeyValuePair<(string, bool, int), string[]> teachDataSet = groupDictionary.ElementAt(setId);

                            dataSetsCopy.Remove(teachDataSet.Key);
                            groupDictionary.Remove(teachDataSet.Key);

                            classifier.Teach(teachDataSet.Value, teachDataSet.Key.Item2);
                        }
                    }

                    //GROUP A IS SPAM
                    //GROUP B IS NOT SPAM

                    int correctCount = 0;

                    //Console.WriteLine($"\n\nTesting on {dataSetsCopy.Count} sets\n\n");

                    foreach (KeyValuePair<(string, bool, int), string[]> testSet in dataSetsCopy)
                    {
                        Probability probability = classifier.Predict(testSet.Value);
                        bool isSpam = probability.ProbabilityA > probability.ProbabilityB;

                        string resultText = isSpam ? "SPAM DETECTED" : "No spam";
                        string actualText = testSet.Key.Item2 ? "SPAM" : "NOT SPAM";

                        if (testSet.Key.Item2 == isSpam)
                            ++correctCount;

                        //Console.WriteLine(testSet.Key.Item1);
                        //Console.WriteLine($"{resultText}\nActually was {actualText}\n*******");
                    }

                    double correctAnswerPercent = (double)correctCount / dataSetsCopy.Count;

                    correctAnswerPercentByTeachSetsCount.Add(teachSetsCount, correctAnswerPercent);

                    if (!correctPercent.ContainsKey(teachSetsCount))
                        correctPercent.Add(teachSetsCount, new List<double>());

                    correctPercent[teachSetsCount].Add(correctAnswerPercent);
                }

                plot.AddSignalXY(correctAnswerPercentByTeachSetsCount.Keys.Select(k => (double)k).ToArray(), correctAnswerPercentByTeachSetsCount.Values.ToArray());
            }

            plot.Render().Save(Environment.CurrentDirectory + "/Data/result.png");

            foreach (var kvp in correctPercent)
                Console.WriteLine($"teach = {kvp.Key}; correct avg = {kvp.Value.Average()}");

            Plot avgPlot = new Plot();
            avgPlot.AddSignalXY(correctPercent.Keys.Select(k => (double)k).ToArray(), correctPercent.Values.Select(v => v.Average()).ToArray());
            avgPlot.Render().Save(Environment.CurrentDirectory + "/Data/result_avg.png");

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
