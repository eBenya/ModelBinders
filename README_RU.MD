# ModelBinder library

Появилась нужда в возможности сопоставления с моделью атрибутов json строки и файлов, отправленных из формы.
В поисках вариантов наткнулся на [пост](https://github.com/thomaslevesque/blog/blob/e447b45f52e2552d4bd612cec78e6be747db08ce/content/posts/2018-09-04-handling-multipart-requests-with-json-and-file-uploads-in-asp-net-core/index.md).
В силу того, что не нашел у автора репозитория и пакета, решил сделать сам, опираясь на его решение.

## Как использовать?
Добавляем аттрибут для модели запроса `ModelBinder` с указанием `BinderType` и задаем имя, по которому будет идти сопоставление с данными из `form-data`, через которое передается JSON модель.
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
  <summary>Для модели выше, запрос будет выглядеть примерно так:</summary>

    ```curl
    curl --location 'http://localhost/api/do-some-with-data' \
    --form 'RequestModel="{\"name\":\"kek\", \"age\":\"puc\"}";type=application/json' \
    --form 'File=@"../SomeFile1.jpg"' \
    --form 'OtherFiles=@"../SomeFile1.jpg"' \
    --form 'OtherFiles=@"../SomeFile2.jpg"'
    ```
![PostmanExample.png](Resources%2FImages%2FPostmanExample.png)
</details>