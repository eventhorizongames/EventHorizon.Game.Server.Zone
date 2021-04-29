namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Create
{
    using EventHorizon.Zone.Core.Model.Command;
    using global::System.Security.Cryptography;
    using global::System.Text;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class CreateHashFromContentCommandHandler
        : IRequestHandler<CreateHashFromContentCommand, CommandResult<string>>
    {
        public Task<CommandResult<string>> Handle(
            CreateHashFromContentCommand request,
            CancellationToken cancellationToken
        )
        {
            return new CommandResult<string>(
                true,
                CreateHashFromContent(
                    request.Content
                )
            ).FromResult();
        }

        private static string CreateHashFromContent(
            string consolidatedClasses
        )
        {
            using var sha256Hash = SHA256.Create();
            var scriptHashBytes = sha256Hash.ComputeHash(
                consolidatedClasses.ToBytes()
            );

            // Convert byte array to a string   
            var hashBuilder = new StringBuilder();
            for (int i = 0; i < scriptHashBytes.Length; i++)
            {
                hashBuilder.Append(
                    scriptHashBytes[i].ToString("x2")
                );
            }
            return hashBuilder.ToString();
        }
    }
}
