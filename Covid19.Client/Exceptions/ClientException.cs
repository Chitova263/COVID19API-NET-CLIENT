using FluentResults;
using System;
using System.Collections.Generic;

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

        public CovidClientException(IEnumerable<Error> errors) => Errors = errors;
    }
}
