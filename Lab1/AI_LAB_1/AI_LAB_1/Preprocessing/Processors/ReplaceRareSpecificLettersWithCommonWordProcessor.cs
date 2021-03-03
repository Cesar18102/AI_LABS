using System.Collections.Generic;

namespace AI_LAB_1.Preprocessing.Processors
{
    public class ReplaceRareSpecificLettersWithCommonWordProcessor : IProcessor<string, string>
    {
        private Dictionary<char, char> REPLACED_LETTERS = new Dictionary<char, char>()
        {
            { 'ё', 'е' },
        };

        public string Process(string input)
        {
            foreach (KeyValuePair<char, char> replacement in REPLACED_LETTERS)
                input = input.Replace(replacement.Key, replacement.Value);

            return input;
        }
    }
}
