using System.Globalization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Taxually.TechnicalTest;
using Taxually.TechnicalTest.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<VatRegistrationRequestValidator>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HTTP client for UK API
builder.Services.AddSingleton<ITaxuallyHttpClient, TaxuallyHttpClient>();
// Register queue clients for CSV and XML uploads
builder.Services.AddSingleton<ITaxuallyQueueClient, TaxuallyQueueClient>();
// Register strategies
builder.Services.AddScoped<IVatRegistrationStrategy, UKVatRegistrationStrategy>();
builder.Services.AddScoped<IVatRegistrationStrategy, FranceVatRegistrationStrategy>();
builder.Services.AddScoped<IVatRegistrationStrategy, GermanyVatRegistrationStrategy>();

// Register strategy factory
builder.Services.AddScoped<IVatRegistrationStrategyFactory, VatRegistrationStrategyFactory>();

// Register logging
builder.Services.AddLogging();
        

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
