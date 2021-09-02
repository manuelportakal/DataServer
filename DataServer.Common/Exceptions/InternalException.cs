using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.Common.Exceptions
{
    public class InternalException : Exception
    {

        public InternalException(string msg) : base(msg)
        { }
    }
}
