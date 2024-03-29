
using FluentValidation;

namespace AlbaconTest.Api.Filters
{
    /// <summary>
    /// Jelikož samotné Minimal API ani Fluent validace neprovádí kontrolu na prázdný model, je nutné ji provést ručně
    /// </summary>
    public class EmptyModelValidatorFilter<T> : IEndpointFilter
    {
        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if (validator is not null)
            {
                FluentValidation.Results.ValidationResult results = new FluentValidation.Results.ValidationResult();
                var entity = context.Arguments
                    .OfType<T>()
                    .FirstOrDefault(a => a?.GetType() == typeof(T));
                if (entity is not null)
                {
                    results = await validator.ValidateAsync((entity));
                    if (!results.IsValid)
                    {
                        return Results.ValidationProblem(results.ToDictionary());
                    }
                }
                else
                {
                    results.Errors.Add(new FluentValidation.Results.ValidationFailure("", "Entity not found"));
                    return Results.ValidationProblem(results.ToDictionary());
                }
            }

            return await next(context);
        }
    }
}
