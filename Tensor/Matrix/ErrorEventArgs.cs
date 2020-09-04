using System;
using RestSharp;
using Tensor.Matrix.Protocol;

namespace Tensor.Matrix
{
    public class ErrorEventArgs : EventArgs
    {
        public Error Error { get; }
        public IRestResponse Response { get; }

        public ErrorEventArgs(Error error, IRestResponse response)
        {
            Error = error;
            Response = response;
        }
    }
}