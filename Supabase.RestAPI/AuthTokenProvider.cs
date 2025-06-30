using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supabase.RestAPI;


/// <summary>
/// Defines a contract for a service that provides authentication tokens.
/// </summary>
public interface IAuthTokenProvider
{
    /// <summary>
    /// Asynchronously retrieves the current authentication token.
    /// Returns null or an empty string if no token is available (e.g., user not logged in).
    /// </summary>
    /// <returns>The authentication token string, or null/empty if not available.</returns>
    ValueTask<string> GetAuthTokenAsync();

    string GetAuthToken();

    /// <summary>
    /// Sets the authentication token. This method would typically be called after a successful login.
    /// </summary>
    /// <param name="token">The token to set.</param>
    void SetAuthToken(string token);
}

/// <summary>
/// A basic implementation of IAuthTokenProvider.
/// In a real application, this would fetch the token from persistent storage (e.g., Local Storage)
/// or from an authentication state provider.
/// </summary>
public class AuthTokenProvider : IAuthTokenProvider
{
    private string _currentToken;
    ILocalStorageService _localStorage;
    public AuthTokenProvider(ILocalStorageService localStorage)
    {
        // In a real app, you might try to load a token from local storage here
        // using JavaScript interop (IJSRuntime).
        // Example: _currentToken = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        _localStorage = localStorage;

    }

    /// <summary>
    /// Gets the current authentication token.
    /// </summary>
    /// <returns>The authentication token string, or null/empty if not available.</returns>
    public ValueTask<string> GetAuthTokenAsync()
    {
        _currentToken = _localStorage.GetItemAsync<string>("accessToken").Result.ToString();
        return ValueTask.FromResult(_currentToken);
    }
    public string GetAuthToken()
    {
        _currentToken = _localStorage.GetItemAsync<string>("accessToken").Result.ToString();
        return _currentToken;
    }

    /// <summary>
    /// Sets the authentication token. This method would typically be called after a successful login.
    /// </summary>
    /// <param name="token">The token to set.</param>
    public void SetAuthToken(string token)
    {
        _currentToken = token;
        _localStorage.SetItemAsync("accessToken", token); // Save to local storage
                                                          // In a real app, you would save this to local storage using JavaScript interop:
                                                          // JSRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
    }
}


