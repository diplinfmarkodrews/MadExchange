using MadXchange.Common.Types;
using System;
using System.Net;

namespace MadXchange.Connector
{
    public interface IExceptionToResponseMapper
    {
        ExceptionResponse Map(Exception exception);
    }

    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
            => new ExceptionResponse(new { code = "error", message = exception.Message }, HttpStatusCode.BadRequest);
    }
}