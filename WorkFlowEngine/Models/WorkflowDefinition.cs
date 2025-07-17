namespace WorkflowEngine.Models;

// A complete definition of a workflow, including its states and transitions (actions)
public class WorkflowDefinition
{
    public string Id { get; set; } = default!; // Unique ID of the workflow
    public List<State> States { get; set; } = new(); // All possible states
    public List<Action> Actions { get; set; } = new(); // All transitions/actions
}
