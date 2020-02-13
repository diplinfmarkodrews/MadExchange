using System;

namespace MadXchange.Exchange.Exceptions
{
    /// <summary>
    /// Exception to identify invalid api keys
    /// Todo: has to be caught and handled on appropriate place
    /// </summary>
    public class AuthorizationException : Exception
    {
        private readonly Guid _accountId;
        public Guid AccountId => _accountId;
        
        public AuthorizationException(string message, Guid accountId) : base(message)
        {
            _accountId = accountId;
        }

        //protected AuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //}
    }
}