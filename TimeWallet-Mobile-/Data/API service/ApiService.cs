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
            //_httpClient.BaseAddress = new Uri("https://localhost:5173/api/timewallet/");
            _httpClient.BaseAddress = new Uri("https://timewallet.azurewebsites.net/api/timewallet/");
        }

        


        public async Task<UserBudgetResponseDTO> GetInformationAboutUser(string email)
        {
            try
            {
                string encodedEmail = Uri.EscapeDataString(email);
                var response = await _httpClient.GetAsync($"getInformationAboutUser/{encodedEmail}");

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

        public async Task<string> RegisterUserAsync(User user)
        {
            try
            {
                // Serialize the user object to JSON
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make the POST request to the API endpoint
                var response = await _httpClient.PostAsync("register", content);

                if (response.IsSuccessStatusCode)
                {
                    // Parse the success message from the response
                    var messageRAW = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(messageRAW);
                    return responseData.message;
                }
                else
                {
                    // Return error message if the response is not successful
                    return $"Error: {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                // Handle and return any exceptions that occur
                return $"Exception: {ex.Message}";
            }
        }

        public async Task<string> RegisterUser(UserDTO userDTO)
        {
            try
            {
                var json = JsonConvert.SerializeObject(userDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("register", content);

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

        public async Task<ReceiptDTO> GetReceiptAsync(string email, string receiptId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"getReceipt/{email}?receiptId={receiptId}");

                string jsonMessage = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(jsonMessage);
                string message = jsonDoc.RootElement.GetProperty("message").GetString();
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var receiptResponse = System.Text.Json.JsonSerializer.Deserialize<ReceiptDTO>(jsonResponse);
                    return receiptResponse;
                }
                if(message == "Receipt not found.")
                {
                    ReceiptDTO failed = new ReceiptDTO { Receipt = null, Items = new List<ReceiptItem>()};
                    return failed;
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

        public async Task<string> AddElementAsync(ElementDTO element, string email)
        {
            try
            {
                // Serialize the element object to JSON
                var json = JsonConvert.SerializeObject(element);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make the POST request to the API endpoint
                var response = await _httpClient.PostAsync($"addElement/{email}", content);

                if (response.IsSuccessStatusCode)
                {
                    // Parse the success message from the response
                    var messageRAW = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(messageRAW);

                    string successMessage = responseData.message;
                    return successMessage;
                }
                else
                {
                    // Return error message if the response is not successful
                    return $"Error: {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                // Handle and return any exceptions that occur
                return $"Exception: {ex.Message}";
            }
        }

        public async Task<string> DeleteElementAsync(string email, string id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"deleteElement/{email}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return jsonResponse;
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

        public async Task<string> AddUserReceiptAsync(string email, UsersReceiptsDTO receipt)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(receipt);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"addUserReceipt/{email}", content);
                string jsonMessage = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(jsonMessage);
                string message = jsonDoc.RootElement.GetProperty("message").GetString();
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize to extract message
                    var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    return responseObject.message.ToString(); // Return the success message
                }
                if(message == "User has already registered this receipt.")
                {
                    return message;
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

        public async Task<string> AddBudgetAsync(BudgetAddDTO budget, string email)
        {
            try
            {
                // Serialize the budget object to JSON
                var json = JsonConvert.SerializeObject(budget);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make the POST request to the API endpoint
                var response = await _httpClient.PostAsync($"addBudget/{email}", content);

                if (response.IsSuccessStatusCode)
                {
                    // Parse the success message from the response
                    var messageRAW = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(messageRAW);

                    string successMessage = responseData.message;
                    return successMessage;
                }
                else
                {
                    // Extract error message if available
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    var errorData = JsonConvert.DeserializeObject<dynamic>(errorResponse);

                    return errorData?.message != null ? errorData.message.ToString() : $"Error: {errorResponse}";
                }
            }
            catch (Exception ex)
            {
                // Handle and return any exceptions that occur
                return $"Exception: {ex.Message}";
            }
        }

        public async Task<string> DeleteBudgetAsync(string email, string budgetId)
        {
            try
            {
                // Serialize the budget ID to JSON as the API expects it in the body
                var json = JsonConvert.SerializeObject(budgetId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Create DELETE request with body
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"{_httpClient.BaseAddress}deleteBudget/{email}"),
                    Content = content
                };

                // Send request
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // Extract success message
                    var messageRAW = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(messageRAW);

                    string successMessage = responseData.message;
                    return successMessage;
                }
                else
                {
                    // Return error message if deletion fails
                    return $"Error: {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                // Handle and return exception details
                return $"Exception: {ex.Message}";
            }
        }

        public async Task<string> UpdateUserAsync(string email, string userName, string newEmail, string language, string theme)
        {
            try
            {
                // Create the request body as a JSON object
                var updateUserDTO = new
                {
                    UserName = userName,
                    NewEmail = newEmail,
                    Theme = theme,
                    Language = language
                };

                // Serialize the object to JSON
                var json = JsonConvert.SerializeObject(updateUserDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make the POST request to the API endpoint
                var response = await _httpClient.PostAsync($"updateUser/{email}", content);

                if (response.IsSuccessStatusCode)
                {
                    // Parse the success message from the response
                    var messageRAW = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(messageRAW);

                    string successMessage = responseData.message;
                    return successMessage;
                }
                else
                {
                    // Return error message if the response is not successful
                    return $"Error: {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                // Handle and return any exceptions that occur
                return $"Exception: {ex.Message}";
            }
        }

    }
}
