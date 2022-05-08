using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PTH.Logic.Other;
using PTH.Logic.Persistence;
using PTH.Logic.Query;

var serviceProvider = new ServiceCollection()
    .AddMediatR(typeof(GetClusterVariantPreviewsHandler).GetTypeInfo().Assembly)
    .AddScoped<IClusterJewelRepository, ClusterJewelRepository>()
    .AddScoped<IClusterJewelBackupRepository, ClusterJewelBackupRepository>()
    .AddScoped<ICurrencyPriceRepository, CurrencyPriceRepository>()
    .AddScoped<ICurrencyConverter, CurrencyConverter>()
    .AddScoped<IMongoClient, MongoClient>()
    .AddScoped<IHttpQuery, HttpQuery>()
    .AddScoped<IJsonReader, JsonReader>()
    .BuildServiceProvider();

// await serviceProvider.GetService<IClusterJewelBackupRepository>().FeedClusterVariantPreviews();
// await serviceProvider.GetService<IClusterJewelRepository>().CreateClusterDetails();
// var results = await serviceProvider.GetService<IClusterJewelRepository>().GetClusterDetails();

// await serviceProvider.GetService<ICurrencyPriceRepository>().UpdatePrices();

// await serviceProvider.GetService<IClusterJewelRepository>().UpdateClusterPreviews();

// await serviceProvider.GetService<IClusterJewelBackupRepository>().BackupCurrentClusterDetails();