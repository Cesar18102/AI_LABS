namespace AI_LAB_1.Preprocessing.Processors.Basic
{
    public class ArrayProcessor<TIn, TOut> : IProcessor<TIn[], TOut[]>
    {
        private IProcessor<TIn, TOut> ItemProcessor { get; set; }

        public ArrayProcessor(IProcessor<TIn, TOut> itemProcessor)
        {
            this.ItemProcessor = itemProcessor;
        }

        public TOut[] Process(TIn[] input)
        {
            TOut[] result = new TOut[input.Length];

            for (int i = 0; i < input.Length; ++i)
                result[i] = ItemProcessor.Process(input[i]);

            return result;
        }
    }
}
