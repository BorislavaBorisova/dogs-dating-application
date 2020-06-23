using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Exceptions
{
    public class NoEntriesException : Exception
    {
        public NoEntriesException()
            :base("No entries to these requirements.")
        {

        }
    }
}
