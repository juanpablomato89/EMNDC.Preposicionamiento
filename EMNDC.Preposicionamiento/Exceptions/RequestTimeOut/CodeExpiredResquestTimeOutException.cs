using Microsoft.Extensions.Localization;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class CodeExpiredResquestTimeOutException: BaseRequestTimeOutException
    {
        public CodeExpiredResquestTimeOutException(IStringLocalizer<object> localizer) : base()
        {
            CustomCode = 408002;
            CustomMessage = localizer.GetString(CustomCode.ToString());
        }
    }
}
