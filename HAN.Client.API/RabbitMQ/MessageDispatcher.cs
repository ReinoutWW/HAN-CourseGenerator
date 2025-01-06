using System.Net.Http.Headers;
using System.Text;

namespace HAN.Client.API.RabbitMQ;

public class MessageDispatcher
{
    private readonly HttpClient _httpClient;

    public MessageDispatcher(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> DispatchAsync(Message message)
    {
        var request = new HttpRequestMessage(new HttpMethod(message.Method), message.Endpoint);
        
        request.RequestUri = new Uri($"https://localhost:5005/{message.Endpoint}");

        if (!string.IsNullOrWhiteSpace(message.Payload) && 
            (message.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
             message.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase)  ||
             message.Method.Equals("PATCH", StringComparison.OrdinalIgnoreCase)))
        {
            request.Content = new StringContent(message.Payload, Encoding.UTF8, "application/json");
        }

        if (!string.IsNullOrWhiteSpace(message.AuthToken))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", message.AuthToken);
        }

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }
}