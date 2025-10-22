using Microsoft.Extensions.Localization;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class ErrorCreatingUserBadRequesException: BaseBadRequestException
    {
        public ErrorCreatingUserBadRequesException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 400001;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}
