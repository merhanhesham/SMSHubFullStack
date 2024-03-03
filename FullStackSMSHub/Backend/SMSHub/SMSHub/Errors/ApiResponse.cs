namespace SMSHub.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statuscode,string message=null) { 
            StatusCode = statuscode;
            Message = message?? GetDefaultMessageForStatusCode(statuscode);//lw msg mgtsh mn elerror a7otelo def
        }

        private string GetDefaultMessageForStatusCode(int statuscode)
        {
            return statuscode switch
            {
                400 => "BadRequest",
                401 => "You are not authorized",
                404 => "Resource Not Found",
                500 =>"Internal server error",
                _ => null
            };
        }
    }
}
