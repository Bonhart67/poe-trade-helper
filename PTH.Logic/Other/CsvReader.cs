using System.Collections.Immutable;

namespace PTH.Logic.Http;

public class CsvReader : ICsvReader
{
    public async Task<IReadOnlyList<ClusterVariantRequestData>> ReadLines()
    {
        return (await File.ReadAllLinesAsync($"{Environment.CurrentDirectory}/variant-name-requestbody.csv"))
            .Select(l =>
            {
                var split = l.Split(';');
                return new ClusterVariantRequestData
                {
                    Type = split[0],
                    Variant = split[1],
                    Request = split[2]
                };
            }).ToImmutableList();
    }
}