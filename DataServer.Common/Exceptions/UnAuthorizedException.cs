using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.Common.Exceptions
{
    public class UnAuthorizedException : Exception
    {

        public UnAuthorizedException(string msg) : base(msg)
        { }
    }
}
