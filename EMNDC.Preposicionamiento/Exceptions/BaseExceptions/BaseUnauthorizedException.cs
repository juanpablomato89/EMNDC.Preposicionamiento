
using System.Net;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class BaseUnauthorizedException : CustomBaseException
    {
        public BaseUnauthorizedException() : base()
        {
            HttpCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}