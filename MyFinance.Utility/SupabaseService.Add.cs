using Microsoft.JSInterop;
using MudBlazor;
using MyFinance.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json; // Added for JsonSerializer
using System.Threading.Tasks;

namespace MyFinance.Utility;
public partial class SupabaseService
{
    public async Task<(bool Success, string? ErrorMessage)> Add<TEntity>(string tableName, TEntity entity) where TEntity : class
    {
        try
        {
            // Check if the current session is authenticated
            if (CurrentSession == null || string.IsNullOrEmpty(CurrentSession.AccessToken))
            {
                return (false, "Not authenticated. Please log in to add items.");
            }

            // Get the type name of the entity being added for logging purposes
            var entityTypeName = typeof(TEntity).Name;

            // --- DEBUG LOGGING ---
            Console.WriteLine($"C#: Calling JS insert method for {entityTypeName}. Entity data: {JsonSerializer.Serialize(entity, new JsonSerializerOptions { WriteIndented = true })}");
            // ---------------------

            // Invoke the JavaScript interop method, passing the generic entity
            // We assume 'supabaseInterop.insert' is a generic method on the JS side
            // that can handle various entity types or that the JS side will infer the table
            // name from the entity type itself (e.g., based on TEntity's name or a property).
            // You might need to adjust the JS interop method name or pass a table name if needed.
            var jsResponse = await _jsRuntime.InvokeAsync<JsCrudResponse>("supabaseInterop.insert", tableName, entity);

            Console.WriteLine($"C#: Insert {entityTypeName} result. Success: {jsResponse.Success}, Error: {jsResponse.ErrorMessage ?? "None"}");

            if (!jsResponse.Success)
            {
                // Log and display error if the JS operation failed
                Console.Error.WriteLine($"C#: Error adding {entityTypeName} from JS: {jsResponse.ErrorMessage}");
                _snackbar.Add($"Error adding {entityTypeName}: {jsResponse.ErrorMessage}", Severity.Error);
            }
            else
            {
                // Display success message
                _snackbar.Add($"{entityTypeName} added successfully!", Severity.Success);
            }
            return (jsResponse.Success, jsResponse.ErrorMessage);
        }
        catch (Exception ex)
        {
            // Catch and handle any unexpected exceptions during the process
            var entityTypeName = typeof(TEntity).Name;
            Console.Error.WriteLine($"C#: An unexpected error occurred while adding {entityTypeName}: {ex.Message}");
            _snackbar.Add($"An unexpected error occurred: {ex.Message}", Severity.Error);
            return (false, $"An unexpected error occurred while adding {entityTypeName}: {ex.Message}");
        }
    }
}
