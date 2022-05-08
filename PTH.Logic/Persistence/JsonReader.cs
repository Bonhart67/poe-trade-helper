using Newtonsoft.Json;
using PTH.Domain.Queries;

namespace PTH.Logic.Persistence;

public class JsonReader : IJsonReader
{
    private readonly string _detailsBackupPath = $"{Environment.CurrentDirectory}/Data/details-backup.json";
    private readonly string _requestDataPath = $"{Environment.CurrentDirectory}/Data/request-data.json";
    
    public async Task<List<T>?> Read<T>() where T : class
    {
        return typeof(T) switch
        {
            var detail when detail == typeof(ClusterVariantDetail) => await Read<T>(_detailsBackupPath),
            var request when request == typeof(ClusterVariantRequestData) => await Read<T>(_requestDataPath),
            _ => throw new ArgumentException($"Type {typeof(T)} is not readable")
        };
    }

    private async Task<List<T>?> Read<T>(string path)
    {
        return JsonConvert.DeserializeObject<List<T>>(await File.ReadAllTextAsync(path));
    }
}