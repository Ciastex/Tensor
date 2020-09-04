using System;

namespace Tensor.Matrix.Protocol
{
    public class UnsupportedLoginMethodException : Exception
    {
        public UnsupportedLoginMethodException(string message) : base(message)
        {
        }
    }
}