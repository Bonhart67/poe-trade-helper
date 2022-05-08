using System.Reflection;
using MediatR;
using MongoDB.Driver;
using PTH.API.Scheduling;
using PTH.Logic.Other;
using PTH.Logic.Persistence;
using PTH.Logic.Query;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddMediatR(typeof(GetClusterVariantDetailsHandler).GetTypeInfo().Assembly)
    .AddTransient<IClusterJewelRepository, ClusterJewelRepository>()
    .AddTransient<IClusterJewelBackupRepository, ClusterJewelBackupRepository>()
    .AddTransient<ICurrencyPriceRepository, CurrencyPriceRepository>()
    .AddTransient<ICurrencyConverter, CurrencyConverter>()
    .AddTransient<IHttpQuery, HttpQuery>()
    .AddTransient<IMongoClient, MongoClient>(_ => new MongoClient(new MongoClientSettings
    {
        Server = new MongoServerAddress("172.16.0.14", 27017)
    }))
    .AddTransient<IJsonReader, JsonReader>();

// Add services to the container.

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var updateCurrencyPriceJobKey = new JobKey(nameof(UpdateCurrencyPricesJob));
    q.AddJob<UpdateCurrencyPricesJob>(c => c.WithIdentity(updateCurrencyPriceJobKey));
    q.AddTrigger(c => c
        .ForJob(updateCurrencyPriceJobKey)
        .WithIdentity($"{updateCurrencyPriceJobKey.Name}-trigger")
        .WithSimpleSchedule(x => x
            .WithIntervalInMinutes(10)
            .RepeatForever()));

    var createClusterDetailsJobKey = new JobKey(nameof(CreateClusterDetailsJob));
    q.AddJob<CreateClusterDetailsJob>(c => c.WithIdentity(createClusterDetailsJobKey));
    q.AddTrigger(c => c
        .ForJob(createClusterDetailsJobKey)
        .WithIdentity($"{createClusterDetailsJobKey.Name}-trigger")
        .WithCronSchedule("0 0 0,12 * * ?"));

    var updateClusterPreviewsJobKey = new JobKey(nameof(UpdateClusterPreviewsJob));
    q.AddJob<UpdateClusterPreviewsJob>(c => c.WithIdentity(updateClusterPreviewsJobKey));
    q.AddTrigger(c => c
        .ForJob(updateClusterPreviewsJobKey)
        .WithIdentity($"{updateClusterPreviewsJobKey.Name}-trigger")
        .WithCronSchedule("0 0 * * * ?"));
});

builder.Services.AddQuartzHostedService(q =>
    q.WaitForJobsToComplete = true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions =>
        apiDescriptions.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();

await app.Services.GetRequiredService<IClusterJewelBackupRepository>().FeedAllData();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "MyAPI V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();