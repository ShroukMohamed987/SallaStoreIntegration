using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SallaStoreIntegration.Setting;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SallaStoreIntegration.Repositories.Base
{
    public abstract class BaseRepository
    {
        protected const string BaseUrl = "https://api.salla.dev/admin/v2";
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly ExternalStoresSetting _settings;

        protected BaseRepository(IHttpClientFactory httpClientFactory, IOptions<ExternalStoresSetting> settings)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
        }

        protected HttpClient CreateClient(string token = null)
        {
            var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        //protected HttpClient CreateDefaultClient(string token = null)
        //{
        //    var client = _httpClientFactory.CreateClient();
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    }
        //    return client;
        //}
        protected HttpClient CreateDefaultClient(string token = null)
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(2); // ? add this
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        protected async Task<T> GetAsync<T>(string endpoint, string token) where T : class, new()
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.GetAsync($"{BaseUrl}/{endpoint}");
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(
                        content,  //  full response, not resultJson["data"]
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            Converters = new List<Newtonsoft.Json.JsonConverter> { new StringEnumConverter() }
                        }
                    );
                }

                return CreateErrorResponse<T>(content, result.StatusCode);
            }
            catch (Exception e)
            {
                return CreateExceptionResponse<T>(e.Message);
            }
        }

        protected async Task<List<T>> GetListAsync<T>(string endpoint, string token) where T : class, new()
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.GetAsync($"{BaseUrl}/{endpoint}");
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    var resultJson = JObject.Parse(content);
                    return JsonConvert.DeserializeObject<List<T>>(resultJson["data"].ToString());
                }

                return new List<T> { CreateErrorResponse<T>(content, result.StatusCode) };
            }
            catch (Exception e)
            {
                return new List<T> { CreateExceptionResponse<T>(e.Message) };
            }
        }

        protected async Task<T> PostAsync<T, TRequest>(string endpoint, TRequest request, string token) where T : class, new()
        {
            try
            {
                var client = CreateDefaultClient(token);
                var json = JsonConvert.SerializeObject(request);
               
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await client.PostAsync($"{BaseUrl}/{endpoint}", httpContent);
                var content = await result.Content.ReadAsStringAsync();
                
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(
                        content,  //  full response, not resultJson["data"]
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            Converters = new List<Newtonsoft.Json.JsonConverter> { new StringEnumConverter() }
                        }
                    );
                }

                return CreateErrorResponse<T>(content, result.StatusCode);
            }
            catch (Exception e)
            {
                return CreateExceptionResponse<T>(e.Message);
            }
        }

        protected async Task<bool> PostBoolAsync<TRequest>(string endpoint, TRequest request, string token)
        {
            try
            {
                var client = CreateClient(token);
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await client.PostAsync(endpoint, httpContent);
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        protected async Task<T> PutAsync<T, TRequest>(string endpoint, TRequest request, string token) where T : class, new()
        {
            try
            {
                var client = CreateDefaultClient(token);

                // Serialize with Newtonsoft
                var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new List<Newtonsoft.Json.JsonConverter> { new StringEnumConverter() }
                });

                var content = new StringContent(json, new UTF8Encoding(false), "application/json");

                var result = await client.PutAsync($"{BaseUrl}/{endpoint}", content);
                var responseContent = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(
                        responseContent,  //  full response, not resultJson["data"]
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            Converters = new List<Newtonsoft.Json.JsonConverter> { new StringEnumConverter() }
                        }
                    );
                }

                return CreateErrorResponse<T>(responseContent, result.StatusCode);
            }
            catch (Exception e)
            {
                return CreateExceptionResponse<T>(e.Message);
            }
        }

        protected async Task<bool> PutBoolAsync<TRequest>(string endpoint, TRequest request, string token)
        {
            try
            {
                var client = CreateClient(token);
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await client.PutAsync(endpoint, httpContent);
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        protected async Task<T> DeleteAsync<T>(string endpoint, string token) where T : class, new()
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.DeleteAsync($"{BaseUrl}/{endpoint}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Converters = new List<Newtonsoft.Json.JsonConverter> { new StringEnumConverter() }
                    });
                }
                return CreateErrorResponse<T>(content, result.StatusCode);
            }
            catch (Exception e)
            {
                return CreateExceptionResponse<T>(e.Message);
            }
        }
        protected async Task<bool> BoolDeleteAsync(string endpoint, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.DeleteAsync($"{BaseUrl}/{endpoint}");
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        protected string GetErrorMessage(string content, System.Net.HttpStatusCode statusCode)
        {
            try
            {
                var error = JObject.Parse(content);
                return error["error"]?["message"]?.ToString() ?? statusCode.ToString();
            }
            catch
            {
                return statusCode.ToString();
            }
        }

        public T CreateErrorResponse<T>(string content, System.Net.HttpStatusCode statusCode) where T : class, new()
        {
            var instance = new T();
            var messageProperty = typeof(T).GetProperty("Message");
            if (messageProperty != null)
            {
                messageProperty.SetValue(instance, GetErrorMessage(content, statusCode));
            }
            return instance;
        }

        public T CreateExceptionResponse<T>(string message) where T : class, new()
        {
            var instance = new T();
            var messageProperty = typeof(T).GetProperty("Message");
            if (messageProperty != null)
            {
                messageProperty.SetValue(instance, message);
            }
            return instance;
        }
    }
}
