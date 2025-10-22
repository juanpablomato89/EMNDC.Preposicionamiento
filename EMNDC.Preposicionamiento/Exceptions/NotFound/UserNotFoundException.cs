using Microsoft.Extensions.Localization;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class UserNotFoundException: BaseNotFoundException
    {
        public UserNotFoundException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 404003;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}
