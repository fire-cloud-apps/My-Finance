using Microsoft.JSInterop;
using MudBlazor;
using MyFinance.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Utility;

public partial class SupabaseService
{
    /// <summary>
    /// Deletes a record from a specified Supabase table by its ID.
    /// </summary>
    /// <param name="tableName">The name of the Supabase table to delete from.</param>
    /// <param name="id">The ID of the record to delete.</param>
    /// <returns>A tuple indicating success and an error message if any.</returns>
    public async Task<(bool Success, string? ErrorMessage)> Delete(string tableName, object id)
    {
        try
        {
            // Check if the current session is authenticated
            if (CurrentSession == null || string.IsNullOrEmpty(CurrentSession.AccessToken))
            {
                return (false, "Not authenticated. Please log in to delete items.");
            }

            Console.WriteLine($"C#: Calling JS delete method for table '{tableName}' with ID: {id}");

            // Invoke the JavaScript interop method for deleting
            // The JS function 'delete' expects tableName and the id
            var jsResponse = await _jsRuntime.InvokeAsync<JsCrudResponse>("supabaseInterop.delete", tableName, id);

            Console.WriteLine($"C#: Delete from table '{tableName}' with ID '{id}' result. Success: {jsResponse.Success}, Error: {jsResponse.ErrorMessage ?? "None"}");

            if (!jsResponse.Success)
            {
                // Log and display error if the JS operation failed
                Console.Error.WriteLine($"C#: Error deleting from '{tableName}' from JS: {jsResponse.ErrorMessage}");
                _snackbar.Add($"Error deleting from '{tableName}': {jsResponse.ErrorMessage}", Severity.Error);
            }
            else
            {
                // Display success message
                _snackbar.Add($"Record from '{tableName}' with ID '{id}' deleted successfully!", Severity.Success);
            }
            return (jsResponse.Success, jsResponse.ErrorMessage);
        }
        catch (Exception ex)
        {
            // Catch and handle any unexpected exceptions during the process
            Console.Error.WriteLine($"C#: An unexpected error occurred while deleting from '{tableName}': {ex.Message}");
            _snackbar.Add($"An unexpected error occurred: {ex.Message}", Severity.Error);
            return (false, $"An unexpected error occurred while deleting from '{tableName}': {ex.Message}");
        }
    }
}
