namespace AI_LAB_1.Preprocessing
{
    public interface IProcessor<TIn, TOut>
    {
        TOut Process(TIn input);
    }
}
