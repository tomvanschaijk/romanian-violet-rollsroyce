using RomanianVioletRollsRoyce.Crosscutting.Responses;
using RomanianVioletRollsRoyce.Data.DTO;
using RomanianVioletRollsRoyce.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RomanianVioletRollsRoyce.Crosscutting.Context;

namespace RomanianVioletRollsRoyce.Web.Clients
{
    public class CustomersClient : ICustomersClient
    {
        private readonly HttpClient _httpClient;
        private readonly APIConfiguration _apiConfiguration;
        private readonly RequestContext _requestContext;
        private readonly ILogger<CustomersClient> _logger;

        public CustomersClient(HttpClient httpClient, APIConfiguration apiConfiguration, RequestContext requestContext, ILogger<CustomersClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiConfiguration = apiConfiguration ?? throw new ArgumentNullException(nameof(apiConfiguration));
            _requestContext = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CustomerAccountDataResponse> GetCustomerAccountData()
        {
            try
            {
                _logger.LogInformation("Getting customer account data");

                var uri = _httpClient.BaseAddress + _apiConfiguration.AccountDataEndpoint;
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(uri))
                {
                    Headers = { { "CorrelationId", _requestContext?.CorrelationId } }
                };

                var apiResponse = await _httpClient.SendAsync(request);
                return apiResponse.IsSuccessStatusCode
                        ? await HandleSuccessResponse(apiResponse)
                        : await HandleErrorResponse(apiResponse);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Error while getting customer account data: {@Exception}", exc);
                return new CustomerAccountDataResponse
                {
                    Success = false,
                    ErrorMessage = exc.Message
                };
            }
        }

        private async Task<CustomerAccountDataResponse> HandleSuccessResponse(HttpResponseMessage apiResponse)
        {
            var response = new DataResponse<ICollection<CustomerAccountData>>();
            if (apiResponse.Content != null)
            {
                response = await apiResponse.Content.ReadAsAsync<DataResponse<ICollection<CustomerAccountData>>>();
            }

            return new CustomerAccountDataResponse
            {
                Success = true,
                Data = response.Data,
                ErrorMessage = string.Empty
            };
        }

        private async Task<CustomerAccountDataResponse> HandleErrorResponse(HttpResponseMessage apiResponse)
        {
            var response = await apiResponse.Content.ReadAsAsync<ErrorResponse>();
            return new CustomerAccountDataResponse
            {
                Success = false,
                ErrorMessage = $"Error while getting customer account data: {string.Join("\n", response.Errors.Select(e => e.Title))}"
            };
        }
    }
}
