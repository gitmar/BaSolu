// Client/Services/ColumnPreferencesClient.cs
using System.Net.Http.Json;

using GxShared.Cols.Models;

namespace GxPilo.Services.Cols
{
    public class ColumnPreferencesClient
    {
        private readonly HttpClient _dftClient;


        public ColumnPreferencesClient(IHttpClientFactory httpClientFactory)
        {
            _dftClient = httpClientFactory.CreateClient("AuthClient");
        }

        public async Task<List<LSelCol>> LoadAsync(
            int idorg,
            ColPrefContext context,
            CancellationToken cancellationToken = default)
        {
            var url = BuildContextUrl(idorg, context);

            return await _dftClient.GetFromJsonAsync<List<LSelCol>>(
                       url,
                       cancellationToken)
                   ?? [];
        }

        public async Task<bool> SaveAsync(
            int idorg,
            ColPrefContext context,
            List<LSelCol> selection,
            CancellationToken cancellationToken = default)
        {
            var url = BuildContextUrl(idorg, context);

            using var response = await _dftClient.PutAsJsonAsync(
                url,
                selection,
                cancellationToken);

            return response.IsSuccessStatusCode;
        }

        public async Task ResetAsync(
            int idorg,
            ColPrefContext context,
            CancellationToken cancellationToken = default)
        {
            var url = $"{BuildPath(idorg)}/reset{BuildQueryString(context)}";

            using var response = await _dftClient.PostAsync(
                url,
                content: null,
                cancellationToken);

            response.EnsureSuccessStatusCode();
        }

        public async Task<ColumnSyncPreviewDto> PreviewSynchronizationAsync(
            int idorg,
            ColPrefContext context,
            CancellationToken cancellationToken = default)
        {
            var url =
                $"{BuildPath(idorg)}/synchronize/preview" +
                BuildQueryString(context);

            using var response = await _dftClient.PostAsync(
                url,
                content: null,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ColumnSyncPreviewDto>(
                       cancellationToken: cancellationToken)
                   ?? new ColumnSyncPreviewDto();
        }

        public async Task<ColumnSyncResultDto> SynchronizeAsync(
            int idorg,
            ColPrefContext context,
            CancellationToken cancellationToken = default)
        {
            var url =
                $"{BuildPath(idorg)}/synchronize" +
                BuildQueryString(context);

            using var response = await _dftClient.PostAsync(
                url,
                content: null,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ColumnSyncResultDto>(
                       cancellationToken: cancellationToken)
                   ?? new ColumnSyncResultDto();
        }

        public async Task<ColumnSyncAllResultDto> SynchronizeAllAsync(
            int idorg,
            CancellationToken cancellationToken = default)
        {
            var url = $"{BuildPath(idorg)}/synchronize-all";

            using var response = await _dftClient.PostAsync(
                url,
                content: null,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ColumnSyncAllResultDto>(
                       cancellationToken: cancellationToken)
                   ?? new ColumnSyncAllResultDto();
        }

        private static string BuildPath(int idorg)
            => $"Colprefs/{idorg}";

        private static string BuildQueryString(ColPrefContext context)
            => $"?Totyp={context.Totyp}" +
               $"&Efrm={context.Efrm}" +
               $"&Styp={context.Styp}";

        private static string BuildContextUrl(int idorg, ColPrefContext context)
            => $"{BuildPath(idorg)}{BuildQueryString(context)}";
    }
}
//    public class ColumnPreferencesClient
//    {
//        private readonly HttpClient _http;

//        public ColumnPreferencesClient(IHttpClientFactory httpClientFactory)
//        {
//            _http = httpClientFactory.CreateClient("AuthClient");
//        }

//        public async Task<List<LSelCol>> LoadAsync(
//            int idorg,
//            ColPrefContext context,
//            CancellationToken cancellationToken = default)
//        {
//            var url = BuildUrl(idorg, context);

//            return await _http.GetFromJsonAsync<List<LSelCol>>(
//                       url,
//                       cancellationToken)
//                   ?? [];
//        }

//        public async Task<bool> SaveAsync(
//            int idorg,
//            ColPrefContext context,
//            List<LSelCol> selection,
//            CancellationToken cancellationToken = default)
//        {
//            var url = BuildUrl(idorg, context);

//            using var response = await _http.PutAsJsonAsync(
//                url,
//                selection,
//                cancellationToken);

//            return response.IsSuccessStatusCode;
//        }
//        public async Task ResetAsync(
//    int idorg,
//    ColPrefContext context,
//    CancellationToken cancellationToken = default)
//        {
//            var url = $"{BuildPath(idorg)}/reset{BuildQueryString(context)}";

//            using var response = await _http.PostAsync(
//                url,
//                content: null,
//                cancellationToken);

//            response.EnsureSuccessStatusCode();
//        }
//        private static string BuildPath(int idorg)
//            => $"Colprefs/{idorg}";
//        private static string BuildQueryString(ColPrefContext context)
//            => $"?Totyp={context.Totyp}" +
//            $"&Efrm={context.Efrm}" +
//            $"&Styp={context.Styp}";
//        public async Task<ColumnSyncPreviewDto> PreviewSynchronizationAsync(
//            int idorg,
//            ColPrefContext context,
//            CancellationToken cancellationToken = default)
//        {
//            var url = $"{BuildUrl(idorg, context)}/synchronize/preview";

//            using var response = await _http.PostAsync(
//                url,
//                content: null,
//                cancellationToken);

//            response.EnsureSuccessStatusCode();

//            return await response.Content.ReadFromJsonAsync<ColumnSyncPreviewDto>(
//                       cancellationToken: cancellationToken)
//                   ?? new ColumnSyncPreviewDto();
//        }

//        public async Task<ColumnSyncResultDto> SynchronizeAsync(
//            int idorg,
//            ColPrefContext context,
//            CancellationToken cancellationToken = default)
//        {
//            var url = $"{BuildUrl(idorg, context)}/synchronize";

//            using var response = await _http.PostAsync(
//                url,
//                content: null,
//                cancellationToken);

//            response.EnsureSuccessStatusCode();

//            return await response.Content.ReadFromJsonAsync<ColumnSyncResultDto>(
//                       cancellationToken: cancellationToken)
//                   ?? new ColumnSyncResultDto();
//        }
//        public async Task<ColumnSyncAllResultDto> SynchronizeAllAsync(
//    int idorg,
//    CancellationToken cancellationToken = default)
//        {
//            using var response = await _http.PostAsync(
//                $"Colprefs/{idorg}/synchronize-all",
//                content: null,
//                cancellationToken);

//            response.EnsureSuccessStatusCode();

//            return await response.Content.ReadFromJsonAsync<ColumnSyncAllResultDto>(
//                       cancellationToken: cancellationToken)
//                   ?? new ColumnSyncAllResultDto();
//        }

//        private static string BuildUrl(int idorg, ColPrefContext context)
//            => $"Colprefs/{idorg}?Totyp={context.Totyp}" +
//               $"&Efrm={context.Efrm}" +
//               $"&Styp={context.Styp}";
//    }
//}