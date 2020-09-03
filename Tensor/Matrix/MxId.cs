using System;
using System.Text.RegularExpressions;

namespace Tensor.Matrix
{
    public class MxId
    {
        private static Regex _uidValidationRegex;

        public string Username { get; }
        public string Server { get; }

        static MxId()
        {
            _uidValidationRegex = new Regex(@"\@(?<username>[a-z0-9\._=-/]+)\:(?<server>.+)");
        }

        public MxId(string username, string server)
        {
            Username = username;
            Server = server;
        }

        public static MxId Parse(string mxidString)
        {
            var match = _uidValidationRegex.Match(mxidString);

            if (!match.Success)
                throw new FormatException("The provided string is not a valid MXID.");

            var username = match.Groups["username"].Value;
            var server = match.Groups["server"].Value;

            if (!Uri.IsWellFormedUriString($"https://{server}", UriKind.Absolute))
                throw new FormatException("The homeserver part of the MXID is invalid.");

            return new MxId(username, server);
        }
    }
}