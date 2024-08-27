using Microsoft.AspNetCore.Mvc;
using MossadAgenseMvc.Models;
using System.Text.Json;

namespace MossadAgenseMvc.Controllers
{
    public class TargetController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string BaseUrl = "https://localhost:7024";
        [HttpGet]//הצגת כל היעדים
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
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
