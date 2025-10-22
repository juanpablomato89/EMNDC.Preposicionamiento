using Microsoft.Extensions.Localization;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class InvalidTokenSessionsUnauthorizedException: BaseUnauthorizedException
    {
        public InvalidTokenSessionsUnauthorizedException(IStringLocalizer<object> localizer): base()
        {
            CustomCode = 401001;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}
