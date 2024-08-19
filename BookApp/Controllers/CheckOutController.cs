using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BookApp.Controllers
{
    public class CheckoutController : Controller
    {
        public string PaypalClientId { get; set; } = "";
        public string PaypalSecret { get; set; } = "";
        public string PaypalUrl { get; set; } = "";

        public CheckoutController(IConfiguration con)
        {
            PaypalClientId = con["PayPalSettings:ClientId"]!;
            PaypalSecret = con["PayPalSettings:Secret"]!;
            PaypalUrl = con["PayPalSettings:Url"]!;
        }

        public IActionResult Index()
        {
            ViewBag.PaypalClientId = PaypalClientId;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CompleteOrder([FromBody] JsonObject data)
        {
            var orderId = data?["orderID"]?.ToString();
            if (orderId == null)
            {
                return new JsonResult("error");
            }
            string accessToken = await GetPaypalAccessToken();

            string url = PaypalUrl + "/v2/checkout/orders/" + orderId + "/capture";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("", null, "application/json");
                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStr = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(responseStr);
                    if (jsonResponse != null)
                    {
                        string paypalOrderStatus = jsonResponse["status"]?.ToString() ?? "";
                        if (paypalOrderStatus == "COMPLETED")
                        {
                            return new JsonResult("success");
                        }
                    }
                }
            }

            return new JsonResult("error");
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrder([FromBody] JsonObject data)
        {
            var totalAmount = data?["amount"]?.ToString();
            if (totalAmount == null)
            {
                return new JsonResult(new { Id = "" });
            }

            // Create the request body
            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");

            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            amount.Add("value", totalAmount);

            JsonObject purchaseUnit1 = new JsonObject();
            purchaseUnit1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit1);

            createOrderRequest.Add("purchase_units", purchaseUnits);

            // Get access token
            string accessToken = await GetPaypalAccessToken();

            string url = $"{PaypalUrl}/v2/checkout/orders";

            using (var client = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), Encoding.UTF8, "application/json");
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStr = await httpResponse.Content.ReadAsStringAsync();
                    using (var jsonDoc = JsonDocument.Parse(responseStr))
                    {
                        var jsonRoot = jsonDoc.RootElement;
                        if (jsonRoot.TryGetProperty("id", out var idProperty))
                        {
                            string paypalOrderId = idProperty.GetString() ?? "";
                            return new JsonResult(new { Id = paypalOrderId });
                        }
                    }
                }
            }

            return new JsonResult(new { Id = "" });
        }


        private async Task<string> GetPaypalAccessToken()
        {
            string token = "";

            string url = $"{PaypalUrl}/v1/oauth2/token";
            using (var client = new HttpClient())
            {
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{PaypalClientId}:{PaypalSecret}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                var requestContent = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await client.PostAsync(url, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonDocument.Parse(responseString).RootElement;
                    if (jsonResponse.TryGetProperty("access_token", out var accessToken))
                    {
                        token = accessToken.GetString()!;
                    }
                }
                else
                {
                    // Log the response status code and content
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, {errorResponse}");
                }
            }
            return token;
        }
    }
}
