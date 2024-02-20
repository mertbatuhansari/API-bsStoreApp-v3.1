using Microsoft.AspNetCore.Mvc;
using NLog;
using Services.Contracts;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

#pragma warning disable CS0618 // Type or member is obsolete
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
#pragma warning restore CS0618 // Type or member is obsolete

/*��erik Pazarl��� - Content Negotiation */
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
})
    .AddCustomCsvFormatter() //Extensions/IMvcBuilderExtensions i�erisinde ��z�ld�.
    .AddXmlDataContractSerializerFormatters();

// Add services to the container.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyRefence).Assembly) //presentation katman�
    .AddNewtonsoftJson(); //patch kullanabilmek i�in
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSqlContext(builder.Configuration); // ServicesExtensions'tan geliyor.
builder.Services.ConfigureRepositoryManager(); // Tek parametre oldu�u i�in (dizi olmad��� i�in) vermek zorunda de�ilim.
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService(); //log kayd�
builder.Services.AddAutoMapper(typeof(Program)); //Tek sat�rda �a��rabildi�im i�in Extensions'a eklemedim.


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; //Model state invalid oldu�unda devreye girer. 422 hata kodu g�ndermek i�in.
});

var app = builder.Build();

//uygulama aya�a kalkt�ktan sonra GetRequiredService ifadesiyle servis �a��r�labilir.
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
