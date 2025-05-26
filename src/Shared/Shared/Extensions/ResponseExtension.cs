using EShop.Shared.Domain;
using Microsoft.AspNetCore.Http;

namespace EShop.Shared.Extensions;

public static class ResponseExtension
{
    public static IResult ToApiResponse<T>(this Response<T> result, Func<T, IResult> onSuccess)
    {
        return result.IsSuccess
            ? onSuccess(result.Value!)
            : Results.BadRequest(new { error = result.Error });
    }
}