using System.Linq;

namespace AI_LAB_1.Preprocessing.Processors
{
    public class RemoveEndingsWordProcessor : IProcessor<string, string>
    {
        private static string[] ENDINGS = new string[]
        {
            "ый", "ий", "ей", "ой",
            "ые", "ие", "ое", "ее",
            "ая", "яя", "оя", "ия",
            "ую", "ию", "ыю",
            "их", "ых", "ах", "ях",
            "ым", "ем", "ом", "ам",
            "а", "я", 
            "ы", "и", 
            "о", "е", 
            "ь"
        };

        public string Process(string input)
        {
            string ending = ENDINGS.FirstOrDefault(end => input.EndsWith(end));
            return ending == null || (double)ending.Length / input.Length >= 0.4 ? 
                input : input.Substring(0, input.Length - ending.Length);
        }
    }
}
