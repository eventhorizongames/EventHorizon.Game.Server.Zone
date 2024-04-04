namespace EventHorizon.Zone.System.Wizard.Json.Mapper;

using global::System;
using EventHorizon.Zone.System.Wizard.Json.Model;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;

public class MapWizardDataToJsonDataDocumentHandler
    : IRequestHandler<MapWizardDataToJsonDataDocument, JsonDataDocument>
{
    private const char PROPERTY_SEPARATOR = ':';
    private const string PROPERTY_PREFIX = "property:";

    public Task<JsonDataDocument> Handle(
        MapWizardDataToJsonDataDocument request,
        CancellationToken cancellationToken
    )
    {
        var properties = request
            .WizardData.Where(dataItem => dataItem.Key.StartsWith(PROPERTY_PREFIX))
            .OrderBy(property => property.Key);
        var dataSplitIntoJson = new List<JsonDataProperty>();

        foreach (var property in properties)
        {
            var splitPropertyKey = property.Key.Split(PROPERTY_SEPARATOR).Skip(1);
            var stringPropertyValue = request.WizardData[
                property.Key.Replace(PROPERTY_PREFIX, string.Empty)
            ];
            var propertyType = property.Value;

            Console.WriteLine($"{property.Key} : {propertyType} : {stringPropertyValue}");

            //var propertyValue = default(object);
            //if (property.Value == "String")
            //{
            //    propertyValue = stringPropertyValue;
            //}
            //else if (property.Value == "Long")
            //{
            //    propertyValue = long.Parse(stringPropertyValue);
            //}
            //else if (property.Value == "Boolean")
            //{
            //    propertyValue = bool.Parse(stringPropertyValue);
            //}
            var jsonProperty = new JsonDataProperty(
                splitPropertyKey.Last(),
                propertyType,
                stringPropertyValue
            );

            var parentKey = splitPropertyKey.Take(splitPropertyKey.Count() - 1);
            // If we have any parent property keys, we need to loop down the key list and figure out the parent property
            if (!parentKey.Any())
            {
                dataSplitIntoJson.Add(jsonProperty);
                continue;
            }

            var parentKeyFirst = parentKey.First();
            var parentProperty = dataSplitIntoJson.FirstOrDefault(a => a.Name == parentKeyFirst);
            if (parentProperty.Name is null)
            {
                parentProperty = new JsonDataProperty(parentKey.First(), "Object", string.Empty);
                dataSplitIntoJson.Add(parentProperty);
            }
            for (var i = 1; i < parentKey.Count(); i++)
            {
                var newParentProperty = parentProperty.Data.FirstOrDefault(
                    a => a.Name == parentKey.ElementAt(i)
                );
                if (newParentProperty.Name is null)
                {
                    newParentProperty = new JsonDataProperty(
                        parentKey.ElementAt(i),
                        "Object",
                        string.Empty
                    );
                    parentProperty.Data.Add(newParentProperty);
                }
                parentProperty = newParentProperty;
            }
            if (propertyType == "InputKeyMap")
            {
                jsonProperty = AddInputKeyMapProperties(
                    splitPropertyKey,
                    jsonProperty,
                    request.WizardData
                );
            }
            parentProperty.Data.Add(jsonProperty);
        }

        return new JsonDataDocument(dataSplitIntoJson).FromResult();
    }

    private static JsonDataProperty AddInputKeyMapProperties(
        IEnumerable<string> splitPropertyKey,
        JsonDataProperty parentProperty,
        Dictionary<string, string> wizardData
    )
    {
        var inputKeyMapArray = wizardData.Where(
            dataItem =>
                dataItem.Key.StartsWith(
                    $"{string.Join(PROPERTY_SEPARATOR, splitPropertyKey)}{PROPERTY_SEPARATOR}"
                )
        );

        // Get max length of the InputKeyMap
        // playerInput:keyInputMap:{key}:key
        var indexKeys = inputKeyMapArray
            .Select(
                a =>
                    a.Key.Replace(
                        $"{string.Join(PROPERTY_SEPARATOR, splitPropertyKey)}{PROPERTY_SEPARATOR}",
                        string.Empty
                    )
                        .Split(':')[0]
            )
            .Distinct();

        foreach (var key in indexKeys)
        {
            var currentKeyMap = inputKeyMapArray
                .Where(a => a.Key.EndsWith($"{key}:key"))
                .Select(a => a.Key.Split(':')[^1])
                .FirstOrDefault();
            if (currentKeyMap is null)
            {
                continue;
            }

            var keyProperty = new JsonDataProperty(key, "Object", string.Empty);
            // Filter to only; :type, :pressed, :released, :camera
            var keyMapData = inputKeyMapArray
                .Where(
                    a =>
                        a.Key.StartsWith(
                            $"{string.Join(PROPERTY_SEPARATOR, splitPropertyKey)}{PROPERTY_SEPARATOR}{key}"
                        )
                )
                .Where(
                    a =>
                        a.Key.EndsWith(":key")
                        || a.Key.EndsWith(":type")
                        || a.Key.EndsWith(":pressed")
                        || a.Key.EndsWith(":released")
                        || a.Key.EndsWith(":camera")
                )
                .ToDictionary(a => a.Key, a => a.Value);

            foreach (var keyMapItem in keyMapData)
            {
                var type = "String";
                if (keyMapItem.Key.EndsWith(":pressed") || keyMapItem.Key.EndsWith(":released"))
                {
                    type = "Long";
                }

                keyProperty.Data.Add(
                    new JsonDataProperty(
                        keyMapItem.Key.Split(':')[^1],
                        type,
                        wizardData[keyMapItem.Key]
                    )
                );
            }

            parentProperty.Data.Add(keyProperty);
        }

        return parentProperty;
    }
}
