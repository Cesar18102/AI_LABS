using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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
                { @"D:\!!!OLEG\!!!WORK\Studying\ИАД\Lab\Lab1\Data\1.txt", true }, //нефть
                { @"D:\!!!OLEG\!!!WORK\Studying\ИАД\Lab\Lab1\Data\2.txt", true }, //нефть
                { @"D:\!!!OLEG\!!!WORK\Studying\ИАД\Lab\Lab1\Data\3.txt", true }, //нефть
                { @"D:\!!!OLEG\!!!WORK\Studying\ИАД\Lab\Lab1\Data\4.txt", false },
                { @"D:\!!!OLEG\!!!WORK\Studying\ИАД\Lab\Lab1\Data\5.txt", false },
                { @"D:\!!!OLEG\!!!WORK\Studying\ИАД\Lab\Lab1\Data\6.txt", false },
                { @"D:\!!!OLEG\!!!WORK\Studying\ИАД\Lab\Lab1\Data\7.txt", false },
                { @"D:\!!!OLEG\!!!WORK\Studying\ИАД\Lab\Lab1\Data\8.txt", false },
                { @"D:\!!!OLEG\!!!WORK\Studying\ИАД\Lab\Lab1\Data\9.txt", false },
                { @"D:\!!!OLEG\!!!WORK\Studying\ИАД\Lab\Lab1\Data\10.txt", true }, //нефть
                { @"D:\!!!OLEG\!!!WORK\Studying\ИАД\Lab\Lab1\Data\11.txt", false },
                { @"D:\!!!OLEG\!!!WORK\Studying\ИАД\Lab\Lab1\Data\12.txt", false },
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

            BaesianClassifier<string> classifier = new BaesianClassifier<string>();

            double teachRatio = 2.0 / 3.0;
            Random random = new Random();

            Dictionary<(string, bool), string[]> teachSets = new Dictionary<(string, bool), string[]>();
            Dictionary<(string, bool), string[]> testSets = new Dictionary<(string, bool), string[]>();

            foreach(KeyValuePair<(string, bool), string[]> kvp in dataSets)
            {
                if(random.NextDouble() < teachRatio)
                    teachSets.Add(kvp.Key, kvp.Value);
                else
                    testSets.Add(kvp.Key, kvp.Value);
            }

            //GROUP A IS SPAM
            //GROUP B IS NOT SPAM

            foreach (KeyValuePair<(string, bool), string[]> teachSet in teachSets)
                classifier.Teach(teachSet.Value, teachSet.Key.Item2);

            foreach(KeyValuePair<(string, bool), string[]> testSet in testSets)
            {
                Probability probability = classifier.Predict(testSet.Value);

                string resultText = probability.ProbabilityA > probability.ProbabilityB ? "SPAM DETECTED" : "No spam";
                string actualText = testSet.Key.Item2 ? "SPAM" : "NOT SPAM";

                Console.WriteLine(testSet.Key.Item1);
                Console.WriteLine($"\n{resultText}\nActually was {actualText}\n\n************************************************\n\n");
            }

            Console.ReadLine();
        }
    }
}
