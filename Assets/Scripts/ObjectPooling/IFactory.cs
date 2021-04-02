namespace ObjectPooling
{
    public interface IFactory<T>
    {
        T Create();
    }
}