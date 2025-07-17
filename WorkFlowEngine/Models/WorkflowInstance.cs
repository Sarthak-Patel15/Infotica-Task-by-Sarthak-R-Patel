namespace WorkflowEngine.Models;

// Represents a running workflow based on a definition
public class WorkflowInstance
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); // Unique instance ID
    public string DefinitionId { get; set; } = default!; // Reference to the definition used
    public string CurrentState { get; set; } = default!; // Current state of the workflow
    public List<(string ActionId, DateTime Timestamp)> History { get; set; } = new(); // All actions performed
}
