namespace AI_LAB_1.Preprocessing.Infrastructure
{
    public class ProcessingQuery<TIn> : IProcessingQuery<TIn>
    {
        public TIn Context { get; private set; }

        public ProcessingQuery(TIn input)
        {
            this.Context = input;
        }

        public IProcessingQuery<TOut> Apply<TOut>(IProcessor<TIn, TOut> processor)
        {
            return new ProcessingQuery<TOut>(processor.Process(Context));
        }
    }
}
