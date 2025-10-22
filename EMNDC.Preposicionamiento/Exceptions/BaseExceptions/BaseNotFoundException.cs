
using System.Net;

namespace EMNDC.Preposicionamiento.Exceptions
{
    public class BaseNotFoundException : CustomBaseException
    {
        public BaseNotFoundException() : base()
        {
            HttpCode = (int)HttpStatusCode.NotFound;
        }
    }
}