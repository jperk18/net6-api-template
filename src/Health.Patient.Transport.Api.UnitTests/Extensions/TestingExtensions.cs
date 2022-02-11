using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Health.Patient.Transport.Api.UnitTests.Extensions;

public static class TestingExtensions
{
    public static T AssertResponseFromIActionResult<T>(int statusCode, IActionResult? result)
    {
        Assert.NotNull(result);
        var objResult = result as ObjectResult;
        Assert.NotNull(objResult);
        Assert.Equal(statusCode, objResult.StatusCode);
        var response = (T) objResult?.Value!;
        Assert.NotNull(response);
        return response;
    }

    public static void TestHandler(Func<Task> action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            Assert.True(false, e.Message);
        }
    }
}