using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public class ErrorUtils
    {
        public class SqlErrorException : Exception
        {
            public SqlErrorException(string message) : base(message)
            {
            }

            public SqlErrorException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }
    }
}
