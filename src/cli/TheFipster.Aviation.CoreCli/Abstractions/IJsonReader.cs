namespace TheFipster.Aviation.CoreCli.Abstractions
{
    public interface IJsonReader<T>
    {
        T FromFile(string filepath);
        T FromText(string json);
    }
}
