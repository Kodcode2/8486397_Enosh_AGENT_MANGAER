using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace MossadAgenseMvc.Controllers
{
    public class AgentController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string BaseUrl = "https://localhost:7024/api/Agents";
        public IActionResult Index()
        {
            return View();
        }

        /*public IActionResult Update(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserVM user)
        {
            var httpClient = clientFactory.CreateClient();
            var httpContent = new StringContent(
                JsonSerializer.Serialize(new
                {
                    user.Name,
                    user.Email,
                    user.Password,
                    //Image = ImageUtils.ConvertFromIformFile(user.Image),
                }),
                Encoding.UTF8,
                "application/json"
            );
            var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/update/{user.Id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authenticatin.Token);
            ///httpContent;
            var result = await httpClient.PutAsync($"{BaseUrl}/update/{user.Id}", httpContent);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }*/
    }
}
