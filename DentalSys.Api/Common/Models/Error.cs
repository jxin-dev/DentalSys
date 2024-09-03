namespace DentalSys.Api.Common.Models
{
    public class Error
    {
        public Error(string code, string description, int statusCode)
        {
            Code = code;
            Description = description;
            StatusCode = statusCode;
        }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public int StatusCode { get; private set; }
    }
}
