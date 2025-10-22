using Microsoft.Extensions.Localization;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class PasswordInvalidBadRequesException : BaseBadRequestException
    {
        public PasswordInvalidBadRequesException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 400002;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}
