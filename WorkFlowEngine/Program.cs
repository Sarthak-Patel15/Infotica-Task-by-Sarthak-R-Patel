// This is the entry point of the application where all API endpoints are defined using Minimal API style.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkflowEngine.Models;
using WorkflowEngine.Services;

var builder = WebApplication.CreateBuilder(args);

// Registering our WorkflowService as a singleton to keep data in-memory for the whole app lifecycle.
builder.Services.AddSingleton<WorkflowService>();

var app = builder.Build();

var service = app.Services.GetRequiredService<WorkflowService>();

// Create a new workflow definition
app.MapPost("/workflow", (WorkflowDefinition def) =>
{
    var result = service.CreateWorkflow(def);
    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
});

// Retrieve an existing workflow definition by ID
app.MapGet("/workflow/{id}", (string id) =>
{
    var result = service.GetWorkflow(id);
    return result is not null ? Results.Ok(result) : Results.NotFound("Workflow not found");
});

// Start a new workflow instance from a given definition
app.MapPost("/instance/{workflowId}", (string workflowId) =>
{
    var result = service.StartInstance(workflowId);
    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
});

// Execute an action on a workflow instance to move it to a new state
app.MapPost("/instance/{instanceId}/action/{actionId}", (string instanceId, string actionId) =>
{
    var result = service.ExecuteAction(instanceId, actionId);
    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
});

// Get the current state and history of a workflow instance
app.MapGet("/instance/{instanceId}", (string instanceId) =>
{
    var instance = service.GetInstance(instanceId);
    return instance is not null ? Results.Ok(instance) : Results.NotFound("Instance not found");
});

app.Run();
