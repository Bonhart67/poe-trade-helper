using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using PTH.Logic;
using PTH.Logic.Other;
using PTH.Logic.Persistence;
using PTH.Logic.Queries;

var serviceProvider = new ServiceCollection()
    .AddMediatR(typeof(GetClusterVariantPreviewsHandler).GetTypeInfo().Assembly)
    .AddScoped<IClusterJewelRepository, ClusterJewelRepository>()
    .AddScoped<IClusterJewelBackupRepository, ClusterJewelBackupRepository>()
    .AddScoped<ICurrencyPriceRepository, CurrencyPriceRepository>()
    .AddScoped<ICurrencyConverter, CurrencyConverter>()
    .AddScoped<IMongoClient, MongoClient>()
    .AddScoped<IHttpQuery, HttpQuery>()
    .AddScoped<ICsvReader, CsvReader>()
    .BuildServiceProvider();

// await serviceProvider.GetService<IClusterJewelBackupRepository>().FeedClusterVariantPreviewsIfEmpty();
await serviceProvider.GetService<IClusterJewelRepository>().CreateClusterDetails();
// var results = await serviceProvider.GetService<IClusterJewelRepository>().GetClusterDetails();

// await serviceProvider.GetService<ICurrencyPriceRepository>().UpdatePrices();

// await serviceProvider.GetService<IClusterJewelRepository>().UpdateClusterPreviews();
Console.ReadKey();