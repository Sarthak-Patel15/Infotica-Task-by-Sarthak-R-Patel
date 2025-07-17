namespace WorkflowEngine.Models;

// Represents a state in the workflow (e.g., "Draft", "Approved", "Done")
public class State
{
    public string Id { get; set; } = default!; // Unique ID of the state
    public string Name { get; set; } = default!; // Display name
    public bool IsInitial { get; set; } // Is this the starting state?
    public bool IsFinal { get; set; } // Is this the ending state?
    public bool Enabled { get; set; } = true; // Can the state be used currently?
}
