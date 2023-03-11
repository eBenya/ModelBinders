﻿using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;

namespace ModelBinders.MVC;

public class JsonWithFilesFormModelBinder : IModelBinder
{
	private readonly FormFileModelBinder _formFileModelBinder;

	public JsonWithFilesFormModelBinder( ILoggerFactory loggerFactory)
	{
		_formFileModelBinder = new FormFileModelBinder(loggerFactory);
	}

	public async Task BindModelAsync(ModelBindingContext bindingContext)
	{
		if (bindingContext == null)
			throw new ArgumentNullException(nameof(bindingContext));

		// Retrieve the form part containing the JSON
		var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
		if (valueResult == ValueProviderResult.None)
		{
			var message = bindingContext.ModelMetadata.ModelBindingMessageProvider.MissingBindRequiredValueAccessor(bindingContext.FieldName);
			bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, message);
			return;
		}
		var rawValue = valueResult.FirstValue;

		// Deserialize the JSON
		var model = JsonSerializer.Deserialize(rawValue, bindingContext.ModelType);

		// Bind each of the IFormFile properties from the other form parts
		foreach (var property in bindingContext.ModelMetadata.Properties)
		{
			if (property.ModelType != typeof(ICollection<IFormFile>) && property.ModelType != typeof(IFormFile))
				continue;

			var fieldName = property.BinderModelName ?? property.PropertyName;
			var modelName = fieldName;
			var propertyModel = property.PropertyGetter(bindingContext.Model);
			ModelBindingResult propertyResult;
			using (bindingContext.EnterNestedScope(property, fieldName, modelName, propertyModel))
			{
				await _formFileModelBinder.BindModelAsync(bindingContext).ConfigureAwait(false);
				propertyResult = bindingContext.Result;
			}

			if (propertyResult.IsModelSet)
			{
				// The IFormFile was successfully bound, assign it to the corresponding property of the model
				property.PropertySetter(model, propertyResult.Model);
			}
			else if (property.IsBindingRequired)
			{
				var message = property.ModelBindingMessageProvider.MissingBindRequiredValueAccessor(fieldName);
				bindingContext.ModelState.TryAddModelError(modelName, message);
			}
		}

		// Set the successfully constructed model as the result of the model binding
		bindingContext.Result = ModelBindingResult.Success(model);
	}
}
