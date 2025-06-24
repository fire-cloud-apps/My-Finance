//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;
//using MudBlazor;
//using System;
//using System.Collections.Generic;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;


//namespace MyFinance.Services
//{

//    public class JsUserMetadata
//    {
//        [JsonPropertyName("avatar_url")]
//        public string? AvatarUrl { get; set; }
//        [JsonPropertyName("full_name")]
//        public string? FullName { get; set; }
//    }

//    public class JsAppMetadata
//    {
//        [JsonPropertyName("provider")]
//        public string? Provider { get; set; }
//    }

//    public class JsUser
//    {
//        [JsonPropertyName("id")]
//        public string? Id { get; set; }
//        [JsonPropertyName("email")]
//        public string? Email { get; set; }
//        [JsonPropertyName("created_at")]
//        public DateTime? CreatedAt { get; set; }
//        [JsonPropertyName("last_sign_in_at")]
//        public DateTime? LastSignInAt { get; set; }
//        [JsonPropertyName("user_metadata")]
//        public JsonElement UserMetadata { get; set; }
//        [JsonPropertyName("app_metadata")]
//        public JsonElement AppMetadata { get; set; }
//    }

//    public class JsSession
//    {
//        [JsonPropertyName("access_token")]
//        public string? AccessToken { get; set; }
//        [JsonPropertyName("refresh_token")]
//        public string? RefreshToken { get; set; }
//        [JsonPropertyName("expires_in")]
//        public int? ExpiresIn { get; set; }
//        [JsonPropertyName("expires_at")]
//        public long? ExpiresAt { get; set; }
//        [JsonPropertyName("token_type")]
//        public string? TokenType { get; set; }
//        [JsonPropertyName("user")]
//        public JsUser? User { get; set; }

//        public DateTime? GetExpiresAtDateTime()
//        {
//            if (ExpiresAt.HasValue)
//            {
//                return DateTimeOffset.FromUnixTimeSeconds(ExpiresAt.Value).LocalDateTime;
//            }
//            return null;
//        }

//        public string? GetUserMetadataString(string key)
//        {
//            if (User != null && User.UserMetadata.ValueKind == JsonValueKind.Object && User.UserMetadata.TryGetProperty(key, out var prop) && prop.ValueKind == JsonValueKind.String)
//            {
//                return prop.GetString();
//            }
//            return null;
//        }

//        public string? GetAppMetadataString(string key)
//        {
//            if (User != null && User.AppMetadata.ValueKind == JsonValueKind.Object && User.AppMetadata.TryGetProperty(key, out var prop) && prop.ValueKind == JsonValueKind.String)
//            {
//                return prop.GetString();
//            }
//            return null;
//        }
//    }

//    //public class JsTasksResponse
//    //{
//    //    [JsonPropertyName("data")]
//    //    public List<TaskItem>? Data { get; set; }
//    //    [JsonPropertyName("count")]
//    //    public int Count { get; set; }
//    //    [JsonPropertyName("error")]
//    //    public JsError? Error { get; set; }
//    //}

//    public class JsError
//    {
//        [JsonPropertyName("message")]
//        public string? Message { get; set; }
//    }

//    public class JsCrudResponse
//    {
//        [JsonPropertyName("success")]
//        public bool Success { get; set; }
//        [JsonPropertyName("errorMessage")]
//        public string? ErrorMessage { get; set; }
//    }

//    public class SupabaseService : IAsyncDisposable
//    {
//        private readonly IJSRuntime _jsRuntime;
//        private readonly ISnackbar _snackbar;
//        private readonly IDialogService _dialogService;
//        private readonly NavigationManager _navigationManager;
//        private DotNetObjectReference<SupabaseService>? _dotNetRef;

//        public event Action<JsSession?>? OnAuthStateChanged;
//        public JsSession? CurrentSession { get; private set; }

//        public SupabaseService(IJSRuntime jsRuntime, ISnackbar snackbar, IDialogService dialogService, NavigationManager navigationManager)
//        {
//            _jsRuntime = jsRuntime;
//            _snackbar = snackbar;
//            _dialogService = dialogService;
//            _navigationManager = navigationManager;
//        }

//        public async Task InitializeAsync()
//        {
//            _dotNetRef = DotNetObjectReference.Create(this);
//            await _jsRuntime.InvokeVoidAsync("supabaseInterop.setServiceReference", _dotNetRef);
//        }

//        [JSInvokable]
//        public void OnJsAuthStateChanged(JsSession? session)
//        {
//            Console.WriteLine($"C#: Auth state changed received from JS. Session present: {session != null}");
//            CurrentSession = session;
//            OnAuthStateChanged?.Invoke(session);

//            if (session?.User?.Id != null)
//            {
//                _snackbar.Add("Logged in successfully!", Severity.Success);
//                _navigationManager.NavigateTo("/");
//            }
//            else
//            {
//                _snackbar.Add("Logged out successfully!", Severity.Info);
//                _navigationManager.NavigateTo("/login");
//            }
//        }

//        [JSInvokable]
//        public void HandleJsError(string errorMessage)
//        {
//            _snackbar.Add($"JS Error: {errorMessage}", Severity.Error);
//            Console.Error.WriteLine($"Error from JS Interop: {errorMessage}");
//        }

//        public async Task SignInWithGoogle()
//        {
//            _snackbar.Add("Initiating Google sign-in...", Severity.Info);
//            var redirectUrl = _navigationManager.ToAbsoluteUri("/").ToString();
//            await _jsRuntime.InvokeVoidAsync("supabaseInterop.signInWithGoogleRedirect", redirectUrl);
//        }

//        public async Task SignOut()
//        {
//            _snackbar.Add("Signing out...", Severity.Info);
//            await _jsRuntime.InvokeVoidAsync("supabaseInterop.signOut");
//        }

//        public async Task RefreshSession()
//        {
//            _snackbar.Add("Refreshing session...", Severity.Info);
//            await _jsRuntime.InvokeVoidAsync("supabaseInterop.getInitialSession");
//        }

//        /*
//        public async Task<(List<TaskItem>? Tasks, int Count, string? ErrorMessage)> GetTasks(int skip, int take)
//        {
//            try
//            {
//                if (CurrentSession == null || string.IsNullOrEmpty(CurrentSession.AccessToken))
//                {
//                    return (null, 0, "Not authenticated. Please log in.");
//                }

//                Console.WriteLine($"C#: Calling JS fetchTasks with skip={skip}, take={take}");
//                var jsResponse = await _jsRuntime.InvokeAsync<JsTasksResponse>("supabaseInterop.fetchTasks", skip, take);
//                Console.WriteLine($"C#: Received JsTasksResponse. Data count: {jsResponse.Data?.Count ?? 0}, Total count: {jsResponse.Count}, Error: {jsResponse.Error?.Message ?? "None"}");


//                if (jsResponse.Error != null)
//                {
//                    Console.Error.WriteLine($"C#: Error fetching tasks from JS: {jsResponse.Error.Message}");
//                    return (null, 0, $"Failed to fetch tasks: {jsResponse.Error.Message}");
//                }
//                else
//                {
//                    if (jsResponse.Data == null)
//                    {
//                        Console.WriteLine("C#: jsResponse.Data is null, returning empty list.");
//                    }
//                    return (jsResponse.Data ?? new List<TaskItem>(), jsResponse.Count, null);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine($"C#: An unexpected error occurred while fetching tasks: {ex.Message}");
//                return (null, 0, $"An unexpected error occurred while fetching tasks: {ex.Message}");
//            }
//        }

//        public async Task<(bool Success, string? ErrorMessage)> AddTask(TaskItem task)
//        {
//            try
//            {
//                if (CurrentSession == null || string.IsNullOrEmpty(CurrentSession.AccessToken))
//                {
//                    return (false, "Not authenticated. Please log in to add tasks.");
//                }

//                Console.WriteLine($"C#: Calling JS insertTask for task: {task.Name}");
//                var jsResponse = await _jsRuntime.InvokeAsync<JsCrudResponse>("supabaseInterop.insertTask", task);
//                Console.WriteLine($"C#: InsertTask result. Success: {jsResponse.Success}, Error: {jsResponse.ErrorMessage ?? "None"}");

//                if (!jsResponse.Success)
//                {
//                    Console.Error.WriteLine($"C#: Error adding task from JS: {jsResponse.ErrorMessage}");
//                    _snackbar.Add($"Error adding task: {jsResponse.ErrorMessage}", Severity.Error);
//                }
//                else
//                {
//                    _snackbar.Add("Task added successfully!", Severity.Success);
//                }
//                return (jsResponse.Success, jsResponse.ErrorMessage);
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine($"C#: An unexpected error occurred while adding task: {ex.Message}");
//                _snackbar.Add($"An unexpected error occurred: {ex.Message}", Severity.Error);
//                return (false, $"An unexpected error occurred while adding task: {ex.Message}");
//            }
//        }

//        public async Task<(bool Success, string? ErrorMessage)> UpdateTask(TaskItem task)
//        {
//            try
//            {
//                if (CurrentSession == null || string.IsNullOrEmpty(CurrentSession.AccessToken))
//                {
//                    return (false, "Not authenticated. Please log in to update tasks.");
//                }

//                Console.WriteLine($"C#: Calling JS updateTask for task ID: {task.Id}");
//                var jsResponse = await _jsRuntime.InvokeAsync<JsCrudResponse>("supabaseInterop.updateTask", task);
//                Console.WriteLine($"C#: UpdateTask result. Success: {jsResponse.Success}, Error: {jsResponse.ErrorMessage ?? "None"}");

//                if (!jsResponse.Success)
//                {
//                    Console.Error.WriteLine($"C#: Error updating task from JS: {jsResponse.ErrorMessage}");
//                    _snackbar.Add($"Error updating task: {jsResponse.ErrorMessage}", Severity.Error);
//                }
//                else
//                {
//                    _snackbar.Add("Task updated successfully!", Severity.Success);
//                }
//                return (jsResponse.Success, jsResponse.ErrorMessage);
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine($"C#: An unexpected error occurred while updating task: {ex.Message}");
//                _snackbar.Add($"An unexpected error occurred: {ex.Message}", Severity.Error);
//                return (false, $"An unexpected error occurred while updating task: {ex.Message}");
//            }
//        }

//        public async Task<(bool Success, string? ErrorMessage)> DeleteTask(int id) // Changed type to int
//        {
//            try
//            {
//                if (CurrentSession == null || string.IsNullOrEmpty(CurrentSession.AccessToken))
//                {
//                    return (false, "Not authenticated. Please log in to delete tasks.");
//                }

//                var parameters = new DialogParameters
//                {
//                    ["ContentText"] = $"Do you really want to delete task {id}?",
//                    ["ButtonText"] = "Delete"
//                };
//                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
//                var dialog = await _dialogService.ShowAsync<DeleteDialog>("Confirm Deletion", parameters, options);
//                var result = dialog.Result;

//                if (!result.IsCanceled)
//                {
//                    var jsResponse = await _jsRuntime.InvokeAsync<JsCrudResponse>("supabaseInterop.deleteTask", id); // Pass ID as int directly

//                    if (!jsResponse.Success)
//                    {
//                        Console.Error.WriteLine($"Error deleting task from JS: {jsResponse.ErrorMessage}");
//                        _snackbar.Add($"Error deleting task: {jsResponse.ErrorMessage}", Severity.Error);
//                    }
//                    else
//                    {
//                        _snackbar.Add("Task deleted successfully!", Severity.Success);
//                    }
//                    return (jsResponse.Success, jsResponse.ErrorMessage);
//                }
//                else
//                {
//                    _snackbar.Add("Task deletion cancelled.", Severity.Info);
//                    return (false, "Deletion cancelled by user.");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine($"An unexpected error occurred while deleting task: {ex.Message}");
//                _snackbar.Add($"An unexpected error occurred: {ex.Message}", Severity.Error);
//                return (false, $"An unexpected error occurred while deleting task: {ex.Message}");
//            }
//        }

//        */
//        public async Task<string> GetLocalStorageTokenJson()
//        {
//            var projectRef = await _jsRuntime.InvokeAsync<string>("eval", "window.supabaseJSClient.supabaseUrl.split('//')[1].split('.')[0]");
//            if (string.IsNullOrEmpty(projectRef))
//            {
//                Console.WriteLine("C#: Could not dynamically determine Supabase project reference from JS client URL. Using default.");
//                projectRef = "default-project-id";
//            }

//            var storedSessionJson = await _jsRuntime.InvokeAsync<string>("supabaseInterop.getLocalStorageToken", projectRef);
//            return storedSessionJson ?? "No Supabase session found in local storage.";
//        }

//        public async ValueTask DisposeAsync()
//        {
//            if (_dotNetRef != null)
//            {
//                _dotNetRef.Dispose();
//            }
//        }
//    }
//}
