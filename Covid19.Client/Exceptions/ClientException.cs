using FluentResults;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Covid19
{
    public class CovidClientException : Exception
    {
        public IEnumerable<Error> Errors { get; set; } = new List<Error>();

        public CovidClientException()
        {
        }

        public CovidClientException(string? message) : base(message)
        {
        }

        public CovidClientException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CovidClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CovidClientException(IEnumerable<Error> errors)
        {
            Errors = errors;
        }
    }
}
