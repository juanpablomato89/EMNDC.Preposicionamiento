using Microsoft.Extensions.Localization;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class ErrorSendNextCodeRequestTimeOutException: BaseRequestTimeOutException
    {
        public ErrorSendNextCodeRequestTimeOutException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 408001;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}
