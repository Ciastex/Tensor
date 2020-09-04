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
    public class Request
    {
        protected List<int> IgnoredStatusCodes { get; private set; } = new List<int>();

        protected Dictionary<int, Action<RestRequest, IRestResponse, Error>> StatusCodeActions { get; } =
            new Dictionary<int, Action<RestRequest, IRestResponse, Error>>();

        protected RestClient RestClient { get; private set; }
        protected string Endpoint { get; }
        protected Method Method { get; }
        protected object Body { get; private set; }
        protected bool IsNoisy { get; private set; }

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

        public Request Noisy()
        {
            IsNoisy = true;
            return this;
        }

        public async Task<T> Execute<T>(RestRequest customRestRequest = null)
        {
            var request = customRestRequest ?? new RestRequest(Method);
            request.Resource = Endpoint;
            request.Method = Method;

            if (Body != null)
            {
                var json = JsonConvert.SerializeObject(Body);

                if (IsNoisy) Console.WriteLine($"|> OUT ==> {json}");

                request.AddParameter("application/json", json, ParameterType.RequestBody);
            }


            var response = await RestClient.ExecuteAsync(request);

            if (IsNoisy) Console.WriteLine($"<== IN <| {response.Content}");

            var data = JsonConvert.DeserializeObject<T>(response.Content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var action = SuccessDelegate as Action<T>;
                action?.Invoke(data);
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

            return data;
        }

        private bool IsIgnored(HttpStatusCode code)
        {
            return IgnoredStatusCodes.Contains((int)code);
        }
    }
}