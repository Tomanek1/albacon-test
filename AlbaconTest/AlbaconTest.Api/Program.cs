using AlbaconTest.Services.Models;
using Microsoft.AspNetCore.Mvc;
using AlbaconTest.Services.Infrastructure;
using System.Text.Json;
using AlbaconTest.Services.Extensions;
using System.Net.Mime;
using AlbaconTest.Services.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using AlbaconTest.Api.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.RegisterServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/documents/", async (
    HttpContext context,
    [FromServices] IDatastoreService datastoreService) =>
{
    var result = await datastoreService.GetAll();

    //Jelikož Minimal API narozdíl od MVC neumí automaticky zpracovat Accept header, musíme to udìlat ruènì
    if (context.Request.Headers.Accept.Equals(MediaTypeNames.Text.Xml))
        return result.ToList().SerializeObject();
    if (context.Request.Headers.Accept.Equals("application/x-msgpack"))
        return ""; // Protože jsem s MsgPack nikdy nepracoval, nevešel bych se pøi jeho nastudování do èasového limitu 2 hodin, takže jsem to neimplementoval
    else
        return JsonSerializer.Serialize(result);
})
    .Produces<IReadOnlyCollection<Document>>()
    .Accepts<Document>(MediaTypeNames.Application.Json, [MediaTypeNames.Text.Xml, "application/x-msgpack"])
    .WithName("GetDocuments");


app.MapGet("/documents/{id}", async (
    //Validace na pøítomnost základních typù se provádí automaticky
    //int count,
    Guid id,
    HttpContext context,
    [FromServices] IDatastoreService datastoreService) =>
{
    var result = await datastoreService.GetAll();

    if (context.Request.Headers.Accept.Equals(MediaTypeNames.Text.Xml))
        return result.ToList().SerializeObject();
    else if (context.Request.Headers.Accept.Equals("application/x-msgpack"))
        return "";
    else
        return JsonSerializer.Serialize(result);
})
    .Produces<Document>()
    .Accepts<Document>(MediaTypeNames.Application.Json, [MediaTypeNames.Text.Xml, "application/x-msgpack"])
    .WithName("GetDocumentById");


app.MapPost("/documents/", async (
    //Validace na referenèní typy se neprovádí automaticky
    [Required, BindRequired, FromBody] Document document,
    [FromServices] IDatastoreService datastoreService) =>
{
    Guid id = await datastoreService.Insert(document);

    return Results.CreatedAtRoute("GetDocumentById", new { Id = id });
})
    //Fluent validace se provádí jen nad propertama modelu ne nad jeho instancí
    .AddFluentValidationAutoValidation()
    .AddEndpointFilter<EmptyModelValidatorFilter<Document>>();

app.MapPut("/documents/", async (
    [Required, FromBody] Document document,
    [FromServices] IDatastoreService datastoreService) =>
{
    await datastoreService.Update(document);
    return Results.CreatedAtRoute("GetDocumentById", new { Id = document.Identifier });
});

app.MapDelete("/documents/{id}", async (
   [BindRequired] Guid id,
   [FromServices] IDatastoreService datastoreService) =>
{
    await datastoreService.Delete(id);
    return Results.NoContent();
});

app.Run();
