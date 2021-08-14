namespace VT.Utilities.Factory
{
    public abstract class AbstractFactory<T> : IFactory<T>
    {
        public abstract T Create();
    }
}