using Microsoft.Extensions.Localization;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class InvalidAuthenticationGoogleUnauthorizedException : BaseUnauthorizedException
    {
        public InvalidAuthenticationGoogleUnauthorizedException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 401002;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}