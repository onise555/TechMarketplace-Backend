using System.Text;
using System.Text.Json;
using TechMarketplace.API.Models.Payments;

namespace TechMarketplace.API.Services
{
    public class PayPalService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;

        public PayPalService(IConfiguration configuration, HttpClient httpClient)
        {
            _clientId = configuration["PayPal:ClientId"];
            _clientSecret = configuration["PayPal:ClientSecret"];
            var environment = configuration["PayPal:Environment"];

            _baseUrl = environment == "Sandbox"
                ? "https://api.sandbox.paypal.com"
                : "https://api.paypal.com";

            _httpClient = httpClient;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));

                var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/v1/oauth2/token");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", auth);
                request.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                });

                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"PayPal Token Error: {json}");
                }

                var tokenData = JsonSerializer.Deserialize<JsonElement>(json);
                return tokenData.GetProperty("access_token").GetString();
            }
            catch (Exception ex)
            {
                throw new Exception($"PayPal Authentication failed: {ex.Message}");
            }
        }

        public async Task<PayPalOrderResult> CreateOrderAsync(decimal amount)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();

                var orderData = new
                {
                    intent = "CAPTURE",
                    purchase_units = new[]
                    {
                        new
                        {
                            amount = new
                            {
                                currency_code = "USD",
                                value = amount.ToString("F2")
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(orderData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/v2/checkout/orders");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                request.Content = content;

                var response = await _httpClient.SendAsync(request);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"PayPal Order Creation Error: {responseJson}");
                }

                var orderResponse = JsonSerializer.Deserialize<JsonElement>(responseJson);

                return new PayPalOrderResult
                {
                    OrderId = orderResponse.GetProperty("id").GetString(),
                    Status = orderResponse.GetProperty("status").GetString(),
                    ApprovalUrl = GetApprovalUrl(orderResponse)
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"PayPal Order Creation failed: {ex.Message}");
            }
        }

        public async Task<PayPalCaptureResult> CaptureOrderAsync(string paypalOrderId)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();

                var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/v2/checkout/orders/{paypalOrderId}/capture");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                request.Content = new StringContent("{}", Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var responseJson = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"PayPal Capture Response: {responseJson}");

                if (!response.IsSuccessStatusCode)
                {
                    return new PayPalCaptureResult
                    {
                        IsSuccess = false,
                        Status = "FAILED",
                        ErrorMessage = $"PayPal Error: {response.StatusCode} - {responseJson}"
                    };
                }

                var captureResponse = JsonSerializer.Deserialize<JsonElement>(responseJson);

                // PayPal Capture Response შემოწმება
                if (captureResponse.TryGetProperty("status", out var statusProperty))
                {
                    var status = statusProperty.GetString();
                    return new PayPalCaptureResult
                    {
                        IsSuccess = status == "COMPLETED",
                        Status = status,
                        CaptureId = captureResponse.TryGetProperty("id", out var idProp) ? idProp.GetString() : paypalOrderId,
                        RawResponse = responseJson
                    };
                }

                return new PayPalCaptureResult
                {
                    IsSuccess = false,
                    Status = "UNKNOWN",
                    ErrorMessage = "Invalid PayPal response structure",
                    RawResponse = responseJson
                };
            }
            catch (Exception ex)
            {
                return new PayPalCaptureResult
                {
                    IsSuccess = false,
                    Status = "ERROR",
                    ErrorMessage = ex.Message
                };
            }
        }

        private string GetApprovalUrl(JsonElement orderResponse)
        {
            if (orderResponse.TryGetProperty("links", out var links))
            {
                foreach (var link in links.EnumerateArray())
                {
                    if (link.TryGetProperty("rel", out var rel) && rel.GetString() == "approve")
                    {
                        if (link.TryGetProperty("href", out var href))
                        {
                            return href.GetString();
                        }
                    }
                }
            }
            return "";
        }
    }



    public class PayPalCaptureResult
    {
        public bool IsSuccess { get; set; }
        public string Status { get; set; }
        public string CaptureId { get; set; }
        public string ErrorMessage { get; set; }
        public string RawResponse { get; set; }
    }
}

