using FinancialTracker.Services.AuthorizeApi.Application.Features;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Captcha
{
    public interface ICaptchaService
    {
        Task<OperationResult> VerifyAsync(string captchaToken);
    }
}
