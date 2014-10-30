using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericSearch.Grammar
{
    [Serializable]
    public class InvalidSearchException : Exception
    {
        public InvalidSearchException(string message)
            : base(message)
        {
        }
    }
}
