using Microsoft.AspNetCore.Mvc;
using ModelBinders.MVC;

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
        var files = new List<IFormFile>(requestModel.OtherFiles);
        files.Add(requestModel.File);
        
        var fileAttributes = files.Select(f => new
        {
            FormName = f.Name,
            f.FileName,
            f.Length,
            f.ContentType
        });
        
        return Ok(new
        {
            FromJsonName = requestModel.Name,
            FromJsonAge = requestModel.Age,
            FileAttributes = fileAttributes,
        });
    }
    
    [HttpGet("check")]
    public IActionResult Test()
    {
        return Ok("Ok");
    }
}

[ModelBinder(typeof(JsonWithFilesFormModelBinder), Name = nameof(ExampleRequestModel))]
public record ExampleRequestModel
{
    public string Name { get; init; }
    public int Age { get; init; }
     
    public IFormFile File { get; init; }
    public ICollection<IFormFile> OtherFiles { get; init; }
}