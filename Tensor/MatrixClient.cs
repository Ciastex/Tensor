using System;
using System.Threading.Tasks;
using RestSharp;
using Tensor.Matrix;

namespace Tensor
{
    public class MatrixClient
    {
        private RestClient RestClient { get; }
        public Uri HomeServerUri { get; }

        public MatrixClient(string homeServerUri)
        {
            HomeServerUri = new Uri(homeServerUri);
            RestClient = new RestClient(HomeServerUri);
        }
    }
}