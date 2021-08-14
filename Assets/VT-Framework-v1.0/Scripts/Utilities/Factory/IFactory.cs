namespace VT.Utilities.Factory
{
    public interface IFactory<T>
    {
        T Create();
    }
}