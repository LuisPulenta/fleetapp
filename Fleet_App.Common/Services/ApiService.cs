using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Fleet_App.Common.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;

namespace Fleet_App.Common.Services
{
    public class ApiService : IApiService
    {
        public async Task<Response<UserResponse>> GetUserByEmailAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string email,
            string password)
        {
            try
            {
                var request = new UserRequest { Email = email,Password=password };
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<UserResponse>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var owner = JsonConvert.DeserializeObject<UserResponse>(result);
                return new Response<UserResponse>
                {
                    IsSuccess = true,
                    Result = owner
                };
            }
            catch (Exception ex)
            {
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<bool> CheckConnectionAsync(string url)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }
            return true;
            //return await CrossConnectivity.Current.IsRemoteReachable(url);
        }

        public async Task<Response<object>> GetRemotesForUser(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new UserIdRequest { Id = id };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var remotes = JsonConvert.DeserializeObject<List<Reclamo>>(answer);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = remotes
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response<object>> GetCablesForUser(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new UserIdRequest { Id = id };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var cables = JsonConvert.DeserializeObject<List<ReclamoCable>>(answer);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = cables
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response<object>> GetPrismasForUser(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new UserIdRequest { Id = id };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var prismas = JsonConvert.DeserializeObject<List<ReclamoPrisma>>(answer);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = prismas
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response<object>> GetTasasForUser(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new UserIdRequest { Id = id };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var tasas = JsonConvert.DeserializeObject<List<ReclamoTasa>>(answer);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = tasas
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response<object>> GetDtvsForUser(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new UserIdRequest { Id = id };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var dtvs = JsonConvert.DeserializeObject<List<ReclamoDtv>>(answer);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = dtvs
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response<object>> GetDevolucionesForUser(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new UserIdRequest { Id = id };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var tasas = JsonConvert.DeserializeObject<List<Devolucion>>(answer);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = tasas
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response<object>> GetAllsForUser(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var model = new UserIdRequest { Id = id };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var remotes = JsonConvert.DeserializeObject<List<Reclamo>>(answer);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = remotes
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response<object>> GetListAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string id,
            int? userid)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };

                var model = new ControlRequest { RECUPIDJOBCARD = id, UserID=userid };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");


                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<object>> GetList2Async<T>(
            string urlBase,
            string servicePrefix,
            string controller)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };


                var url = $"{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<object>> GetList3Async<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            int? id,
            int? userid)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };

                var model = new ControlCableRequest { ReclamoTecnicoID = id, UserID=userid };
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");


                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<Response<object>> PutAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            int id)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}/{id}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                return new Response<object>
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response<object>> PostAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }


                return new Response<object>
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response<object>> GetLastWebSesion(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}/{id}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var webSesion = JsonConvert.DeserializeObject<WebSesionRequest>(result);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = webSesion
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response2> GetTrabajos(string urlBase, string servicePrefix, string controller, TrabajosRequest model)
        {
            try
            {
                string request = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response2
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                List<TrabajosResponse> trabajos = JsonConvert.DeserializeObject<List<TrabajosResponse>>(result);
                return new Response2
                {
                    IsSuccess = true,
                    Result = trabajos
                };
            }
            catch (Exception ex)
            {
                return new Response2
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response2> GetTrabajos2(string urlBase, string servicePrefix, string controller, TrabajosRequest model)
        {
            try
            {
                string request = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response2
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                List<TrabajosResponse2> trabajos = JsonConvert.DeserializeObject<List<TrabajosResponse2>>(result);
                return new Response2
                {
                    IsSuccess = true,
                    Result = trabajos
                };
            }
            catch (Exception ex)
            {
                return new Response2
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response2> GetModelos(string urlBase, string servicePrefix, string controller, TrabajosRequest model)
        {
            try
            {
                string request = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response2
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                List<ModelosResponse> trabajos = JsonConvert.DeserializeObject<List<ModelosResponse>>(result);
                return new Response2
                {
                    IsSuccess = true,
                    Result = trabajos
                };
            }
            catch (Exception ex)
            {
                return new Response2
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}