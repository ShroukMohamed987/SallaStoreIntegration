using Microsoft.Extensions.Options;
using Newtonsoft.Json;

//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SallaStoreIntegration.Dtos.Customers;
using SallaStoreIntegration.Repositories.Base;
using SallaStoreIntegration.Setting;
using System.Text;
using System.Text.Json;

namespace SallaStoreIntegration.Repositories.Customer
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(IHttpClientFactory httpClientFactory, IOptions<ExternalStoresSetting> settings)
            : base(httpClientFactory, settings)
        {
        }

        //public async Task<bool> CreateCustomerAsync(CreateCustomerDTO inputDto, string token)
        //{
        //    try
        //    {
        //        var client = CreateClient(token);


        //        var data = JsonSerializer.Serialize(inputDto);
        //        var content = new StringContent(data, Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await client.PostAsync("customers", content);
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        public async Task<int?> CreateCustomerAsync(CreateCustomerDTO inputDto, string token)
        {
            try
            {
                var client = CreateClient(token);
                var data = JsonConvert.SerializeObject(inputDto);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("customers", content);

                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(responseBody);

                return result?.data?.id;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<CustomerResponseDto>> GetCustomerListAsync(string token)
        {
            try
            {
                var client = CreateClient(token);
                HttpResponseMessage response = await client.GetAsync("customers");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var resultJson = JObject.Parse(content);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerResponseDto>>(resultJson["data"].ToString());
                }

                return new List<CustomerResponseDto>();
            }
            catch
            {
                return new List<CustomerResponseDto>();
            }
        }

        public async Task<CustomerResponseDto> GetCustomerByIdAsync(string token, int id)
        {
            try
            {
                var client = CreateClient(token);
                HttpResponseMessage response = await client.GetAsync($"customers/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonData = JObject.Parse(content);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerResponseDto>(jsonData["data"].ToString());
                }

                return new CustomerResponseDto();
            }
            catch
            {
                return new CustomerResponseDto();
            }
        }

        public async Task<bool> UpdateCustomerAsync(string token, int id, CreateCustomerDTO inputDto)
        {
            try
            {
                var client = CreateClient(token);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(inputDto);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync($"customers/{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
