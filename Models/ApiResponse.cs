namespace LoginCrudApi.Models
{
    public class ApiResponse
    {
        public object? Data { get; set; }
        public bool Success { get; set; }
        public int HttpStatus { get; set; }
    }
}
