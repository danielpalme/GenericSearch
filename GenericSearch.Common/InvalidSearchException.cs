using System;

namespace GenericSearch.Common
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
