using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Tensor.Matrix;
using Tensor.Matrix.Client;
using Tensor.Matrix.Protocol;
using Tensor.Rest;

namespace Tensor
{
    public class MatrixClient
    {
        private JwtAuthenticator JwtAuthenticator { get; set; }
        private RestClient RestClient { get; }
        private Uri HomeServerUri { get; }

        public MxId MxId { get; private set; }
        public string DeviceId { get; private set; }

        public event EventHandler<Error> ProtocolError;

        public MatrixClient(string homeServerUri)
        {
            HomeServerUri = new Uri(homeServerUri);
            RestClient = new RestClient(HomeServerUri).UseJson();
        }

        public async Task<RegistrationResult> Register(string username, string password)
        {
            var result = await Post.To("/_matrix/client/r0/register")
                .Using(RestClient)
                .AddBody(new RegistrationRequest
                {
                    Username = username,
                    Password = password,
                    AuthenticationData = new AuthenticationData
                    {
                        Type = "m.login.dummy"
                    },
                    InhibitLogin = false,
                    DeviceId = null,
                    InitialDeviceDisplayName = null
                })
                .On(400, (req, res, err) => ProtocolError?.Invoke(this, err))
                .On(401, (req, res, err) => ProtocolError?.Invoke(this, err))
                .On(403, (req, res, err) => ProtocolError?.Invoke(this, err))
                .On(429, (req, res, err) => ProtocolError?.Invoke(this, err))
                .WhenSucceeded(new Action<RegistrationResult>((r) =>
                {
                    JwtAuthenticator = new JwtAuthenticator(r.AccessToken);
                    RestClient.Authenticator = JwtAuthenticator;

                    MxId = MxId.Parse(r.UserId);
                }))
                .Execute<RegistrationResult>();

            return result;
        }

        public async Task<ProtocolVersions> GetProtocolVersions()
            => await Get.To("/_matrix/client/versions")
                .Using(RestClient)
                .Execute<ProtocolVersions>();
    }
}