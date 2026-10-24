namespace LoginCrudApi.Models
{
    public class LoginModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string? FaceDescriptor { get; set; }
    }
}
