using System.Net;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class BaseRequestTimeOutException: CustomBaseException
    {
        public BaseRequestTimeOutException() : base()
        {
            HttpCode = (int)HttpStatusCode.RequestTimeout;
        }
    }
}
