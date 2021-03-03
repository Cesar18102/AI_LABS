using System.Linq;
using System.Text.RegularExpressions;

namespace AI_LAB_1.Preprocessing.Processors
{
    public class TextToRawWordsPreprocessor : IProcessor<string, string[]>
    {
        private static Regex NON_ALPHANUMERIC_PATTERN = new Regex("\\W+");
        private static Regex NUMERIC_PATTERN = new Regex("\\d+");

        public string[] Process(string input)
        {
            string[] words = NON_ALPHANUMERIC_PATTERN.Split(input);
            return words.Select(word => NUMERIC_PATTERN.Replace(word, ""))
                .Where(word => !string.IsNullOrEmpty(word))
                .ToArray();
        }
    }
}
