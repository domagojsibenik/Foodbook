using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Foodbook.Controllers
{
    public class AssistantRequest
    {
        [Required]
        [MaxLength(2000)]
        public string Message { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/assistant")]
    public class AssistantController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AssistantController> _logger;

        public AssistantController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<AssistantController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Ask(
            [FromBody] AssistantRequest request,
            CancellationToken cancellationToken)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            var model = _configuration["OpenAI:Model"];

            if (string.IsNullOrWhiteSpace(apiKey)
                || string.IsNullOrWhiteSpace(model))
            {
                return Problem("Nedostaje OpenAI konfiguracija.");
            }

            var tools = JsonSerializer.Deserialize<JsonElement>(
                """
                [
                  {
                    "type": "function",
                    "name": "get_recipes",
                    "description": "Dohvati sve recepte ili recepte korisnika čiji je točan ID poznat. Nema pretrage po nazivu, sastojcima, vremenu pripreme ili sortiranja.",
                    "strict": true,
                    "parameters": {
                      "type": "object",
                      "properties": {
                        "userId": {
                          "type": ["string", "null"],
                          "description": "Točan ID korisnika ili null za sve recepte."
                        }
                      },
                      "required": ["userId"],
                      "additionalProperties": false
                    }
                  },
                  {
                    "type": "function",
                    "name": "get_recipe_by_id",
                    "description": "Dohvati jedan recept prema poznatom pozitivnom cjelobrojnom ID-u.",
                    "strict": true,
                    "parameters": {
                      "type": "object",
                      "properties": {
                        "id": {
                          "type": "integer",
                          "description": "ID recepta."
                        }
                      },
                      "required": ["id"],
                      "additionalProperties": false
                    }
                  }
                ]
                """
            );

            var openAi = _httpClientFactory.CreateClient("OpenAI");

            using var openAiRequest =
                new HttpRequestMessage(HttpMethod.Post, "responses");

            openAiRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            openAiRequest.Content = JsonContent.Create(new
            {
                model,
                instructions = """
                    Pomažeš korisniku dohvatiti podatke iz Foodbook API-ja.
                    Koristi dostupne funkcije kada podržavaju zahtjev.
                    Ne izmišljaj ID-eve korisnika ni recepata.
                    Ako nedostaje potreban ID, zatraži ga.
                    Ako traženi filtar nije podržan, objasni ograničenje
                    i nemoj pozivati funkciju kao da taj filtar postoji.
                    Odgovaraj na hrvatskom.
                    """,
                input = request.Message,
                tools,
                tool_choice = "auto",
                parallel_tool_calls = false
            });

            using var openAiResponse = await openAi.SendAsync(
                openAiRequest, cancellationToken);

            if (!openAiResponse.IsSuccessStatusCode)
            {
                var error = await openAiResponse.Content
                    .ReadAsStringAsync(cancellationToken);

                _logger.LogError(
                    "OpenAI greška {Status}: {Error}",
                    openAiResponse.StatusCode, error);

                return Problem(
                    statusCode: 502,
                    detail: "Poziv OpenAI API-ja nije uspio. Provjeri backend log.");
            }

            using var document = JsonDocument.Parse(
                await openAiResponse.Content
                    .ReadAsStringAsync(cancellationToken));

            var output = document.RootElement.GetProperty("output");

            foreach (var item in output.EnumerateArray())
            {
                if (item.GetProperty("type").GetString() != "function_call")
                    continue;

                var name = item.GetProperty("name").GetString();
                var argumentsJson =
                    item.GetProperty("arguments").GetString()!;

                using var argumentsDocument =
                    JsonDocument.Parse(argumentsJson);

                var arguments = argumentsDocument.RootElement;

                string path;

                switch (name)
                {
                    case "get_recipes":
                        {
                            var userId = arguments
                                .GetProperty("userId")
                                .GetString();

                            path = userId is null
                                ? "api/recipe"
                                : $"api/recipe?UserId={Uri.EscapeDataString(userId)}";

                            break;
                        }

                    case "get_recipe_by_id":
                        {
                            var id = arguments.GetProperty("id").GetInt32();

                            if (id <= 0)
                                return BadRequest("ID recepta mora biti pozitivan.");

                            path = $"api/recipe/{id}";
                            break;
                        }

                    default:
                        return Problem("Model je odabrao nepoznatu funkciju.");
                }

                var foodbook = _httpClientFactory.CreateClient("Foodbook");

                using var apiResponse = await foodbook.GetAsync(
                    path, cancellationToken);

                var body = await apiResponse.Content
                    .ReadAsStringAsync(cancellationToken);

                JsonElement? data = null;

                if (!string.IsNullOrWhiteSpace(body))
                {
                    using var resultDocument = JsonDocument.Parse(body);
                    data = resultDocument.RootElement.Clone();
                }

                return Ok(new
                {
                    function = name,
                    arguments = arguments.Clone(),
                    endpoint = $"GET /{path}",
                    statusCode = (int)apiResponse.StatusCode,
                    data
                });
            }

        
            var texts = new List<string>();

            foreach (var item in output.EnumerateArray())
            {
                if (item.GetProperty("type").GetString() != "message")
                    continue;

                foreach (var content in item
                    .GetProperty("content").EnumerateArray())
                {
                    if (content.GetProperty("type").GetString() == "output_text")
                    {
                        texts.Add(content.GetProperty("text").GetString()!);
                    }
                }
            }

            return Ok(new
            {
                message = string.Join("\n", texts)
            });
        }
    }
}