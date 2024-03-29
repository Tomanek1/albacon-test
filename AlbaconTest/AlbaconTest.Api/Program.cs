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

    //Jeliko� Minimal API narozd�l od MVC neum� automaticky zpracovat Accept header, mus�me to ud�lat ru�n�
    if (context.Request.Headers.Accept.Equals(MediaTypeNames.Text.Xml))
        return result.ToList().SerializeObject();
    if (context.Request.Headers.Accept.Equals("application/x-msgpack"))
        return ""; // Proto�e jsem s MsgPack nikdy nepracoval, neve�el bych se p�i jeho nastudov�n� do �asov�ho limitu 2 hodin, tak�e jsem to neimplementoval
    else
        return JsonSerializer.Serialize(result);
})
    .Produces<IReadOnlyCollection<Document>>()
    .Accepts<Document>(MediaTypeNames.Application.Json, [MediaTypeNames.Text.Xml, "application/x-msgpack"])
    .WithName("GetDocuments");


app.MapGet("/documents/{id}", async (
    //Validace na p��tomnost z�kladn�ch typ� se prov�d� automaticky
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
    //Validace na referen�n� typy se neprov�d� automaticky
    [Required, BindRequired, FromBody] Document document,
    [FromServices] IDatastoreService datastoreService) =>
{
    Guid id = await datastoreService.Insert(document);

    return Results.CreatedAtRoute("GetDocumentById", new { Id = id });
})
    //Fluent validace se prov�d� jen nad propertama modelu ne nad jeho instanc�
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
