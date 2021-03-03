namespace AI_LAB_1.Preprocessing.Processors
{
    public class RawWordFormatProcessor : IProcessor<string, string>
    {
        public string Process(string input)
        {
            return input.ToLower();
        }
    }
}
