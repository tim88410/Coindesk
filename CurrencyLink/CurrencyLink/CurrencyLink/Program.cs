using CurrencyLink.Application.Commands;
using CurrencyLink.Application.Queries;
using CurrencyLink.Domain.Repositories.CoindeskAPIClient;
using CurrencyLink.Domain.Service;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using CurrencyLink.Infrastructure.Repositories.CoindeskAPIClient;
using CurrencyLink.Infrastructure.Service;
using DBUtility;
using RestSharp;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
// 設定 Serilog 日誌
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt")         // 輸出到檔案
    .CreateLogger();

builder.Services.AddSingleton(new AESHelper());

var aesHelper = builder.Services.BuildServiceProvider().GetService<AESHelper>();
string connAESString = builder.Configuration.GetConnectionString("DefaultConnection");
string connectionString = aesHelper.Decrypt(connAESString);
builder.Services.AddScoped<IDataBaseUtility, DataBaseUtility>(provider =>
    new DataBaseUtility(connectionString));

builder.Services.AddScoped<ICoindeskAPIClientService, CoindeskAPIClientService>();
builder.Services.AddScoped(sp =>
{
    var baseUrl = "https://api.coindesk.com/v1/bpi/currentprice.json";
    var options = new RestClientOptions(baseUrl);
    return new RestClient(options);
});
builder.Services.AddScoped<IRestClientWrapperService, RestClientWrapperService>();

builder.Services.AddScoped<ICoindeskAPIClientRepository, CoindeskAPIClientRepository>();
builder.Services.AddScoped<ICoindeskQueryRepository, CoindeskQueryRepository>();
builder.Services.AddScoped<ICoindeskCommandRepository, CoindeskCommandRepository>();

// 註冊 MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
// 設定序列化選項
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // 使用原始屬性名稱
    });

//AutoMapper
builder.Services.AddAutoMapper(
    (serviceProvider, mapperConfiguration) => mapperConfiguration.AddProfiles(new AutoMapper.Profile[]
    {
        new CommandProfile(),
        new QueriesProfile()
    }),
    AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
