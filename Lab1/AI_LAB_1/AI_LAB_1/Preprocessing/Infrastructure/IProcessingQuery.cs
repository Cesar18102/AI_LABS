namespace AI_LAB_1.Preprocessing.Infrastructure
{
    public interface IProcessingQuery<TIn>
    {
        TIn Context { get; }

        IProcessingQuery<TOut> Apply<TOut>(IProcessor<TIn, TOut> processor);
    }
}
