using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using PTH.Logic;
using PTH.Logic.Http;
using PTH.Logic.Persistence;

var serviceProvider = new ServiceCollection()
    .AddMediatR(typeof(ClusterTypeQueryHandler).GetTypeInfo().Assembly)
    .AddScoped<IClusterJewelRepository, ClusterJewelRepository>()
    .AddScoped<IClusterJewelBackupRepository, ClusterJewelBackupRepository>()
    .AddScoped<IMongoClient, MongoClient>()
    .AddScoped<IHttpQuery, HttpQuery>()
    .AddScoped<ICsvReader, CsvReader>()
    .BuildServiceProvider();

await serviceProvider.GetService<IClusterJewelBackupRepository>().FeedClusterVariantPreviewsIfEmpty();
await serviceProvider.GetService<IClusterJewelRepository>().CreateClusterDetails();
// var results = await serviceProvider.GetService<IClusterJewelRepository>().GetClusterDetails();