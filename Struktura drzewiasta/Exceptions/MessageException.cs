using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Struktura_drzewiasta.Exceptions
{
    public class MessageException : Exception
    {
        public MessageException(string message) : base(message)
        {
        }
    }
}
