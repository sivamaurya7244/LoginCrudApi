namespace LoginCrudApi.Models
{
    public class ApiResponse
    {
        public object? Data { get; set; }
        public bool Success { get; set; }
        public int HttpStatus { get; set; }

        //public  ApiResponse(object? data, bool success, int httpStatus)
        //{
        //    Data = data;
        //    Success = success;
        //    HttpStatus = httpStatus;
        //}
    }
}
