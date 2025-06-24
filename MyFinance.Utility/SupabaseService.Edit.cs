using Microsoft.JSInterop;
using MudBlazor;
using MyFinance.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection; // Required for PropertyInfo

namespace MyFinance.Utility;

public partial class SupabaseService
{
    /// <summary>
    /// Edits an existing record in a specified Supabase table.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to edit.</typeparam>
    /// <param name="tableName">The name of the Supabase table to update.</param>
    /// <param name="entity">The entity object containing the updated data. It must have an 'Id' property.</param>
    /// <returns>A tuple indicating success and an error message if any.</returns>
    public async Task<(bool Success, string? ErrorMessage)> Edit<TEntity>(string tableName, TEntity entity) where TEntity : class
    {
        try
        {
            // Check if the current session is authenticated
            if (CurrentSession == null || string.IsNullOrEmpty(CurrentSession.AccessToken))
            {
                return (false, "Not authenticated. Please log in to edit items.");
            }

            var entityTypeName = typeof(TEntity).Name;
            Console.WriteLine($"C#: Calling JS update method for {entityTypeName}: {entity}");

            // Use reflection to get the 'Id' property value from the entity
            PropertyInfo? idProperty = typeof(TEntity).GetProperty("Id");
            if (idProperty == null)
            {
                var errorMessage = $"Entity type '{entityTypeName}' does not have an 'Id' property for updating.";
                Console.Error.WriteLine($"C#: {errorMessage}");
                _snackbar.Add(errorMessage, Severity.Error);
                return (false, errorMessage);
            }

            var id = idProperty.GetValue(entity);
            if (id == null)
            {
                var errorMessage = $"The 'Id' property of entity type '{entityTypeName}' is null. Cannot update a record without an ID.";
                Console.Error.WriteLine($"C#: {errorMessage}");
                _snackbar.Add(errorMessage, Severity.Error);
                return (false, errorMessage);
            }

            // Invoke the JavaScript interop method for updating
            // The JS function 'update' expects tableName, id, and the updates (entity itself)
            var jsResponse = await _jsRuntime.InvokeAsync<JsCrudResponse>("supabaseInterop.update", tableName, id, entity);

            Console.WriteLine($"C#: Update {entityTypeName} result. Success: {jsResponse.Success}, Error: {jsResponse.ErrorMessage ?? "None"}");

            if (!jsResponse.Success)
            {
                // Log and display error if the JS operation failed
                Console.Error.WriteLine($"C#: Error updating {entityTypeName} from JS: {jsResponse.ErrorMessage}");
                _snackbar.Add($"Error updating {entityTypeName}: {jsResponse.ErrorMessage}", Severity.Error);
            }
            else
            {
                // Display success message
                _snackbar.Add($"{entityTypeName} updated successfully!", Severity.Success);
            }
            return (jsResponse.Success, jsResponse.ErrorMessage);
        }
        catch (Exception ex)
        {
            // Catch and handle any unexpected exceptions during the process
            var entityTypeName = typeof(TEntity).Name;
            Console.Error.WriteLine($"C#: An unexpected error occurred while updating {entityTypeName}: {ex.Message}");
            _snackbar.Add($"An unexpected error occurred: {ex.Message}", Severity.Error);
            return (false, $"An unexpected error occurred while updating {entityTypeName}: {ex.Message}");
        }
    }
}
