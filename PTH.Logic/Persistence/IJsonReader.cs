namespace PTH.Logic.Persistence;

public interface IJsonReader
{
    Task<List<T>?> Read<T>() where T : class;
}