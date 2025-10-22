using Microsoft.Extensions.Localization;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class ChangePasswordForbidenException: BaseForbiddenException
    {
        public ChangePasswordForbidenException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 403001;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}
