using Microsoft.Extensions.Localization;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class InvalidCodeNotFoundException: BaseNotFoundException
    {
        public InvalidCodeNotFoundException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 404002;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}
