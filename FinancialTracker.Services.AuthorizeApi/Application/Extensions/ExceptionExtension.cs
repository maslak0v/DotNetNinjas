namespace FinancialTracker.Services.AuthorizeApi.Application.Extensions
{
    public static class ExceptionExtension
    {
        public static string MessageWithInner(this Exception ex) 
            => ex.InnerException switch
            {
                null => ex.Message,
                not null => $"{ex.Message}\nInner:{ex.InnerException.Message}",
            };
    }
}
