using ExternalStores.DTO.Salla;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Brands;
using SallaStoreIntegration.Dtos.Customers;
using SallaStoreIntegration.Setting;
using System.Net.Http.Headers;
using System.Text;

namespace SallaStoreIntegration.BLL
{
    public class SallaBLL : ISallaBLL
    {
        private ExternalStoresSetting _externalStoresSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string category = "categories";
        private const string order = "orders";

        public SallaBLL(IOptions<ExternalStoresSetting> DexefExternalStores, IHttpClientFactory httpClientFactory)
        {
            _externalStoresSettings = DexefExternalStores.Value;
            _httpClientFactory = httpClientFactory;
        }

        #region Auth
        public async Task<string> AuthorizeAsync()
        {

            var challengeUrl = $"{_externalStoresSettings.SallaConfigSetting.AuthUri}?" +
                                     $"{AuthSallaParameterEnum.client_id.ToString()}={_externalStoresSettings.SallaConfigSetting.ClientId}&" +
                                     $"{AuthSallaParameterEnum.response_type.ToString()}={_externalStoresSettings.SallaConfigSetting.ResponseType}&" +
                                     $"{AuthSallaParameterEnum.redirect_uri.ToString()}={_externalStoresSettings.SallaConfigSetting.RedirectUri}&" +
                                     $"{AuthSallaParameterEnum.scope.ToString()}={_externalStoresSettings.SallaConfigSetting.Scope}&" +
                                     $"{AuthSallaParameterEnum.state.ToString()}={new Random().Next(111111111, 999999999).ToString()}";

            return challengeUrl;
        }

        public async Task<LoginResultDto> CallbackAsync(string code, string state)
        {

            var response = new LoginResultDto();

            try
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _externalStoresSettings.SallaConfigSetting.TokenUri);

                var formData = PrepareFormData(code, state, true);

                tokenRequest.Content = new FormUrlEncodedContent(formData);

                var tokenResponse = await _httpClientFactory.CreateClient().SendAsync(tokenRequest);
                tokenResponse.EnsureSuccessStatusCode();

                if (!tokenResponse.IsSuccessStatusCode)
                {
                    response.ErrorMessage = tokenResponse.StatusCode.ToString();
                    return response;
                }

                var tokenResponseBody = await tokenResponse.Content.ReadAsStringAsync();

                response = JsonConvert.DeserializeObject<LoginResultDto>(tokenResponseBody);


                return response;
            }
            catch (Exception e)
            {
                response.ErrorMessage += e.ToString();
                return response;
            }

        }



        public async Task<LoginResultDto> RefreshTokenAsync(string refreshToken)
        {

            var response = new LoginResultDto();

            try
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _externalStoresSettings.SallaConfigSetting.TokenUri);

                var formData = PrepareFormData(null, null, false, refreshToken);

                tokenRequest.Content = new FormUrlEncodedContent(formData);

                var tokenResponse = await _httpClientFactory.CreateClient().SendAsync(tokenRequest);
                tokenResponse.EnsureSuccessStatusCode();

                if (!tokenResponse.IsSuccessStatusCode)
                {
                    response.ErrorMessage = tokenResponse.StatusCode.ToString();
                    return response;
                }

                var tokenResponseBody = await tokenResponse.Content.ReadAsStringAsync();


                response = JsonConvert.DeserializeObject<LoginResultDto>(tokenResponseBody);

                return response;
            }
            catch (Exception e)
            {
                response.ErrorMessage += e.ToString();
                return response;
            }

        }
        #endregion


        #region Category
        public async Task<bool> CreateCategoryAsync(CreateCategoryDto inputDto, string token)
        {

            try
            {
                var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());

                // Customize headers as needed
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Serialize the categoryInput object to JSON
                string data = JsonConvert.SerializeObject(inputDto);

                // Create a StringContent with the serialized data
                var content = new StringContent(data, Encoding.UTF8);

                // Make the POST request
                HttpResponseMessage result = await client.PostAsync(category, content);

                if (result.IsSuccessStatusCode)
                    return true;

                else
                    return false;
            }
            catch (Exception e)
            {

                return false;
            }


        }
        public async Task<List<CategoryResultDto>> GetCategoriesAsync(string token)
        {
            var response = new List<CategoryResultDto>();
            try
            {
                var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());

                // Customize headers as needed
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Make the GET request
                HttpResponseMessage result = await client.GetAsync(category);

                if (result.IsSuccessStatusCode)
                {
                    // Read and return the response content as a string
                    string content = await result.Content.ReadAsStringAsync();
                    var resultJson = JObject.Parse(content);
                    List<CategoryResultDto> categories = JsonConvert.DeserializeObject<List<CategoryResultDto>>(resultJson["data"].ToString());

                    return categories;
                }
                else
                {
                    return new List<CategoryResultDto>() { new CategoryResultDto { Message = "Invaled Token" } };
                }
            }
            catch (Exception e)
            {

                return new List<CategoryResultDto>() { new CategoryResultDto { Message = e.Message.ToString() } };
            }



        }

        public async Task<bool> UpdateCategoriesAsync(string token, int id, UpdateCategoryInputDto inputDto)
        {
           // var response = new CategoryUpdateResultDto();
            try
            {
                var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string data = JsonConvert.SerializeObject(inputDto);

                
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage result = await client.PutAsync($"{category}/{id}", content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception e)
            {
                //response.Message = e.Message;
                return false;
            }
        }


        //public async Task<List<CategoryResultDto>> GetCategoriesAsync(string key, string secret, string url)
        //{
        //    var response = new List<CategoryResultDto>();
        //    try
        //    {
        //        RestAPI rest = new RestAPI("https://cottonhousekidswear.com/wp-json/wc/v3/", "ck_49be46229384a6cc536d004b11267798c5234315", "cs_e566be5dd4a7cad1bda7ec169252ca004f763e2c");
        //        WCObject wc = new WCObject(rest);
        //        var products = await wc.Product.GetAll();
        //        return response.CreateResponse(new List<CategoryResultDto> { });

        //    }
        //    catch (Exception e)
        //    {

        //        return response.CreateResponse(MessageCodes.Exception, e.Message.ToString());
        //    }
        //}
        #endregion

        #region Product
        public async Task<bool> CreateProductAsync(CreateProductDto inputDto, string token)
        {

            try
            {
                var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());

                // Customize headers as needed
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Serialize the categoryInput object to JSON
                string data = JsonConvert.SerializeObject(inputDto);

                // Create a StringContent with the serialized data
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                // Make the POST request
                HttpResponseMessage result = await client.PostAsync("products", content);

                if (result.IsSuccessStatusCode)
                    return true;

                else
                    return false;
            }
            catch (Exception e)
            {

                return false;
            }


        }
        public async Task<List<ProductResultDto>> GetProductsAsync(FilterProductDto filterDto)
        {
            var response = new List<ProductResultDto>();

            try
            {
                var client = _httpClientFactory.CreateClient("SallaApi");

                // Customize headers as needed
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", filterDto.Token);

                var queryParameters = new List<string>
                                                                {
                                                                    $"{nameof(filterDto.category)}={filterDto?.category}",
                                                                    $"{nameof(filterDto.page)}={filterDto?.page}",
                                                                    $"{nameof(filterDto.keyword)}={filterDto?.keyword}",
                                                                    $"{nameof(filterDto.status)}={filterDto?.status}",
                                                                    $"{nameof(filterDto.per_page)}={filterDto?.per_page}",

                                                                };

                // Build the request URL with the query parameters
                var requestUrl = $"products?{string.Join("&", queryParameters)}";

                // Make the GET request
                HttpResponseMessage result = await client.GetAsync(requestUrl);

                if (result.IsSuccessStatusCode)
                {
                    // Read and return the response content as a string
                    string content = await result.Content.ReadAsStringAsync();
                    var resultJson = JObject.Parse(content);


                    List<ProductResultDto> products = JsonConvert.DeserializeObject<List<ProductResultDto>>(resultJson["data"].ToString());

                    return products;
                }
                else
                {
                    return new List<ProductResultDto>() { new ProductResultDto { Message = "Invalid Token" } };
                }
            }
            catch (Exception e)
            {

                return new List<ProductResultDto>() { new ProductResultDto { Message = e.Message.ToString() } };
            }

        }



        #endregion


        #region Order
        public async Task<List<OrderResultDto>> GetOrdersAsync(string token)
        {
            var response = new List<OrderResultDto>();
            try
            {
                var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());

                // Customize headers as needed
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Make the GET request
                HttpResponseMessage result = await client.GetAsync(order);

                if (result.IsSuccessStatusCode)
                {
                    // Read and return the response content as a string
                    string content = await result.Content.ReadAsStringAsync();
                    var resultJson = JObject.Parse(content);
                    List<OrderResultDto> orders = JsonConvert.DeserializeObject<List<OrderResultDto>>(resultJson["data"].ToString());

                    return orders;
                }
                else
                {
                    return new List<OrderResultDto>() { new OrderResultDto { ErrorMessage = "Invaled Token" } };
                }
            }
            catch (Exception e)
            {

                return new List<OrderResultDto>() { new OrderResultDto { ErrorMessage = e.Message.ToString() } };
            }

        }
        public async Task<OrderResultDto> GetOrderByIdAsync(string token, int id)
        {
            var response = new List<OrderResultDto>();
            try
            {
                var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());

                // Customize headers as needed
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Make the GET request
                HttpResponseMessage result = await client.GetAsync($"{order}/{id}");

                if (result.IsSuccessStatusCode)
                {
                    // Read and return the response content as a string
                    string content = await result.Content.ReadAsStringAsync();
                    var resultJson = JObject.Parse(content);
                    OrderResultDto order = JsonConvert.DeserializeObject<OrderResultDto>(resultJson["data"].ToString());

                    return order;
                }
                else
                {
                    return new OrderResultDto { ErrorMessage = "Invaled Token" };
                }
            }
            catch (Exception e)
            {

                return new OrderResultDto { ErrorMessage = e.Message.ToString() };
            }
        }
        public async Task<bool> UpdateOrderStatusAsync(string token, int id, UpdateOrderStatusInputDto inputDto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());

                // Customize headers as needed
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Serialize the categoryInput object to JSON
                string data = JsonConvert.SerializeObject(inputDto);

                // Create a StringContent with the serialized data
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                // Make the POST request
                HttpResponseMessage result = await client.PostAsync($"{order}/{id}/status", content);

                if (result.IsSuccessStatusCode)
                    return true;

                else
                {
                    string errorResponse = await result.Content.ReadAsStringAsync();
                    Console.WriteLine("Error Response: " + errorResponse);
                    return false;
                }

            }
            catch (Exception e)
            {

                return false;
            }
        }

        public async Task<List<StatusResultDto>> GetOrderStatusAsync(string token)
        {
            var response = new List<StatusResultDto>();
            try
            {
                var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());

                // Customize headers as needed
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Make the GET request
                HttpResponseMessage result = await client.GetAsync($"{order}/statuses");

                if (result.IsSuccessStatusCode)
                {
                    // Read and return the response content as a string
                    string content = await result.Content.ReadAsStringAsync();
                    var resultJson = JObject.Parse(content);
                    var status = JsonConvert.DeserializeObject<List<StatusResultDto>>(resultJson["data"].ToString());

                    return status;
                }
                else
                {
                    return new List<StatusResultDto>() { new StatusResultDto { ErrorMessage = "Invaled Token" } };
                }
            }
            catch (Exception e)
            {

                return new List<StatusResultDto>() { new StatusResultDto { ErrorMessage = e.Message.ToString() } };
            }

        }
        #endregion

        #region Helper
        private List<KeyValuePair<string, string>> PrepareFormData(string? code, string? state, bool callback, string? refreshToken = null)
        {
            var formData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(AuthSallaParameterEnum.client_id.ToString(),  _externalStoresSettings.SallaConfigSetting.ClientId),
                new KeyValuePair<string, string>(AuthSallaParameterEnum.client_secret.ToString(),  _externalStoresSettings.SallaConfigSetting.ClientSecret),
                new KeyValuePair<string, string>(AuthSallaParameterEnum.redirect_uri.ToString(), _externalStoresSettings.SallaConfigSetting.RedirectUri),
            };
            if (callback)
            {
                formData.AddRange(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>(AuthSallaParameterEnum.grant_type.ToString(),  _externalStoresSettings.SallaConfigSetting.GrantType),
                    new KeyValuePair<string, string>(AuthSallaParameterEnum.code.ToString(), code),
                    new KeyValuePair<string, string>(AuthSallaParameterEnum.state.ToString(), state),
                    new KeyValuePair<string, string>(AuthSallaParameterEnum.scope.ToString(),  _externalStoresSettings.SallaConfigSetting.Scope),
                });
            }
            else
            {
                formData.AddRange(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>(AuthSallaParameterEnum.grant_type.ToString(), "refresh_token"),
                    new KeyValuePair<string, string>(AuthSallaParameterEnum.refresh_token.ToString(), refreshToken),
                });
            }
            return formData;

        }

        #region Brands

        public async Task<bool> CreateBrandAsync(CreateBrandDto inputDto, string token)
        {
            var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var data = JsonConvert.SerializeObject(inputDto);
            var content = new StringContent(data, Encoding.UTF8);

            HttpResponseMessage response = await client.PostAsync("brands",content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Customers

        public async Task<bool> CreateCustomerAsync(CreateCustomerDTO inputDto, string token)
        {
            var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var data = JsonConvert.SerializeObject(inputDto);
            var content = new StringContent(data,Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("customers", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<List<CustomerResponseDto>> GetCustomerListAsync(string token)
        {
            var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respone = await client.GetAsync("customers");
            if(respone.IsSuccessStatusCode)
            {
                string content = await respone.Content.ReadAsStringAsync();
                var resultJson = JObject.Parse(content);
               List<CustomerResponseDto> customers = JsonConvert.DeserializeObject<List<CustomerResponseDto>>(resultJson["data"].ToString());
                return customers;
            }
            return new List<CustomerResponseDto> { };
        }

        public  async Task<CustomerResponseDto> GetCustomerByIdAsync(string token, int id)
        {
           
            var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.GetAsync($"customers/{id}");
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonData = JObject.Parse(content);
                CustomerResponseDto customer = JsonConvert.DeserializeObject<CustomerResponseDto>(jsonData["data"].ToString());
                return customer;
            }
            return new CustomerResponseDto { }; 
        }

        public async Task<bool> UpdateCustomerAsync(string token, int id, CreateCustomerDTO inputDto)
        {
            var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var data = JsonConvert.SerializeObject(inputDto);
            var content = new StringContent(data,Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"customers/{id}", content);

            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        #endregion

        #endregion

    }
}
