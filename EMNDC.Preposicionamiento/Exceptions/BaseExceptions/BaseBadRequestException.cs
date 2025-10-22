
using System.Net;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class BaseBadRequestException : CustomBaseException
    {
        public BaseBadRequestException() : base()
        {
            HttpCode = (int)HttpStatusCode.BadRequest;
        }
    }
}