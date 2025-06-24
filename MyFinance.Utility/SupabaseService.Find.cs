using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Utility;
public partial class SupabaseService
{
    /// <summary>
    /// Searches for entities by a name field using the Supabase JS interop.
    /// This assumes a 'Name' column in your Supabase table for the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to search for.</typeparam>
    /// <param name="tableName">The name of the Supabase table to search in.</param>
    /// <param name="name">The name or partial name to search for.</param>
    /// <returns>A list of entities of type TEntity matching the search criteria.</returns>
    public async Task<List<TEntity>> SearchEntitiesByNameAsync<TEntity>(string tableName, string name)
    {
        try
        {
            // Call the searchByName method from your supabaseInterop.js
            var result = await _jsRuntime.InvokeAsync<SupabaseSearchResult<List<TEntity>>>(
                "supabaseInterop.searchByName",
                tableName, // Dynamic table name
                name
            );

            if (result.Error != null)
            {
                Console.Error.WriteLine($"Error searching entities in '{tableName}': {result.Error.Message}");
                return new List<TEntity>();
            }

            return result.Data ?? new List<TEntity>();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception in SearchEntitiesByNameAsync for '{tableName}': {ex.Message}");
            return new List<TEntity>();
        }
    }

    /// <summary>
    /// Searches for a single entity by its ID using the Supabase JS interop.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to search for.</typeparam>
    /// <param name="tableName">The name of the Supabase table to search in.</param>
    /// <param name="id">The ID of the entity to search for.</param>
    /// <returns>The TEntity object if found, otherwise null.</returns>
    public async Task<TEntity?> SearchEntityByIdAsync<TEntity>(string tableName, Guid id)
    {
        try
        {
            var result = await _jsRuntime.InvokeAsync<SupabaseSearchResult<TEntity>>(
                "supabaseInterop.searchById",
                tableName, // Dynamic table name
                id.ToString()
            );

            if (result.Error != null)
            {
                Console.Error.WriteLine($"Error searching entity by ID in '{tableName}': {result.Error.Message}");
                return default;
            }

            return result.Data;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception in SearchEntityByIdAsync for '{tableName}': {ex.Message}");
            return default;
        }
    }

    // Helper class to deserialize the JS interop search results
    public class SupabaseSearchResult<T>
    {
        [System.Text.Json.Serialization.JsonPropertyName("data")]
        public T? Data { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("error")]
        public SupabaseError? Error { get; set; }
    }

    public class SupabaseError
    {
        [System.Text.Json.Serialization.JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}

