using AI_LAB_1.Preprocessing.Processors.Basic;

namespace AI_LAB_1.Preprocessing
{
    public static class ProcessorFactory
    {
        public static ArrayProcessor<TIn, TOut> CreateArrayProcessor<TIn, TOut>(this IProcessor<TIn, TOut> processor)
        {
            return new ArrayProcessor<TIn, TOut>(processor);
        }
    }
}
