using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TimeWallet_Mobile_.Data.DTO_s;
using TimeWallet_Mobile_.Data.DTO_s.Json;
using TimeWallet_Mobile_.Data.Models;

namespace TimeWallet_Mobile_.Data.API_service
{


    public class ApiService
    {

        private readonly HttpClient _httpClient;


        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:5173/api/timewallet/");
        }

        public async Task<UserBudgetResponseDTO> GetInformationAboutUser(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"getInformationAboutUser/{email}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var userBudgetResponse = JsonConvert.DeserializeObject<UserBudgetResponseDTO>(jsonResponse);


                    return userBudgetResponse;
                }
                else
                {
                    throw new Exception($"Error: {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception: {ex.Message}");
            }
        }

        public async Task<UserDTO> GetUserAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"getUser/{email}");

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {errorMessage}");
                    return null;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON object properly
                UserDTO user = JsonConvert.DeserializeObject<UserDTO>(jsonResponse);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetUserAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<string> LoginUser(LoginModelDTO login)
        {
            try
            {
                var json = JsonConvert.SerializeObject(login);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("login", content);

                if (response.IsSuccessStatusCode)
                {
                    var messageRAW = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(messageRAW);

                    string successMessage = responseData.message;
                    return successMessage;
                }
                else
                {
                    return $"Error: {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }

        }

        public async Task<BudgetDTO> GetLastBudgetAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"getLastBudget/{email}");


                string id = await SecureStorage.GetAsync("UserEmail");


                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {errorMessage}");
                    throw new ArgumentException($"API Error: {errorMessage}");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {jsonResponse}"); // ✅ Print the full JSON response

                BudgetDTO responseObject = System.Text.Json.JsonSerializer.Deserialize<BudgetDTO>(jsonResponse);
                return responseObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetLastBudgetAsync: {ex}"); // ✅ Print full exception
                throw;
            }
        }

        public async Task<List<Elements>> GetLastElementsAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"getLastTenElements/{email}");

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {errorMessage}");
                    throw new ArgumentException($"API Error: {errorMessage}");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {jsonResponse}"); // ✅ Print full JSON response

                var responseObject = System.Text.Json.JsonSerializer.Deserialize<LastElementsDTO>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return responseObject?.LastElements ?? new List<Elements>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetLastElementsAsync: {ex}"); // ✅ Print full exception
                throw;
            }
        }
    }

    


}
