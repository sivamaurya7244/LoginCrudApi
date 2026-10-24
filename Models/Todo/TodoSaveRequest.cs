namespace LoginCrudApi.Models.Todo
{
    public class TodoSaveRequest
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Category { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;
    }
}
