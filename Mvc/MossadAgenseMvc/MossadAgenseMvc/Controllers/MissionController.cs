using Microsoft.AspNetCore.Mvc;
using MossadAgenseMvc.Models;
using System.Text.Json;

namespace MossadAgenseMvc.Controllers
{
    public class MissionController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string BaseUrl = "https://localhost:7024";
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            
            var responce = await httpClient.GetAsync($"{BaseUrl}/missions");
            if (responce.IsSuccessStatusCode)
            {
                string content = await responce.Content.ReadAsStringAsync();
                List<MissionModel>? res = JsonSerializer.Deserialize<List<MissionModel>>
                    (content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(res);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UpdateToActive(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var httpContent = new StringContent("");
            var responce = await httpClient.PutAsync($"{BaseUrl}/missions/active/{id}", httpContent);
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var responce = await httpClient.GetAsync($"{BaseUrl}/missions/id?id={id}");
            if (responce.IsSuccessStatusCode)
            {
                string content = await responce.Content.ReadAsStringAsync();
                MissionModel? res = JsonSerializer.Deserialize<MissionModel>
                    (content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(res);
            }
            return RedirectToAction("Index");
        }


    }
}
