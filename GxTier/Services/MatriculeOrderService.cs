
using GxShared.Others;
using GxShared.Sess;

using Newtonsoft.Json;

using System.Net.Http.Json;

namespace GxTie.Services
{
    public interface IClientMatriculeOrdreService
    {
        Task<string> GenerateOrdreAsync(
            string sorga,
            int ugpe,
            string sgpe,
            string saffilia,
            MatriculeGenerationMode generationMode,
            CancellationToken cancellationToken = default);
    }
    public class ClientMatriculeOrdreService : IClientMatriculeOrdreService
    {
        private readonly HttpClient _authClient;

        public ClientMatriculeOrdreService(IHttpClientFactory authClient)
        {
            _authClient = authClient.CreateClient("AuthClient");
        }

        public async Task<string> GenerateOrdreAsync(
            string sorga,
            int ugpe,
            string sgpe,
            string saffilia,
            MatriculeGenerationMode generationMode,
            CancellationToken cancellationToken = default)
        {
            return generationMode switch
            {
                MatriculeGenerationMode.Local =>
                    GenerateLocalOrdre(),

                MatriculeGenerationMode.Api =>
                    await GenerateApiOrdreAsync(
                        sorga, ugpe, sgpe, saffilia, cancellationToken),

                _ => throw new ArgumentOutOfRangeException(
                        nameof(generationMode))
            };
        }

        private static string GenerateLocalOrdre()
        {
            // Suitable for offline/draft registrations.
            // Replace later during synchronization if required.
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var suffix = Random.Shared.Next(100, 999);

            var raw = $"{timestamp}{suffix}";

            return raw.Length >= 10
                ? raw[^10..]
                : raw.PadLeft(10, '0');
        }

        private async Task<string> GenerateApiOrdreAsync(
            string sorga,
            int ugpe,
            string sgpe,
            string saffilia,
            CancellationToken cancellationToken)
        {
            var ordreq = new OrdreRequest
            {
                Sorga = sorga,
                Ugpe = ugpe,
                Sgpe = sgpe,
                Saffilia = saffilia,
                IsProvisional = false
            };
            var response = await _authClient.PostAsJsonAsync(
                "lgauth/matricule/generate-ordre", ordreq, cancellationToken);
          
            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<OrdreResponse>(
                    cancellationToken: cancellationToken);

            if (result is null || string.IsNullOrWhiteSpace(result.Ordre))
            {
                throw new InvalidOperationException(
                    "The API did not return a valid Ordre.");
            }

            return result.Ordre;
        }
    }
}