using Newtonsoft.Json;
using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementApp.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public Repository(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

        public async Task<object> Authenticate(string email, string password)
        {
            try
            {
                var credentials = new LoginVM { Email = email, Password = password };
                var url = $"{URL.AuthenticateAPIPath}";
                var response = await _httpClientFactory.CreateClient().PostAsJsonAsync(url, credentials);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);
                    string token = jsonResponse.token;
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var jwtToken = jwtHandler.ReadJwtToken(token);

                    var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                    if (roleClaim == null)
                    {
                        throw new Exception("Role not found in token.");
                    }

                    if (roleClaim == "Admin")
                    {
                        var admin = JsonConvert.DeserializeObject<Admin>(responseData);
                        admin.Token = token; // Ensure the token is set
                        return admin;
                    }
                    else if (roleClaim == "Teacher")
                    {
                        var teacher = JsonConvert.DeserializeObject<Teacher>(responseData);
                        teacher.Token = token; // Ensure the token is set
                        return teacher;
                    }
                    else
                    {
                        throw new Exception("Unknown user type.");
                    }
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Authentication failed: {errorMessage}");
                }
                else
                {
                    throw new Exception($"Authentication failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public async Task<bool> CreateAsync(string url, T ObjToCreate)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, url);
			if (ObjToCreate != null)
			{
				request.Content = new StringContent(JsonConvert.SerializeObject(ObjToCreate),
					Encoding.UTF8, "application/json");
			}
			var client = _httpClientFactory.CreateClient();
			HttpResponseMessage response = await client.SendAsync(request);
			return response.StatusCode == HttpStatusCode.Created;
		}

		public async Task<bool> DeleteAsync(string url, int id)
		{
			var request = new HttpRequestMessage(HttpMethod.Delete, url + "/" + id.ToString());
			var client = _httpClientFactory.CreateClient();
			HttpResponseMessage response = await client.SendAsync(request);
			return response.StatusCode == HttpStatusCode.OK;
		}

		public async Task<IEnumerable<T>> GetAllAsync(string url)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, url);
			var client = _httpClientFactory.CreateClient();
			HttpResponseMessage response = await client.SendAsync(request);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				var jsonstring = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonstring);
			}
			return null;
		}

		public async Task<T> GetAsync(string url, int id)
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get, url + "/" + id.ToString());
				var client = _httpClientFactory.CreateClient();
				HttpResponseMessage response = await client.SendAsync(request);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var jsonstring = await response.Content.ReadAsStringAsync();
					return JsonConvert.DeserializeObject<T>(jsonstring);
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<bool> IsUniqueUser(string Email)
		{
			try
			{
				var url = $"{URL.RegisterAPIPath}/IsUniqueUser/{Email}";
				var response = await _httpClientFactory.CreateClient().GetAsync(url);
				return response.IsSuccessStatusCode;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<Admin> Register(string url, Admin admin)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, url);
			if (admin != null)
			{
				request.Content = new StringContent(JsonConvert.SerializeObject(admin),
					Encoding.UTF8, "application/json");
			}
			var client = _httpClientFactory.CreateClient();
			HttpResponseMessage response = await client.SendAsync(request);
			if (response.StatusCode == HttpStatusCode.Created)
				return admin;
			else
				return null;
		}

		public async Task<bool> UpdateAsync(string url, T ObjToUpdate)
		{
			var request = new HttpRequestMessage(HttpMethod.Put, url);
			if (ObjToUpdate != null)
			{
				request.Content = new StringContent(JsonConvert.SerializeObject(ObjToUpdate),
					Encoding.UTF8, "application/json");
			}
			var client = _httpClientFactory.CreateClient();
			HttpResponseMessage response = await client.SendAsync(request);
			return response.StatusCode == HttpStatusCode.NoContent;
		}
        public async Task<bool> ExistAsync(string url, T ObjToCheck)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url + "/" + ObjToCheck.ToString());
                var client = _httpClientFactory.CreateClient();
                HttpResponseMessage response = await client.SendAsync(request);

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
