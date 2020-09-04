using System;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Tensor.Matrix;
using Tensor.Matrix.Client;
using Tensor.Matrix.Client.Authentication;
using Tensor.Matrix.Protocol;
using Tensor.Rest;

namespace Tensor
{
    public class MatrixClient
    {
        private JwtAuthenticator JwtAuthenticator { get; set; }
        private RestClient RestClient { get; }

        public MxId MxId { get; private set; }
        public string DeviceId { get; private set; }

        public MatrixClient(string homeServerUri)
        {
            RestClient = new RestClient(new Uri(homeServerUri)).UseJson();
        }

        public event EventHandler<ErrorEventArgs> ProtocolError;
        public event EventHandler<LoginEventArgs> LoggedIn;
        public event EventHandler<RegistrationEventArgs> AccountRegistered;

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
                    InhibitLogin = true,
                    DeviceId = null,
                    InitialDeviceDisplayName = null
                })
                .On(400, (req, res, err) => ProtocolError?.Invoke(this, new ErrorEventArgs(err, res)))
                .On(401, (req, res, err) => ProtocolError?.Invoke(this, new ErrorEventArgs(err, res)))
                .On(403, (req, res, err) => ProtocolError?.Invoke(this, new ErrorEventArgs(err, res)))
                .On(429, (req, res, err) => ProtocolError?.Invoke(this, new ErrorEventArgs(err, res)))
                .WhenSucceeded(new Action<RegistrationResult>(r =>
                {
                    AccountRegistered?.Invoke(this, new RegistrationEventArgs(MxId.Parse(r.UserId)));
                }))
                .Execute<RegistrationResult>();

            return result;
        }

        public async Task<LoginResult> Login(string username, string password, string deviceId = null,
            string initialDisplayName = "TensorMatrix")
        {
            var methodQueryResult = await QueryLoginMethods();

            if (!methodQueryResult.SupportedMethods.Any(x => x.Type == "m.login.password"))
                throw new UnsupportedLoginMethodException("Homeserver does not support password-based login.");

            var result = await Post.To("/_matrix/client/r0/login")
                .Using(RestClient)
                .AddBody(new PasswordLoginRequest<MxidIdentifier>
                {
                    Identifier = new MxidIdentifier {User = username},
                    DeviceId = deviceId,
                    InitialDeviceDisplayName = initialDisplayName,
                    Password = password
                })
                .On(400, (req, res, err) => ProtocolError?.Invoke(this, new ErrorEventArgs(err, res)))
                .On(403, (req, res, err) => ProtocolError?.Invoke(this, new ErrorEventArgs(err, res)))
                .On(429, (req, res, err) => ProtocolError?.Invoke(this, new ErrorEventArgs(err, res)))
                .WhenSucceeded(new Action<LoginResult>(r =>
                {
                    MxId = MxId.Parse(r.UserId);
                    JwtAuthenticator = new JwtAuthenticator(r.AccessToken);
                    RestClient.Authenticator = JwtAuthenticator;
                    DeviceId = r.DeviceId;

                    if (r.WellKnown != null) RestClient.BaseUrl = new Uri(r.WellKnown.HomeServer.BaseUrl);

                    LoggedIn?.Invoke(this, new LoginEventArgs(MxId, r.AccessToken, r.DeviceId));
                }))
                .Execute<LoginResult>();

            return result;
        }

        public async Task<LoginMethodQueryResult> QueryLoginMethods()
        {
            var result = await Get.To("/_matrix/client/r0/login")
                .Using(RestClient)
                .Execute<LoginMethodQueryResult>();

            return result;
        }

        public async Task<ProtocolVersions> GetProtocolVersions()
        {
            return await Get.To("/_matrix/client/versions")
                .Using(RestClient)
                .Execute<ProtocolVersions>();
        }
    }
}