using System;
using Tensor.Matrix.Protocol;

namespace Tensor.Matrix
{
    public class RegistrationEventArgs : EventArgs
    {
        public MxId MxId { get; }

        public RegistrationEventArgs(MxId mxid)
        {
            MxId = mxid;
        }
    }
}