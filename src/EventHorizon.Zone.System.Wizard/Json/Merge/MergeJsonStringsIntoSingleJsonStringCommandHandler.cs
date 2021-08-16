namespace EventHorizon.Zone.System.Wizard.Json.Merge
{
    using EventHorizon.Zone.Core.Model.Command;

    using global::System.Buffers;
    using global::System.Text;
    using global::System.Text.Json;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class MergeJsonStringsIntoSingleJsonStringCommandHandler
        : IRequestHandler<MergeJsonStringsIntoSingleJsonStringCommand, CommandResult<string>>
    {
        private const string INVALID_JSON_ERROR_CODE = "json_not_valid_type";

        public Task<CommandResult<string>> Handle(
            MergeJsonStringsIntoSingleJsonStringCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {

                var outputBuffer = new ArrayBufferWriter<byte>();

                using var sourceDocument = JsonDocument.Parse(
                    request.SourceJson
                );
                using var updateDocument = JsonDocument.Parse(
                    request.UpdatedJson
                );
                using (var jsonWriter = new Utf8JsonWriter(
                    outputBuffer
                ))
                {
                    var sourceRoot = sourceDocument.RootElement;
                    var updateRoot = updateDocument.RootElement;

                    if (sourceRoot.ValueKind != JsonValueKind.Object)
                    {
                        return new CommandResult<string>(
                            INVALID_JSON_ERROR_CODE
                        ).FromResult();
                    }

                    if (sourceRoot.ValueKind != updateRoot.ValueKind)
                    {
                        return new CommandResult<string>(
                            true,
                            request.SourceJson
                        ).FromResult();
                    }

                    MergeObjects(
                        jsonWriter,
                        sourceRoot,
                        updateRoot
                    );
                }

                return new CommandResult<string>(
                    true,
                    Encoding.UTF8.GetString(
                        outputBuffer.WrittenSpan
                    )
                ).FromResult();
            }
            catch (JsonException)
            {
                return new CommandResult<string>(
                    INVALID_JSON_ERROR_CODE
                ).FromResult();
            }
        }

        private static void MergeObjects(
            Utf8JsonWriter writer,
            JsonElement jsonRoot1,
            JsonElement jsonRoot2
        )
        {
            writer.WriteStartObject();

            // Write all the properties of the first document.
            // If a property exists in both documents, either:
            // * Merge them, if the value kinds match (e.g. both are objects),
            // * Completely override the value of the first with the one from the second, if the value kind mismatches (e.g. one is object, while the other is an array or string),
            // * Or favor the value of the first (regardless of what it may be), if the second one is null (i.e. don't override the first).
            foreach (var property in jsonRoot1.EnumerateObject())
            {
                var propertyName = property.Name;
                var newValueKind = default(JsonValueKind);

                bool TryGetPropertyValue(
                    string propertyName,
                    out JsonElement value,
                    out string writablePropertyName
                )
                {
                    writablePropertyName = propertyName.LowercaseFirstChar();

                    return jsonRoot2.TryGetProperty(
                        propertyName,
                        out value
                    ) || jsonRoot2.TryGetProperty(
                        propertyName.LowercaseFirstChar(),
                        out value
                    );
                }

                if (TryGetPropertyValue(
                    propertyName,
                    out var newValue,
                    out var writablePropertyName
                ) && (newValueKind = newValue.ValueKind) != JsonValueKind.Null)
                {
                    var originalValue = property.Value;
                    var originalValueKind = originalValue.ValueKind;

                    writer.WritePropertyName(
                        writablePropertyName
                    );

                    if (newValueKind == JsonValueKind.Object
                        && originalValueKind == JsonValueKind.Object
                    )
                    {
                        MergeObjects(
                            writer,
                            originalValue,
                            newValue
                        );
                        continue;
                    }

                    newValue.WriteTo(
                        writer
                    );
                    continue;
                }

                property.WriteTo(
                    writer
                );
            }

            // Write all the properties of the second document that are unique to it.
            foreach (var property in jsonRoot2.EnumerateObject())
            {
                // Check for the name as is and capitalized.
                if (DoesNotContainProperty(
                    jsonRoot1,
                    property.Name
                ))
                {
                    // If not found in either forms write property to writer.
                    property.WriteTo(
                        writer
                    );
                }
            }

            writer.WriteEndObject();
        }

        private static bool DoesNotContainProperty(
            JsonElement jsonElement,
            string propertyName
        ) => !jsonElement.TryGetProperty(
            propertyName,
            out _
        ) && !jsonElement.TryGetProperty(
            propertyName.UppercaseFirstChar(),
            out _
        );
    }
}
