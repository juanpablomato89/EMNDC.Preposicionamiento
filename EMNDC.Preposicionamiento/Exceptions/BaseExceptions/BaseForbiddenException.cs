
using System.Net;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class BaseForbiddenException : CustomBaseException
    {
        public BaseForbiddenException() : base()
        {
            HttpCode = (int)HttpStatusCode.Forbidden;
        }
    }
}