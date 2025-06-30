using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MyFinance;
using MyFinance.Utility;
using Supabase.RestAPI;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection; // Add this using directive at the top of the file
using System.Net.Http.Json;
using MyFinance.Utility.Helper;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<SupabaseService>();

#region Supabase Configuration
// --- Supabase Configuration ---
var supabaseUrl = "https://cqfkvyppzrwjrikyaqsv.supabase.co"; // Corrected URL format
// IMPORTANT: Replace with your actual Supabase API Key (Anon Public Key)
// It's safer to load this from configuration or environment variables in production.
var supabaseApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImNxZmt2eXBwenJ3anJpa3lhcXN2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQ2Mjc5ODUsImV4cCI6MjA2MDIwMzk4NX0.mGgQm5SEUtDvfP8d3itpGsJqIC-5-4g2Gfnivw0ALPc";

// Register HttpClient with its base address and default API key header.
// The Authorization Bearer token will be set dynamically per request.
builder.Services.AddHttpClient("SupabaseApi", client =>
{
    client.BaseAddress = new Uri($"{supabaseUrl}/rest/v1/");
    client.DefaultRequestHeaders.Add("apikey", supabaseApiKey);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    // DO NOT set Authorization header here, it will be handled dynamically by SupabaseApiService.
});


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();

// Register the authentication token provider service
// IAuthTokenProvider and AuthTokenProvider are now in the Supabase.RestAPI namespace
builder.Services.AddScoped<IAuthTokenProvider, AuthTokenProvider>();

// Register the generic SupabaseApiService
// The DI container will automatically resolve IHttpClientFactory and IAuthTokenProvider
builder.Services.AddScoped(typeof(SupabaseApiService<>));


#endregion


await builder.Build().RunAsync();


