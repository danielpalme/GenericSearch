using System;
using System.Collections.Generic;
using System.Linq;
using GenericSearch.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace GenericSearch.UI
{
    public class AbstractSearchModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(AbstractSearch))
            {
                var assembly = typeof(AbstractSearch).Assembly;
                var abstractSearchClasses = assembly.GetExportedTypes()
                    .Where(t => t.BaseType.Equals(typeof(AbstractSearch)))
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

                    modelBuilderByType.Add(type, new ComplexTypeModelBinder(propertyBinders));
                }

                return new AbstractSearchModelBinder(modelBuilderByType, context.MetadataProvider);
            }

            return null;
        }
    }
}
