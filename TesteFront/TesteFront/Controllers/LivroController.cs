using TesteFront.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace TesteFront.Controllers
{
    public class LivroController : Controller
    {
        private readonly HttpClient _httpClient;

        public LivroController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("LivrosApiClient");
        }


        public async Task<IActionResult> Index(int? ano, int? mes)
        {
            try
            {
                string url = "Livros";

                if (ano.HasValue && mes.HasValue)
                {
                    url += $"?ano={ano.Value}&mes={mes.Value}";
                }
                var tagsResponse = await _httpClient.GetAsync("Tag");
                var tagsJson = await tagsResponse.Content.ReadAsStringAsync();
                if (tagsJson.Contains("Nenhuma tag encontrada."))
                {
                    ViewBag.ErrorMessage = "Adicione ao menos uma tag.";
                    return View(new List<Livro>());
                }
                var tags = JsonConvert.DeserializeObject<List<Tag>>(tagsJson) ?? new List<Tag>();

                var tiposEncResponse = await _httpClient.GetAsync("TipoEncadernacao");
                var tiposEncJson = await tiposEncResponse.Content.ReadAsStringAsync();
                if (tiposEncJson.Contains("Nenhum tipo de encadernação encontrado."))
                {
                    ViewBag.ErrorMessage = "Adicione ao menos um tipo de encadernação.";
                    return View(new List<Livro>());
                }
                var tiposEncadernacao = JsonConvert.DeserializeObject<List<TipoEncadernacao>>(tiposEncJson) ?? new List<TipoEncadernacao>();


                ViewBag.CanAddNewBook = tags.Any() && tiposEncadernacao.Any();
 
                var response = await _httpClient.GetAsync(url);
                var livros = new List<Livro>();
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    livros = JsonConvert.DeserializeObject<List<Livro>>(jsonData);
                }
                else
                {
                    ViewBag.ErrorMessage = "A lista de Livro está Vazia";
                    return View(new List<Livro>());
                }

                return View(livros);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                ViewBag.CanAddNewBook = false; 
                return View(new List<Livro>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Livros/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var livro = JsonConvert.DeserializeObject<Livro>(jsonData);
                    return View(livro);
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao obter detalhes do livro.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: /Livro/Create
        public async Task<IActionResult> Create()
        {
            var tagsResponse = await _httpClient.GetAsync("Tag");
            var tagsJson = await tagsResponse.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<List<Tag>>(tagsJson);

            var tiposEncResponse = await _httpClient.GetAsync("TipoEncadernacao");
            var tiposEncJson = await tiposEncResponse.Content.ReadAsStringAsync();
            var tiposEncadernacao = JsonConvert.DeserializeObject<List<TipoEncadernacao>>(tiposEncJson);

            // Check if there is at least one tag and one tipo de encadernação
            if (tags == null || !tags.Any() || tiposEncadernacao == null || !tiposEncadernacao.Any())
            {
                ViewBag.ErrorMessage = "Para adicionar um novo livro, é necessário ter pelo menos uma Tag e um Tipo de Encadernação cadastrados.";
                return RedirectToAction("Index");
            }

            ViewBag.Tags = tags;
            ViewBag.TiposEncadernacao = tiposEncadernacao;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Livro livro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (livro.LivroDigital != null && livro.LivroDigital.Formato == null)
                    {
                        livro.LivroDigital = null;
                    }

                    if (livro.LivroImpresso != null && livro.LivroImpresso.TipoEncadernacaoID == 0)
                    {
                        livro.LivroImpresso = null;
                    }

                    var jsonContent = JsonConvert.SerializeObject(livro);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("Livros", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Erro ao cadastrar livro.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                }
            }

            await LoadTagsAndEncadernacao();
            return View(livro);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Livros/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var livro = JsonConvert.DeserializeObject<Livro>(jsonData);
                    var tagsResponse = await _httpClient.GetAsync("Tag");
                    var tagsJson = await tagsResponse.Content.ReadAsStringAsync();
                    var tags = JsonConvert.DeserializeObject<List<Tag>>(tagsJson);

                    var tiposEncResponse = await _httpClient.GetAsync("TipoEncadernacao");
                    var tiposEncJson = await tiposEncResponse.Content.ReadAsStringAsync();
                    var tiposEncadernacao = JsonConvert.DeserializeObject<List<TipoEncadernacao>>(tiposEncJson);
                    ViewBag.Tags = tags;
                    ViewBag.TiposEncadernacao = tiposEncadernacao;

                    return View(livro);
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao obter detalhes do livro.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Livro livro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (livro.LivroDigital != null && string.IsNullOrEmpty(livro.LivroDigital.Formato))
                    {
                        livro.LivroDigital = null;
                    }

                    if (livro.LivroImpresso != null && livro.LivroImpresso.TipoEncadernacaoID == 0)
                    {
                        livro.LivroImpresso = null;
                    }

                    var jsonContent = JsonConvert.SerializeObject(livro);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"Livros/{livro.Codigo}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Erro ao editar livro.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                }
            }

            return View(livro);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Livros/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var livro = JsonConvert.DeserializeObject<Livro>(jsonData);
                    return View(livro);
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao obter detalhes do livro.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Livros/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Erro ao deletar livro.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao conectar à API: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        private async Task LoadTagsAndEncadernacao()
        {
            var tagsResponse = await _httpClient.GetAsync("Tags");
            var tagsJson = await tagsResponse.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<List<Tag>>(tagsJson);

            var tiposEncResponse = await _httpClient.GetAsync("TipoEncadernacao");
            var tiposEncJson = await tiposEncResponse.Content.ReadAsStringAsync();
            var tiposEncadernacao = JsonConvert.DeserializeObject<List<TipoEncadernacao>>(tiposEncJson);

            ViewBag.Tags = tags;
            ViewBag.TiposEncadernacao = tiposEncadernacao;
        }
    }
}
