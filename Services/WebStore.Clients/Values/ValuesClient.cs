using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Clients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        public ValuesClient(IConfiguration config) : base(config, "api/values") { }


        public IEnumerable<string> Get()
        {
            var response = Http.GetAsync(Address).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<IEnumerable<string>>().Result;

            return Enumerable.Empty<string>();
        }

        public string Get(int id)
        {
            var response = Http.GetAsync($"{Address}/{id}").Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<string>().Result;

            return string.Empty;
        }

        public Uri Create(string value)
        {
            var response = Http.PostAsJsonAsync(Address, value).Result;
            return response.EnsureSuccessStatusCode().Headers.Location;
        }

        public HttpStatusCode Edit(int id, string newValue)
        {
            var response = Http.PutAsJsonAsync($"{Address}/{id}", newValue).Result;
            return response.EnsureSuccessStatusCode().StatusCode;
        }

        public bool Remove(int id)
        {
            var response = Http.DeleteAsync($"{Address}/{id}").Result;
            return response.IsSuccessStatusCode;
        }
    }
}
