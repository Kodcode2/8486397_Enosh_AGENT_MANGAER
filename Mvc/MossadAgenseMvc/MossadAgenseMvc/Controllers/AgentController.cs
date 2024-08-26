using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using MossadAgenseMvc.Models;

namespace MossadAgenseMvc.Controllers
{
    public class AgentController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string BaseUrl = "https://localhost:7024";
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var responce = await httpClient.GetAsync($"{BaseUrl}/agents");
            if (responce.IsSuccessStatusCode)
            {
                string content = await responce.Content.ReadAsStringAsync();
                List<AgentModel>? Agents = JsonSerializer.Deserialize<List<AgentModel>>
                    (content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(Agents);
            }
            return RedirectToAction("Index", "Home");

        }

        public IActionResult Update(int id)
        {
            return View(new AgentModel() { Id = id});
        }

        
    }
}
