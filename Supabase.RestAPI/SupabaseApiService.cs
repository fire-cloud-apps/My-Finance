// Supabase.RestAPI/SupabaseApiService.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using MyFinance.Model; // Added for IHttpClientFactory

// IAuthTokenProvider and AuthTokenProvider are now part of this namespace
namespace Supabase.RestAPI
{
    /// <summary>
    /// A generic service for interacting with Supabase REST API for any given model type.
    /// The table name is inferred from the model type name (e.g., Goal model maps to "Goal" table).
    /// HttpClient must be pre-configured with the Supabase base URL and API key.
    /// Authorization token is dynamically provided via IAuthTokenProvider.
    /// </summary>
    /// <typeparam name="TModel">The type of the Supabase table model (e.g., Goal, GoalEntry).</typeparam>
    public class SupabaseApiService<TModel> where TModel : SupabaseModel
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthTokenProvider _authTokenProvider;
        private readonly string _tableName;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupabaseApiService{TModel}"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HttpClientFactory instance provided by dependency injection.</param>
        /// <param name="authTokenProvider">The service providing the current authentication token.</param>
        public SupabaseApiService(IHttpClientFactory httpClientFactory, IAuthTokenProvider authTokenProvider)
        {
            // Use the named client to ensure the correct base URL and API key are used
            _httpClient = httpClientFactory.CreateClient("SupabaseApi");
            _authTokenProvider = authTokenProvider ?? throw new ArgumentNullException(nameof(authTokenProvider));
            _tableName = typeof(TModel).Name; // Infer table name from the model type name

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Adjust as per your Supabase column naming convention if different from default
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Don't send null properties
                WriteIndented = true // For readability in debugging, can remove in production
            };
        }

        public string AccessToken { get; set; }

        /// <summary>
        /// Helper method to send HTTP requests to Supabase.
        /// Sets the Authorization header dynamically before sending.
        /// </summary>
        /// <param name="method">The HTTP method (GET, POST, PATCH, DELETE).</param>
        /// <param name="endpoint">The API endpoint relative to the base URL.</param>
        /// <param name="content">Optional HTTP content for POST/PATCH requests.</param>
        /// <param name="preferHeader">Optional Prefer header value, e.g., "return=representation".</param>
        /// <param name="rangeHeader">Optional Range header value for pagination, e.g., "0-9".</param>
        /// <returns>The HttpResponseMessage from the Supabase API.</returns>
        private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string endpoint, HttpContent content = null, string preferHeader = null, string rangeHeader = null)
        {
            //Console.WriteLine("SendRequest - Triggered!");
            var request = new HttpRequestMessage(method, endpoint);
            try
            {
                #region Authorization Header
                // Dynamically set the Authorization header using the token provider
                var token = AccessToken; //_authTokenProvider.GetAuthToken();

                if (token == null)
                {
                    throw new ArgumentNullException(nameof(token), "AccessToken is null or empty!. Login again.");
                }

                //Console.WriteLine("SupabaseAPIService -> SendRequest:Token Exists?: " + !string.IsNullOrEmpty(token)); // For debugging purposes, can be removed in production   
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    //Console.WriteLine("SendRequest - Authorization Set!");
                }
                else
                {
                    // Clear any old token if user logged out or token expired
                    request.Headers.Authorization = null;
                }
                #endregion

                if (content != null)
                {
                    request.Content = content;
                }

                if (!string.IsNullOrEmpty(preferHeader))
                {
                    request.Headers.Add("Prefer", preferHeader);
                }


                if (!string.IsNullOrEmpty(rangeHeader))
                {
                    // request.Headers.Add("Range", rangeHeader);
                    Console.WriteLine($"SendRequest - Range Header Initiated.- {rangeHeader}"); // For debugging purposes, can be removed in production
                    var parts = rangeHeader.Split('-');
                    if (parts.Length == 2 && long.TryParse(parts[0], out var from) && long.TryParse(parts[1], out var to))
                    {
                        //request.Headers.Range = new RangeHeaderValue(from, to);
                        //request.Headers.Add("Range", rangeHeader);
                        request.Headers.TryAddWithoutValidation("Range", rangeHeader);
                        Console.WriteLine("SendRequest - 1. Range Header Set: " + rangeHeader); // For debugging purposes, can be removed in production
                    }
                    else
                    {
                        throw new ArgumentException("Invalid rangeHeader format. Expected 'start-end'.");
                    }

                    Console.WriteLine("SendRequest - 2. Range Header Set: " + rangeHeader); // For debugging purposes, can be removed in production
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                Console.WriteLine("SendRequest - Exception: " + ex.Message);
            }

            return await _httpClient.SendAsync(request);
        }

        /// <summary>
        /// Creates a new record in the Supabase table.
        /// </summary>
        /// <param name="item">The item to create.</param>
        /// <returns>A <see cref="SupabaseResult{TModel}"/> indicating success/failure and the created data if successful.</returns>
        public async Task<SupabaseResult<TModel>> Create(TModel item)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(item, _jsonSerializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await SendRequest(HttpMethod.Post, _tableName, content, "return=representation");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    // Supabase returns an array for POST requests even if only one item is returned
                    var createdItems = JsonSerializer.Deserialize<List<TModel>>(responseContent, _jsonSerializerOptions);
                    return SupabaseResult<TModel>.Success(createdItems?.Count > 0 ? createdItems[0] : null);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return SupabaseResult<TModel>.Failure($"Failed to create item. Status: {response.StatusCode}. Error: {error}");
                }
            }
            catch (Exception ex)
            {
                return SupabaseResult<TModel>.Failure($"An exception occurred during create: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing record in the Supabase table by its ID.
        /// </summary>
        /// <param name="id">The ID of the record to update.</param>
        /// <param name="item">The updated item data. Only provided fields will be updated.</param>
        /// <returns>A <see cref="SupabaseResult{TModel}"/> indicating success/failure and the updated data if successful.</returns>
        public async Task<SupabaseResult<TModel>> Update(object id, TModel item)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(item, _jsonSerializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await SendRequest(HttpMethod.Patch, $"{_tableName}?id=eq.{id}", content, "return=representation");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    // Supabase returns an array for PATCH requests even if only one item is returned
                    var updatedItems = JsonSerializer.Deserialize<List<TModel>>(responseContent, _jsonSerializerOptions);
                    return SupabaseResult<TModel>.Success(updatedItems?.Count > 0 ? updatedItems[0] : null);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return SupabaseResult<TModel>.Failure($"Failed to update item. Status: {response.StatusCode}. Error: {error}");
                }
            }
            catch (Exception ex)
            {
                return SupabaseResult<TModel>.Failure($"An exception occurred during update: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a record from the Supabase table by its ID.
        /// </summary>
        /// <param name="id">The ID of the record to delete.</param>
        /// <returns>A <see cref="SupabaseResult{TModel}"/> indicating success/failure and the deleted data if successful.</returns>
        public async Task<SupabaseResult<TModel>> Delete(object id)
        {
            try
            {
                var response = await SendRequest(HttpMethod.Delete, $"{_tableName}?id=eq.{id}", preferHeader: "return=representation");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    // Supabase returns an array for DELETE requests even if only one item is returned
                    var deletedItems = JsonSerializer.Deserialize<List<TModel>>(responseContent, _jsonSerializerOptions);
                    return SupabaseResult<TModel>.Success(deletedItems?.Count > 0 ? deletedItems[0] : null);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return SupabaseResult<TModel>.Failure($"Failed to delete item. Status: {response.StatusCode}. Error: {error}");
                }
            }
            catch (Exception ex)
            {
                return SupabaseResult<TModel>.Failure($"An exception occurred during delete: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all records from the Supabase table.
        /// </summary>
        /// <returns>A <see cref="SupabaseResult{List{TModel}}"/> indicating success/failure and the list of retrieved data if successful.</returns>
        public async Task<SupabaseResult<List<TModel>>> GetAll()
        {
            Console.WriteLine("SupabaseAPIService.GetAll called for table: " + _tableName); // For debugging purposes, can be removed in production
            try
            {
                var response = await SendRequest(HttpMethod.Get, _tableName,
                    preferHeader: "count=exact");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var items = JsonSerializer.Deserialize<List<TModel>>(responseContent, _jsonSerializerOptions);
                    return SupabaseResult<List<TModel>>.Success(items);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return SupabaseResult<List<TModel>>.Failure($"Failed to get all items. Status: {response.StatusCode}. Error: {error}");
                }
            }
            catch (Exception ex)
            {
                return SupabaseResult<List<TModel>>.Failure($"An exception occurred during GetAll: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a single record from the Supabase table by its ID.
        /// </summary>
        /// <param name="id">The ID of the record to retrieve.</param>
        /// <returns>A <see cref="SupabaseResult{TModel}"/> indicating success/failure and the retrieved data if successful.</returns>
        public async Task<SupabaseResult<TModel>> GetById(object id)
        {
            try
            {
                var response = await SendRequest(HttpMethod.Get, $"{_tableName}?id=eq.{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var items = JsonSerializer.Deserialize<List<TModel>>(responseContent, _jsonSerializerOptions);
                    return SupabaseResult<TModel>.Success(items?.Count > 0 ? items[0] : null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return SupabaseResult<TModel>.Failure($"Item with ID {id} not found.");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return SupabaseResult<TModel>.Failure($"Failed to get item by ID. Status: {response.StatusCode}. Error: {error}");
                }
            }
            catch (Exception ex)
            {
                return SupabaseResult<TModel>.Failure($"An exception occurred during GetById: {ex.Message}");
            }
        }

        /// <summary>
        /// Filters records by a specific column name and value.
        /// </summary>
        /// <param name="columnName">The name of the column to filter by.</param>
        /// <param name="value">The value to filter for.</param>
        /// <returns>A <see cref="SupabaseResult{List{TModel}}"/> indicating success/failure and the list of filtered data if successful.</returns>
        public async Task<SupabaseResult<List<TModel>>> Filter(string columnName, string value)
        {
            try
            {
                // Supabase filter format: {columnName}=eq.{value}
                var response = await SendRequest(HttpMethod.Get, $"{_tableName}?{columnName}=eq.{value}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var items = JsonSerializer.Deserialize<List<TModel>>(responseContent, _jsonSerializerOptions);
                    return SupabaseResult<List<TModel>>.Success(items);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return SupabaseResult<List<TModel>>.Failure($"Failed to filter items by {columnName}. Status: {response.StatusCode}. Error: {error}");
                }
            }
            catch (Exception ex)
            {
                return SupabaseResult<List<TModel>>.Failure($"An exception occurred during Filter: {ex.Message}");
            }
        }

        

        /// <summary>
        /// Performs a batch read (pagination) of records.
        /// </summary>
        /// <param name="offset">The starting index of the records to retrieve (0-based).</param>
        /// <param name="limit">The maximum number of records to retrieve.</param>
        /// <returns>A <see cref="SupabaseResult{List{TModel}}"/> indicating success/failure and the list of paginated data if successful.</returns>
        public async Task<SupabaseResult<List<TModel>>> BatchRead(int offset, int limit)
        {
            try
            {
                // Supabase uses 'Range' header for pagination
                string rangeHeader = $"{offset}-{offset + limit - 1}";
                var response = await SendRequest(HttpMethod.Get, _tableName, 
                    preferHeader: "count=exact", rangeHeader: rangeHeader);
                //var response = await SendRequest(HttpMethod.Get, _tableName, "your_table?select=*&head=true", null, "count=exact");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var items = JsonSerializer.Deserialize<List<TModel>>(responseContent, _jsonSerializerOptions);
                   
                    //DebugAllHeaders(response);
                    return SupabaseResult<List<TModel>>.Success(items, response.Content.Headers);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();                   
                    return SupabaseResult<List<TModel>>.Failure($"Failed to batch read items. Status: {response.StatusCode}. Error: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // For debugging purposes, can be removed in production
                return SupabaseResult<List<TModel>>.Failure($"An exception occurred during BatchRead: {ex.Message}");
            }
        }

        public void DebugAllHeaders(HttpResponseMessage response)
        {
            Console.WriteLine("=== RESPONSE HEADERS ===");
            foreach (var header in response.Headers)
            {
                Console.WriteLine($"Header: '{header.Key}' = '{string.Join(", ", header.Value)}'");
            }

            Console.WriteLine("=== CONTENT HEADERS ===");
            if (response.Content?.Headers != null)
            {
                foreach (var header in response.Content.Headers)
                {
                    Console.WriteLine($"Content Header: '{header.Key}' = '{string.Join(", ", header.Value)}'");
                }
            }

            Console.WriteLine($"Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Body: {response.Content.ReadAsStringAsync()}");
        }
    }
}
