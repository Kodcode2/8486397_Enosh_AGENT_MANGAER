using Microsoft.AspNetCore.Mvc;
using MossadAgenseMvc.Models;
using System.Text.Json;

namespace MossadAgenseMvc.Controllers
{
    public class TargetController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string BaseUrl = "https://localhost:7024";
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            //var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/Targets");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authenticatin.Token);
            var responce = await httpClient.GetAsync($"{BaseUrl}/targets");
            if (responce.IsSuccessStatusCode)
            {
                string content = await responce.Content.ReadAsStringAsync();
                List<TargetModel>? Targets = JsonSerializer.Deserialize<List<TargetModel>>
                    (content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(Targets);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
