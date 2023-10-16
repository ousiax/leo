﻿using Alyio.Extensions;
using HoneyLovely.Models;
using HoneyLovely.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace HoneyLovely.Services
{
    internal sealed class MemberDetailService : IMemberDetailService
    {
        private readonly HttpClient _http;

        public MemberDetailService(IHttpClientFactory httpClientFactory, IOptions<WebHostAddressOptions> addressProvider)
        {
            _http = httpClientFactory.CreateClient();
            _http.BaseAddress = addressProvider.Value.BaseAddress;
        }

        public async Task<int> CreateAsync(MemberDetail detail)
        {
            var response = await _http.PostAsJsonAsync("/member-details", detail);
            response.EnsureSuccessStatusCode();
            var count = await response.Content.ReadAsStringAsync();
            return count.ToInt32().Value;
        }
    }
}
