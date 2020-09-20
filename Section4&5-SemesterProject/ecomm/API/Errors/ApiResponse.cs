using System;

namespace API.Errors
{
    public class ApiResponse
    {
        
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            //null coalescent ?? reads if message null do whatever after ??
            Message = message ?? GetDefaultMessageForStatusCode(statusCode) ;
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch 
            {
                400 => "Bad request has been made",
                401 => "Not Authorized",
                404 => "Resource not found",
                500 => "Errors were found",
                _ => null  //default
            };
        }

    }
}