using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using System.Text.Json;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Captcha
{
    public class ReCaptchaService : ICaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _site;
        public ReCaptchaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _site = "https://www.google.com/recaptcha/api/siteverify";
        }
        public async Task<OperationResult> VerifyAsync(string captchaToken)
        {
            var secretKeyReCaptcha = Environment.GetEnvironmentVariable("RECAPTCHA_KEY");
            if (string.IsNullOrEmpty(secretKeyReCaptcha))
                return OperationResultCreator.Failure(
                    Enum_StatusCode.INTERNAL_SERVER_ERROR, "reCaptcha key don't must be empty");

            var reCaptchaResponse = await HttpPostAsync<ReCaptchaResponse>(captchaToken, secretKeyReCaptcha);
            if (reCaptchaResponse is null)
                return OperationResultCreator.Failure(
                    Enum_StatusCode.BAD_REQUEST, "reCaptcha response is null");

            /*
            if (reCaptchaResult.Score < 0.5) // Блокируем, если низкий "рейтинг" бота
                return OperationResultCreator.Failure(
                    Enum_StatusCode.BAD_REQUEST, "Low rank of bots by reCaptcha");
            */

            if (!reCaptchaResponse.Success)
            {
                string errors = string.Join("; ", reCaptchaResponse.ErrorCodes);
                return OperationResultCreator.Failure(
                    Enum_StatusCode.BAD_REQUEST, $"Failure reCaptcha verification: {errors}");
            }
            return OperationResultCreator.Success(Enum_StatusCode.OK, "reCaptcha verification passed");
        }

        private async Task<T> HttpPostAsync<T>(string captchaToken, string secretKeyReCaptcha)
        {
            var parameters = new Dictionary<string, string>
            {
                { "secret", secretKeyReCaptcha },
                { "response", captchaToken }
            };
            var encodedContent = new FormUrlEncodedContent(parameters);
            var response = await _httpClient.PostAsync(_site, encodedContent);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var reCaptchaResult = JsonSerializer.Deserialize<T>(jsonResponse);
            return reCaptchaResult!;
        }
    }
}
