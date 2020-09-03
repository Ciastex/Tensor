using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using Tensor.Matrix.Protocol;

namespace Tensor.Rest
{
    internal class Request
    {
        protected List<int> IgnoredStatusCodes { get; private set; } = new List<int>();

        protected Dictionary<int, Action<RestRequest, IRestResponse, Error>> StatusCodeActions { get; } =
            new Dictionary<int, Action<RestRequest, IRestResponse, Error>>();

        protected RestClient RestClient { get; private set; }
        protected string Endpoint { get; private set; }
        protected Method Method { get; private set; }
        protected object Body { get; private set; }

        protected Delegate SuccessDelegate { get; private set; }

        protected Request(string endpoint, Method method)
        {
            Endpoint = endpoint;
            Method = method;
        }

        public Request Using(RestClient restClient)
        {
            RestClient = restClient;
            return this;
        }

        public Request Ignore(params int[] codes)
        {
            IgnoredStatusCodes.AddRange(codes);
            IgnoredStatusCodes = new List<int>(IgnoredStatusCodes.Distinct());

            return this;
        }

        public Request On(int code, Action<RestRequest, IRestResponse, Error> action)
        {
            if (!StatusCodeActions.ContainsKey(code))
                StatusCodeActions.Add(code, action);

            return this;
        }

        public Request WhenSucceeded<T>(Action<T> success)
        {
            SuccessDelegate = success;
            return this;
        }

        public Request AddBody(object body)
        {
            Body = body;
            return this;
        }

        public async Task<T> Execute<T>(RestRequest customRestRequest = null)
        {
            var request = customRestRequest ?? new RestRequest(Method);
            request.Resource = Endpoint;
            request.Method = Method;

            if (Body != null)
            {
                request.AddParameter("application/json", JsonConvert.SerializeObject(Body), ParameterType.RequestBody);
            }

            var response = await RestClient.ExecuteAsync<T>(request);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var action = SuccessDelegate as Action<T>;
                action?.Invoke(response.Data);
            }
            else
            {
                if (StatusCodeActions.ContainsKey((int)response.StatusCode) && !IsIgnored(response.StatusCode))
                {
                    Error err = null;

                    try
                    {
                        err = JsonConvert.DeserializeObject<Error>(response.Content);
                    }
                    catch
                    {
                        /* ignore */
                    }

                    StatusCodeActions[(int)response.StatusCode](
                        request,
                        response,
                        err
                    );
                }
                else
                {
                    throw new HttpRequestException(
                        "Received non-OK status code and no handler was specified for this request.\n");
                }
            }

            return response.Data;
        }

        private bool IsIgnored(HttpStatusCode code)
        {
            return IgnoredStatusCodes.Contains((int)code);
        }
    }
}