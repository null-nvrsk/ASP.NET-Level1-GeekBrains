using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient
    {
        protected string Address { get; }
        protected HttpClient Http { get; }

        protected BaseClient(IConfiguration config, string serviceAddress)
        {
            Address = serviceAddress;

            Http = new HttpClient
            {
                BaseAddress = new Uri(config["WebApiURL"]),
                DefaultRequestHeaders =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };
        }

        
    }
}
