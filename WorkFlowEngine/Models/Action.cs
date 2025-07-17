namespace WorkflowEngine.Models;

// Represents a transition (action) that moves the workflow from one state to another
public class Action
{
    public string Id { get; set; } = default!; // Unique ID of the action
    public string Name { get; set; } = default!; // Display name
    public List<string> FromStates { get; set; } = new(); // Allowed source states
    public string ToState { get; set; } = default!; // Target state to transition to
    public bool Enabled { get; set; } = true; // Can the action be executed now?
}
