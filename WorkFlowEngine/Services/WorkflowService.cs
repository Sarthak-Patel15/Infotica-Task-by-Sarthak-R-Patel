using WorkflowEngine.Models;

namespace WorkflowEngine.Services;

// This service handles all business logic: creating workflows, starting instances, and validating transitions.
public class WorkflowService
{
    // Store workflows and instances in memory
    private readonly Dictionary<string, WorkflowDefinition> _definitions = new();
    private readonly Dictionary<string, WorkflowInstance> _instances = new();

    // Create a new workflow definition
    public (bool IsSuccess, string Message) CreateWorkflow(WorkflowDefinition def)
    {
        // Basic validations
        if (string.IsNullOrWhiteSpace(def.Id))
            return (false, "Workflow must have an ID.");

        if (_definitions.ContainsKey(def.Id))
            return (false, "Workflow ID already exists.");

        // Must have exactly one initial state
        if (!def.States.Any(s => s.IsInitial))
            return (false, "Workflow must have one initial state.");
        if (def.States.Count(s => s.IsInitial) > 1)
            return (false, "Workflow must have exactly one initial state.");

        // No duplicate state IDs
        var stateIds = def.States.Select(s => s.Id).ToHashSet();
        if (stateIds.Count != def.States.Count)
            return (false, "Duplicate state IDs are not allowed.");

        // No duplicate action IDs
        if (def.Actions.Select(a => a.Id).Distinct().Count() != def.Actions.Count)
            return (false, "Duplicate action IDs are not allowed.");

        // All referenced state IDs in actions must exist
        foreach (var action in def.Actions)
        {
            if (!stateIds.Contains(action.ToState) || action.FromStates.Any(s => !stateIds.Contains(s)))
                return (false, $"Action '{action.Id}' refers to unknown states.");
        }

        _definitions[def.Id] = def;
        return (true, "Workflow created successfully.");
    }

    // Fetch a workflow definition by ID
    public WorkflowDefinition? GetWorkflow(string id) => _definitions.GetValueOrDefault(id);

    // Start a new instance of a workflow
    public (bool IsSuccess, string Message, WorkflowInstance? Instance) StartInstance(string workflowId)
    {
        if (!_definitions.ContainsKey(workflowId))
            return (false, "Workflow not found.", null);

        var def = _definitions[workflowId];
        var initial = def.States.First(s => s.IsInitial);

        var instance = new WorkflowInstance
        {
            DefinitionId = workflowId,
            CurrentState = initial.Id
        };

        _instances[instance.Id] = instance;
        return (true, "Instance started.", instance);
    }

    // Perform an action on a workflow instance
    public (bool IsSuccess, string Message, WorkflowInstance? Instance) ExecuteAction(string instanceId, string actionId)
    {
        if (!_instances.TryGetValue(instanceId, out var instance))
            return (false, "Instance not found.", null);

        var def = _definitions[instance.DefinitionId];
        var action = def.Actions.FirstOrDefault(a => a.Id == actionId);

        if (action == null)
            return (false, "Action not found in this workflow.", null);

        if (!action.Enabled)
            return (false, "Action is disabled.", null);

        if (!action.FromStates.Contains(instance.CurrentState))
            return (false, "Action is not valid from current state.", null);

        var currentStateObj = def.States.First(s => s.Id == instance.CurrentState);
        if (currentStateObj.IsFinal)
            return (false, "Cannot perform actions from a final state.", null);

        // All good, update instance state and record the action
        instance.CurrentState = action.ToState;
        instance.History.Add((actionId, DateTime.UtcNow));

        return (true, "Action executed.", instance);
    }

    // Get instance state and action history
    public WorkflowInstance? GetInstance(string id) => _instances.GetValueOrDefault(id);
}
