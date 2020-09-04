using RestSharp;

namespace Tensor.Rest
{
    public sealed class Get : Request
    {
        private Get(string endpoint) : base(endpoint, Method.GET)
        {
        }

        public static Get To(string endpoint)
        {
            return new Get(endpoint);
        }
    }
}