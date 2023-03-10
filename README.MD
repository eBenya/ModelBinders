# ModelBinder library

There was a need to be able to match the json attribute model of the string and files submitted from the form.
Looking for options, I came across this [post](https://github.com/thomaslevesque/blog/blob/e447b45f52e2552d4bd612cec78e6be747db08ce/content/posts/2018-09-04-handling-multipart-requests-with-json-and-file-uploads-in-asp-net-core/index.md).
Since I did not find the repository and package from the author, I decided to do it myself, based on his decision.
## How to use?
Added attribute for the requset model `ModelBinder` 
with parameter type `BinderType` 
and set the name that will be used to compare the data from the `form-data` 
through which the `JSON` model is passed.

```csharp
[ModelBinder(typeof(CustomModelBinder), Name = nameof(RequestModel))]
public class RequestModel
{
     public string Name { get; set; }
     public int Age { get; set; }
     
     public IFormFile File { get; set; }
     public ICollection<IFormFile> OtherFiles { get; set; }
}
```
<details>
  <summary>For the models above, the query would look something like this:</summary>

    ```curl
    curl --location 'http://localhost/api/do-some-with-data' \
    --form 'RequestModel="{\"name\":\"kek\", \"age\":\"puc\"}";type=application/json' \
    --form 'File=@"../SomeFile1.jpg"' \
    --form 'OtherFiles=@"../SomeFile1.jpg"' \
    --form 'OtherFiles=@"../SomeFile2.jpg"'
    ```
![PostmanExample.png](Resources%2FImages%2FPostmanExample.png)
</details>