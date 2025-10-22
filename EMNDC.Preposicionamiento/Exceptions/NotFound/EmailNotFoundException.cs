using Microsoft.Extensions.Localization;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class EmailNotFoundException: BaseNotFoundException
    {
        public EmailNotFoundException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 404001;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}
