﻿using Lyrox.Framework.Messaging.Abstraction;

namespace Lyrox.Framework.Messaging
{
    public abstract class ResponseBase : IResponse
    {
        public bool Successful { get; init; }
        public IEnumerable<string>? Errors { get; init; }

        protected ResponseBase(bool successful, IEnumerable<string>? errors)
        {
            Successful = successful;
            Errors = errors;
        }

        protected ResponseBase(bool successful)
            => Successful = successful;

        public ResponseBase() { }
    }
}
