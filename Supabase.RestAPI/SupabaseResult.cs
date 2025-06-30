using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Supabase.RestAPI;
/// <summary>
/// Represents a generic result from a Supabase API operation.
/// </summary>
/// <typeparam name="T">The type of the data returned by the operation.</typeparam>
public class SupabaseResult<T>
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// The data returned by the operation (e.g., created item, updated item, list of items).
    /// Null if the operation failed or no data was expected.
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// An error message if the operation failed. Null if the operation was successful.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="data">The data associated with the successful operation.</param>
    /// <returns>A new <see cref="SupabaseResult{T}"/> instance indicating success.</returns>
    public static SupabaseResult<T> Success(T data) => new SupabaseResult<T>
    {
        IsSuccess = true,
        Data = data,
    };
    public static SupabaseResult<T> Success(T data, HttpContentHeaders header) => new SupabaseResult<T>
    {
        IsSuccess = true,
        Data = data,
        ContextHeader = header // Store the response header if needed
    };

    public int GetTotalCountFromContentRange(HttpContentHeaders header = null)
    {
        int total = 0;
        try
        {
            if (ContextHeader == null)
            {
                ContextHeader = header;
            }

            // Try different case variations
            string[] headerNames = { "Content-Range", "content-range", "Content-range", "CONTENT-RANGE" };

            foreach (string headerName in headerNames)
            {
                if (ContextHeader.TryGetValues(headerName, out IEnumerable<string> values))
                {
                    var contentRange = values.FirstOrDefault();

                    if (!string.IsNullOrEmpty(contentRange))
                    {
                        var parts = contentRange.Split('/');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int totalCount))
                        {
                            total = totalCount;
                            break;
                        }
                    }
                }
            }

            if (total == 0)
            {
                Console.WriteLine("Content-Range header not found in any case variation.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            total = 0;
        }
        return total;
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message for the failed operation.</param>
    /// <returns>A new <see cref="SupabaseResult{T}"/> instance indicating failure.</returns>
    public static SupabaseResult<T> Failure(string errorMessage) => new SupabaseResult<T> { IsSuccess = false, ErrorMessage = errorMessage };

    public HttpContentHeaders ContextHeader { get; set; } // This property is not used in the current implementation, but can be used to store response headers if needed.
}
