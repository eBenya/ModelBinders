using Microsoft.AspNetCore.Mvc;

namespace JsonWithFilesBindingExample.Controllers;

[ApiController]
[Route("api")]
public class ExampleController : Controller
{
    private readonly ILogger<ExampleController> _logger;

    public ExampleController(ILogger<ExampleController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("do-some-with-data")]
    public IActionResult DoSomethingWithThisData(ExampleRequestModel requestModel)
    {
        return Ok("Файлы с моделью обработаны");
    }
    
    [HttpGet("check")]
    public IActionResult Test()
    {
        return Ok("Ok");
    }
}

//[ModelBinder(typeof(JsonWithFilesFormModelBinder), Name = nameof(ExampleRequestModel))]
public record ExampleRequestModel
{
    public string Name { get; init; }
    public int Age { get; init; }
     
    public IFormFile File { get; init; }
    public ICollection<IFormFile> OtherFiles { get; init; }
}