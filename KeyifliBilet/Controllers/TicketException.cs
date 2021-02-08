using System;
using System.Runtime.Serialization;

namespace KeyifliBilet.Controllers
{
    [Serializable]
    internal class TicketException : Exception
    {
        public TicketException()
        {
            //Hata Sms ve Mail
        }

        public TicketException(string message) : base(message)
        {

        }

        public TicketException(string message, Exception innerException) : base(message, innerException)
        {

        }

        protected TicketException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }


    }
}