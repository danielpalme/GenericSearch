using GenericSearch.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace GenericSearch.UI.Infrastructure.MVC;

public class AbstractSearchModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.ModelType == typeof(AbstractSearch))
        {
            var assembly = typeof(AbstractSearch).Assembly;
            var abstractSearchClasses = assembly.GetExportedTypes()
                .Where(t => typeof(AbstractSearch).Equals(t.BaseType))
                .Where(t => !t.IsAbstract)
                .ToList();

            var modelBuilderByType = new Dictionary<Type, ComplexTypeModelBinder>();

            foreach (var type in abstractSearchClasses)
            {
                var propertyBinders = new Dictionary<ModelMetadata, IModelBinder>();
                var metadata = context.MetadataProvider.GetMetadataForType(type);

                foreach (var property in metadata.Properties)
                {
                    propertyBinders.Add(property, context.CreateBinder(property));
                }

                var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
                modelBuilderByType.Add(type, new ComplexTypeModelBinder(propertyBinders, loggerFactory));
            }

            return new AbstractSearchModelBinder(modelBuilderByType, context.MetadataProvider);
        }

        return null;
    }
}
