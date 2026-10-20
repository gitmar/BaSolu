using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

using GxShared.AdminDtos.AdModels;
using GxShared.Sess;

using Newtonsoft.Json;

namespace GxPilo.Services.Adspec
{
    public interface ISystemAdminClient
    {
        Task<AdminOperationResultDto> InitializeColumnsAsync(
            int idorg,
            bool allContexts,
            CancellationToken cancellationToken = default);

        Task<AdminOperationResultDto> SynchronizeDictionaryAsync(
            int idorg,
            string? language,
            CancellationToken cancellationToken = default);

        Task<List<AdminUserDto>> GetUsersAsync(
            int idorg,
            CancellationToken cancellationToken = default);

        Task<AdminOperationResultDto> ResetPasswordAsync(
            string email,
            string newPassword,
            CancellationToken cancellationToken = default);
    }

    public class SystemAdminClient : ISystemAdminClient
    {
        //private readonly HttpClient _apiClient;
        private readonly HttpClientService _clieManager;

        public SystemAdminClient(HttpClientService clieManager)
        {
            //_apiClient = cliefact.CreateClient("AuthClient");
            _clieManager = clieManager;
        }

        public async Task<AdminOperationResultDto> InitializeColumnsAsync(
            int idorg,
            bool allContexts,
            CancellationToken cancellationToken = default)
        {
            //var response = await _apiClient.PostAsJsonAsync(
            //    "systemadmin/columns/initialize",
            //    new InitializeColumnsRequest
            //    {
            //        Idorg = idorg,
            //        AllContexts = allContexts
            //    },
            //    cancellationToken);

            //return await ReadResultAsync(response, cancellationToken);
            var jsoncol = await _clieManager.SendRequestAsync(
                "AuthClient",
                HttpMethod.Post, "systemadmin/columns/initialize",
                new InitializeColumnsRequest
                {
                    Idorg = idorg,
                    AllContexts = allContexts
                });

            return JsonConvert.DeserializeObject<AdminOperationResultDto>(jsoncol) ?? new AdminOperationResultDto();

            //var responsecol = await _apiClient.PostAsJsonAsync(
            //    "systemadmin/dictionary/synchronize",
            //    new SynchronizeDictionaryRequest
            //    {
            //        Idorg = idorg,
            //        Language = language
            //    },
            //    cancellationToken);

            //return await ReadResultAsync(response, cancellationToken);
        }

        public async Task<AdminOperationResultDto> SynchronizeDictionaryAsync(
            int idorg,
            string? language,
            CancellationToken cancellationToken = default)
        {

            var jsonrpass = await _clieManager.SendRequestAsync(
                "AuthClient",
                HttpMethod.Post,
                "systemadmin/dictionary/synchronize",
                new SynchronizeDictionaryRequest
                {
                    Idorg = idorg,
                    Language = language
                });

            return JsonConvert.DeserializeObject<AdminOperationResultDto>(jsonrpass) ?? new AdminOperationResultDto();

            //var response = await _apiClient.PostAsJsonAsync(
            //    "systemadmin/dictionary/synchronize",
            //    new SynchronizeDictionaryRequest
            //    {
            //        Idorg = idorg,
            //        Language = language
            //    });

            //return await ReadResultAsync(response, cancellationToken);
        }

        public async Task<List<AdminUserDto>> GetUsersAsync(
            int idorg,
            CancellationToken cancellationToken = default)
        {
            var jsonusr = await _clieManager.SendRequestAsync(
                "AuthClient",
                HttpMethod.Get,
                $"systemadmin/users?idorg={idorg}");

            return JsonConvert.DeserializeObject<List<AdminUserDto>>(jsonusr) ?? new List<AdminUserDto>();
        }

        public async Task<AdminOperationResultDto> ResetPasswordAsync(
            string email,
            string newPassword,
            CancellationToken cancellationToken = default)
        {
            var jsonrpass = await _clieManager.SendRequestAsync(
                "AuthClient",
                HttpMethod.Post,
                "systemadmin/users/reset-password",
                 new ResetPasswordRequest
                 {
                     Email = email,
                     NewPassword = newPassword
                 });

            return JsonConvert.DeserializeObject<AdminOperationResultDto>(jsonrpass) ?? new AdminOperationResultDto();
        }

        private static async Task<AdminOperationResultDto> ReadResultAsync(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content
                    .ReadFromJsonAsync<AdminOperationResultDto>(
                        cancellationToken: cancellationToken);

                return error ?? new AdminOperationResultDto
                {
                    Success = false,
                    Message = $"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}"
                };
            }

            return await response.Content
                .ReadFromJsonAsync<AdminOperationResultDto>(
                    cancellationToken: cancellationToken)
                ?? new AdminOperationResultDto
                {
                    Success = false,
                    Message = "Invalid server response."
                };
        }
        private class ResetPasswordRequest
        {
            public string Email { get; set; }
            public string NewPassword { get; set; }
        }
    }
}
