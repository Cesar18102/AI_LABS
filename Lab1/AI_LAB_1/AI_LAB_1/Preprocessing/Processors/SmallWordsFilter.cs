using System.Linq;

namespace AI_LAB_1.Preprocessing.Processors
{
    public class SmallWordsFilter : IProcessor<string[], string[]>
    {
        private int MIN_ACCETABLE_SIZE = 3;

        public string[] Process(string[] input)
        {
            return input.Where(word => word.Length >= MIN_ACCETABLE_SIZE).ToArray();
        }
    }
}
