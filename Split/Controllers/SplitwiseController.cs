using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Split;

[Route("api/[controller]")]
[ApiController]
public class SplitwiseController : ControllerBase
{
    private readonly string apiKey = "euWvvGpb8MWeVaN9GyLAdZc9sUaFqp6GMPpLrmUa";
    private readonly HttpClient httpClient;

    public SplitwiseController()
    {
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    [HttpGet("getExpenses")]
    public async Task<IActionResult> GetExpenses()
    {
        string apiUrl = "https://www.splitwise.com/api/v3.0/get_expenses";

        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        else
        {
            return BadRequest("Failed to fetch expenses from Splitwise.");
        }
    }

    [HttpPost("createExpense")]
    public async Task<IActionResult> CreateExpense([FromBody] ExpenseInputModel expenseInput)
    {
        string apiUrl = "https://www.splitwise.com/api/v3.0/create_expense";

        // Create a new expense object and populate its properties based on your input model
        var newExpense = new
        {
            description = expenseInput.Description,
            cost = expenseInput.Cost,
            currency_code = "USD", // Adjust as needed
                                   // ... other properties as needed
        };

        var content = new StringContent(JsonConvert.SerializeObject(newExpense), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            return Ok(responseContent);
        }
        else
        {
            return BadRequest("Failed to create expense on Splitwise.");
        }
    }

}
