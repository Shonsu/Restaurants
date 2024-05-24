using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares.Tests;

public class ErrorHandlingMiddlewareTests
{
    [Fact]
    public async void InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        // arrange
        var httpContext = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middlewareHandler = new ErrorHandlingMiddleware(loggerMock.Object);
        var nextDelegateMock = new Mock<RequestDelegate>();
        // act
        await middlewareHandler.InvokeAsync(httpContext, nextDelegateMock.Object);

        // assert
        nextDelegateMock.Verify(next => next.Invoke(httpContext), Times.Once);
    }

    [Fact]
    public async void InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
    {
        // arrange
        var httpContext = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middlewareHandler = new ErrorHandlingMiddleware(loggerMock.Object);
        var notFoundException = new NotFoundException(nameof(Restaurant), "1");

        // act
        await middlewareHandler.InvokeAsync(httpContext, _ => throw notFoundException);

        // assert
        httpContext.Response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async void InvokeAsync_WhenForbiddenExceptionThrown_ShouldSetStatusCode403()
    {
        // arrange
        var httpContext = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middlewareHandler = new ErrorHandlingMiddleware(loggerMock.Object);
        var forbiddenException = new ForbiddenException("ForbiddenException");

        // act
        await middlewareHandler.InvokeAsync(httpContext, _ => throw forbiddenException);

        // assert
        httpContext.Response.StatusCode.Should().Be(403);
    }

    [Fact]
    public async void InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCode500()
    {
        // arrange
        var httpContext = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middlewareHandler = new ErrorHandlingMiddleware(loggerMock.Object);
        var exception = new Exception();

        // act
        await middlewareHandler.InvokeAsync(httpContext, _ => throw exception);

        // assert
        httpContext.Response.StatusCode.Should().Be(500);
    }
}
