using Microsoft.AspNetCore.Mvc;
using NLog;
using Services.Contracts;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

#pragma warning disable CS0618 // Type or member is obsolete
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
#pragma warning restore CS0618 // Type or member is obsolete

/*Ýçerik Pazarlýðý - Content Negotiation */
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
})
    .AddCustomCsvFormatter() //Extensions/IMvcBuilderExtensions içerisinde çözüldü.
    .AddXmlDataContractSerializerFormatters();

// Add services to the container.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyRefence).Assembly) //presentation katmaný
    .AddNewtonsoftJson(); //patch kullanabilmek için
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSqlContext(builder.Configuration); // ServicesExtensions'tan geliyor.
builder.Services.ConfigureRepositoryManager(); // Tek parametre olduðu için (dizi olmadýðý için) vermek zorunda deðilim.
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService(); //log kaydý
builder.Services.AddAutoMapper(typeof(Program)); //Tek satýrda çaðýrabildiðim için Extensions'a eklemedim.


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; //Model state invalid olduðunda devreye girer. 422 hata kodu göndermek için.
});

var app = builder.Build();

//uygulama ayaða kalktýktan sonra GetRequiredService ifadesiyle servis çaðýrýlabilir.
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
