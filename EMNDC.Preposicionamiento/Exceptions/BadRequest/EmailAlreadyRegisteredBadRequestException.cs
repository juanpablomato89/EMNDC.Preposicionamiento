using Microsoft.Extensions.Localization;

namespace EMNDC.Preposicionamiento.Exceptions.BadRequest
{
    public class EmailAlreadyRegisteredBadRequestException : BaseBadRequestException
    {
        public EmailAlreadyRegisteredBadRequestException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 400003;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}
