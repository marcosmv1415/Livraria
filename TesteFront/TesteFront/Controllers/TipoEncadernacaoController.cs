using Microsoft.AspNetCore.Mvc;
using TesteFront.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;

namespace TesteFront.Controllers
{
    public class TipoEncadernacaoController : Controller
    {
        private readonly HttpClient _httpClient;

        public TipoEncadernacaoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("LivrosApiClient");
        }

        // GET: /TipoEncadernacao/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("TipoEncadernacao");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var tipos = JsonConvert.DeserializeObject<List<TipoEncadernacao>>(jsonData);
                    return View(tipos);
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao obter lista de tipos de encadernação.";
                    return View(new List<TipoEncadernacao>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                return View(new List<TipoEncadernacao>());
            }
        }

        // GET: /TipoEncadernacao/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /TipoEncadernacao/Create
        [HttpPost]
        public async Task<IActionResult> Create(TipoEncadernacao tipoEncadernacao)
        {
            
                try
                {
                    var jsonContent = JsonConvert.SerializeObject(tipoEncadernacao);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("TipoEncadernacao", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Erro ao cadastrar tipo de encadernação.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                }
            

            return View(tipoEncadernacao);
        }

        // GET: /TipoEncadernacao/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"TipoEncadernacao/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var tipoEncadernacao = JsonConvert.DeserializeObject<TipoEncadernacao>(jsonData);
                    return View(tipoEncadernacao);
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao obter detalhes do tipo de encadernação.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: /TipoEncadernacao/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(TipoEncadernacao tipoEncadernacao)
        {
            
                try
                {
                    var jsonContent = JsonConvert.SerializeObject(tipoEncadernacao);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"TipoEncadernacao/{tipoEncadernacao.Codigo}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Erro ao editar tipo de encadernação.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                }
            

            return View(tipoEncadernacao);
        }

        // GET: /TipoEncadernacao/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"TipoEncadernacao/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var tipoEncadernacao = JsonConvert.DeserializeObject<TipoEncadernacao>(jsonData);
                    return View(tipoEncadernacao);
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao obter detalhes do tipo de encadernação.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: /TipoEncadernacao/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"TipoEncadernacao/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao deletar tipo de encadernação.";
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
