using LibraryManagement.Api.Common;
using LibraryManagement.Api.CustomResults;

namespace LibraryManagement.Api.Extensions;

public static class HttpResponseExtensions
{
    public static void AppendPaginationHeaders<T>(
        this HttpResponse response,
        PagedResult<T> pagedResult)
    {
        response.Headers.Append(ResponseHeaders.TotalCount, pagedResult.TotalCount.ToString());
        response.Headers.Append(ResponseHeaders.TotalPages, pagedResult.TotalPages.ToString());
    }
}
