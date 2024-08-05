using Microsoft.AspNetCore.Mvc;
using TesteFront.Models;
using Newtonsoft.Json;
using System.Text;

namespace TesteFront.Controllers
{
    public class TagController : Controller
    {
        private readonly HttpClient _httpClient;

        public TagController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("LivrosApiClient");
        }

        // GET: /Tag/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("Tag"); 
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var tags = JsonConvert.DeserializeObject<List<Tag>>(jsonData);
                    return View(tags); 
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao obter lista de tags.";
                    return View(new List<Tag>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                return View(new List<Tag>());
            }
        }

        // GET: /Tag/Create
        public IActionResult Create()
        {
            return View(); 
        }

        // POST: /Tag/Create
        [HttpPost]
        public async Task<IActionResult> Create(Tag tag)
        {
                 try
                 {
                    var jsonContent = JsonConvert.SerializeObject(tag);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("Tag", content); 
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Erro ao cadastrar tag.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                }
            

            return View(tag); 
        }

        // GET: /Tag/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Tag/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var tag = JsonConvert.DeserializeObject<Tag>(jsonData);
                    return View(tag); 
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao obter detalhes da tag.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: /Tag/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(Tag tag)
        {
            
                try
                {
                    var jsonContent = JsonConvert.SerializeObject(tag);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"Tag/{tag.Codigo}", content); 
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Erro ao editar tag.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                }
            

            return View(tag); 
        }

        // GET: /Tag/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Tag/{id}"); 
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var tag = JsonConvert.DeserializeObject<Tag>(jsonData);
                    return View(tag); 
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao obter detalhes da tag.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: /Tag/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Tag/{id}"); 
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao deletar tag.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
