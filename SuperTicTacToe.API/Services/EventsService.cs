using SuperTicTacToe.API.Enums.Events;
using System.Text.Json;

namespace SuperTicTacToe.API.Services
{
    public class EventsService
    {
        public readonly List<HttpResponse> BindedResponses;

        public EventsService() {
            BindedResponses = new List<HttpResponse>();
        }

        public void BindResponse(HttpResponse response) {
            BindedResponses.Add(response);
        }

        public void UnbindResponse(HttpResponse response) {
            BindedResponses.Remove(response);
        }

        public void SendEvent(EventHeader header, object? data = null)
            => SendEventAsync(header, data).Wait();
        public async Task SendEventAsync(EventHeader header, object? data = null) {
            foreach (var response in BindedResponses) {
                await response.WriteAsync($"event: {header}\rdata: {JsonSerializer.Serialize(data)}\r\r");
                await response.Body.FlushAsync();
            }
        }
    }
}
