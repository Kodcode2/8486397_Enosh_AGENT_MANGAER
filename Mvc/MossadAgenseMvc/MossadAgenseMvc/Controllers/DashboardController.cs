using Microsoft.AspNetCore.Mvc;
using MossadAgenseMvc.Models;
using System.Text.Json;

namespace MossadAgenseMvc.Controllers
{
    public class DashboardController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string BaseUrl = "https://localhost:7024/missions";
        [HttpGet]
        public async Task<IActionResult> Index()//הצגת נתונים כללים על סטטוס הפרוייקט
        {
            var httpClient = clientFactory.CreateClient();
                 var responceAgent = await httpClient.GetAsync($"{BaseUrl}/Dashboard");
            if (responceAgent.IsSuccessStatusCode)
            {

                string content = await responceAgent.Content.ReadAsStringAsync();
                Dashboard? res = JsonSerializer.Deserialize<Dashboard>
                    (content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(res);
            }
            return View(null);
        }
    }
}
