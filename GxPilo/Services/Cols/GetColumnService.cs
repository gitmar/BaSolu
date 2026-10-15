using System.Net.Http;
using System.Net.Http.Headers;

using GxShared.Cols.Models;

namespace GxPilo.Services.Cols
{
    //    public async Task<List<LSelCol>> GetColumnsAsync(
    //    int idorg,
    //    ColPrefContext context,
    //    string bearerToken,
    //    CancellationToken cancellationToken = default)
    //    {
    //        using var request = new HttpRequestMessage(
    //            HttpMethod.Get,
    //            $"api/colprefs/{idorg}?totyp={context.Totyp}" +
    //            $"&efrm={context.Efrm}&styp={context.Styp}");

    //        request.Headers.Authorization =
    //            new AuthenticationHeaderValue("Bearer", bearerToken);

    //        using var response = await _httpClient.SendAsync(
    //            request, cancellationToken);

    //        response.EnsureSuccessStatusCode();

    //        return await response.Content
    //            .ReadFromJsonAsync<List<LSelCol>>(cancellationToken: cancellationToken)
    //            ?? [];
    //    }
    //}
}