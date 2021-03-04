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
            Dictionary<string, bool> files = new Dictionary<string, bool>()
            {
                { Environment.CurrentDirectory + "/Data/1.txt", true }, //нефть
                { Environment.CurrentDirectory + "/Data/4.txt", false },
                { Environment.CurrentDirectory + "/Data/5.txt", false },
                { Environment.CurrentDirectory + "/Data/2.txt", true }, //нефть
                { Environment.CurrentDirectory + "/Data/6.txt", false },
                { Environment.CurrentDirectory + "/Data/7.txt", false },
                { Environment.CurrentDirectory + "/Data/3.txt", true }, //нефть
                { Environment.CurrentDirectory + "/Data/8.txt", false },
                { Environment.CurrentDirectory + "/Data/9.txt", false },
                { Environment.CurrentDirectory + "/Data/10.txt", true }, //нефть
                { Environment.CurrentDirectory + "/Data/11.txt", false },
                { Environment.CurrentDirectory + "/Data/12.txt", false },
                { Environment.CurrentDirectory + "/Data/13.txt", true }, //нефть
                { Environment.CurrentDirectory + "/Data/14.txt", false },
                { Environment.CurrentDirectory + "/Data/15.txt", false },
            };

            Dictionary<string, bool> texts = files.ToDictionary(
                file => File.ReadAllText(file.Key),
                file => file.Value
            );

            Dictionary<(string, bool), string[]> dataSets = texts.ToDictionary(
                text => (text.Key, text.Value),
                text => new ProcessingQuery<string>(text.Key)
                    .Apply(new TextToRawWordsPreprocessor())
                    .Apply(new RawWordFormatProcessor().CreateArrayProcessor())
                    .Apply(new ReplaceRareSpecificLettersWithCommonWordProcessor().CreateArrayProcessor())
                    .Apply(new RemoveEndingsWordProcessor().CreateArrayProcessor())
                    .Apply(new SmallWordsFilter())
                    .Context
            );

            Random random = new Random();
            Dictionary<int, double> correctAnswerPercentByTeachSetsCount = new Dictionary<int, double>();

            for (int teachSetsCount = 3; teachSetsCount <= dataSets.Count * 2 / 3.0; ++teachSetsCount)
            {
                Console.WriteLine($"Teaching on {teachSetsCount} sets");

                BaesianClassifier<string> classifier = new BaesianClassifier<string>();
                Dictionary<(string, bool), string[]> dataSetsCopy = dataSets.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                for (int j = 0; j < teachSetsCount; ++j)
                {
                    //int setId = random.Next(0, dataSetsCopy.Count);

                    KeyValuePair<(string, bool), string[]> teachDataSet = dataSetsCopy.ElementAt(0);
                    dataSetsCopy.Remove(teachDataSet.Key);

                    classifier.Teach(teachDataSet.Value, teachDataSet.Key.Item2);
                }

                //GROUP A IS SPAM
                //GROUP B IS NOT SPAM

                int correctCount = 0;

                Console.WriteLine($"Testing on {dataSetsCopy.Count} sets");

                foreach (KeyValuePair<(string, bool), string[]> testSet in dataSetsCopy)
                {
                    Probability probability = classifier.Predict(testSet.Value);
                    bool isSpam = probability.ProbabilityA > probability.ProbabilityB;

                    string resultText = isSpam ? "SPAM DETECTED" : "No spam";
                    string actualText = testSet.Key.Item2 ? "SPAM" : "NOT SPAM";

                    if (testSet.Key.Item2 == isSpam)
                        ++correctCount;

                    Console.WriteLine(testSet.Key.Item1);
                    Console.WriteLine($"\n{resultText}\nActually was {actualText}\n\n************************************************\n\n");
                }

                correctAnswerPercentByTeachSetsCount.Add(teachSetsCount, (double)correctCount / dataSetsCopy.Count);
            }

            Plot plot = new Plot();

            plot.AddSignalXY(correctAnswerPercentByTeachSetsCount.Keys.Select(k => (double)k).ToArray(), correctAnswerPercentByTeachSetsCount.Values.ToArray());
            plot.Render().Save(Environment.CurrentDirectory + "/Data/result.png");

            Console.ReadLine();
        }
    }
}
