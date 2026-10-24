namespace LoginCrudApi.Models.Todo
{
    public sealed class TodoItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string Category { get; set; } = "";
        public string Priority { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTodoRequest
    {
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string Category { get; set; } = "";
        public string Priority { get; set; } = "";
    }

    public sealed class UpdateTodoRequest : CreateTodoRequest { }

    public sealed class UpdateTodoStatusRequest
    {
        public string Status { get; set; } = "";
    }

    public sealed class TodoDashboardStats
    {
        public int TotalTodos { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
        public int HighPriority { get; set; }
    }
}