using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Microsoft.Extensions.Configuration;
using SMSHub.Core.Entities;
using SMSHub.Core.Services;




    public class SmsService:ISmsService
    {
        private HttpClient _httpClient;
        private string _accountSid;
        private string _authToken;
        private string _defaultFromNumber;
        private string _baseUrl;
        private SemaphoreSlim _rateLimiter;

        public SmsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _defaultFromNumber = configuration["Twilio:DefaultFromNumber"];
            _baseUrl = $"https://api.twilio.com/2010-04-01/Accounts/{_accountSid}/Messages.json";
            _rateLimiter = new SemaphoreSlim(1, 1);

            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_accountSid}:{_authToken}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

    public string DefaultFromNumber => throw new NotImplementedException();

    public async Task<bool> SendSmsAsync(ICollection<string> to, string message, string fromNumber)
        {
            bool overallSuccess = true;
            foreach (var recipient in to)
            {
                await _rateLimiter.WaitAsync(); // Apply rate limiting
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("To", recipient),
                    new KeyValuePair<string, string>("From", fromNumber),
                    new KeyValuePair<string, string>("Body", message),
                });

                    var response = await _httpClient.PostAsync(_baseUrl, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        overallSuccess = false;
                    }
                }
                finally
                {
                    _rateLimiter.Release();
                }
            }
            return overallSuccess;
        }

  
}


//send to multiple recepients
/*public async Task<bool> SendSmsAsync(IEnumerable<string> toNumbers, string message, IEnumerable<string> fromNumbers)
{
    bool overallSuccess = true;
    foreach (var to in toNumbers)
    {
        foreach (var from in fromNumbers)
        {
            await _rateLimiter.WaitAsync(); // Apply rate limiting
            try
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("To", to),
                    new KeyValuePair<string, string>("From", from),
                    new KeyValuePair<string, string>("Body", message),
                });

                var response = await _httpClient.PostAsync(_baseUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    overallSuccess = false;
                }
            }
            finally
            {
                _rateLimiter.Release();
            }
        }
    }
    return overallSuccess;
}
*/













//public class SmsService : ISmsService
//{
//    private readonly HttpClient _httpClient;
//    private readonly string _accountSid;
//    private readonly string _authToken;
//    private readonly string _baseUrl;
//    private readonly string _defaultFromNumber;

//    public SmsService(HttpClient httpClient, IConfiguration configuration)
//    {
//        _httpClient = httpClient;
//        _accountSid = configuration["Twilio:AccountSid"];
//        _authToken = configuration["Twilio:AuthToken"];
//        _defaultFromNumber = configuration["Twilio:DefaultFromNumber"];
//        _baseUrl = $"https://api.twilio.com/2010-04-01/Accounts/{_accountSid}/Messages.json";

//        // Configure HttpClient for Basic Auth
//        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_accountSid}:{_authToken}"));
//        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
//    }

//    public string DefaultFromNumber => _defaultFromNumber;

//    public async Task<bool> SendSmsAsync(string to, string message, string from = null)
//    {
//        var requestUri = _baseUrl;
//        var content = new FormUrlEncodedContent(new[]
//        {
//            new KeyValuePair<string, string>("To", to),
//            new KeyValuePair<string, string>("From", from ?? _defaultFromNumber),
//            new KeyValuePair<string, string>("Body", message),
//        });

//        var response = await _httpClient.PostAsync(requestUri, content);

//        if (!response.IsSuccessStatusCode)
//        {
//            // Log or handle the error
//            return false;
//        }

//        return true;
//    }
//}
