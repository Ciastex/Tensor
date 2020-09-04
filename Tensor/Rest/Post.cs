using RestSharp;

namespace Tensor.Rest
{
    public sealed class Post : Request
    {
        private Post(string endpoint) : base(endpoint, Method.POST)
        {
        }

        public static Post To(string endpoint)
        {
            return new Post(endpoint);
        }
    }
}