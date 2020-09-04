using System;
using Tensor.Matrix.Protocol;

namespace Tensor.Matrix
{
    public class LoginEventArgs : EventArgs
    {
        public MxId MxId { get; }
        public string AccessToken { get; }
        public string DeviceId { get; }

        public LoginEventArgs(MxId mxid, string accessToken, string deviceId)
        {
            MxId = mxid;
            AccessToken = accessToken;
            DeviceId = deviceId;
        }
    }
}