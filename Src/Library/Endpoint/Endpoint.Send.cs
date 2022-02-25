﻿using Microsoft.AspNetCore.Http;

namespace FastEndpoints;

public abstract partial class Endpoint<TRequest, TResponse> : BaseEndpoint where TRequest : class, new() where TResponse : class, new()
{
    /// <summary>
    /// send the supplied response dto serialized as json to the client.
    /// </summary>
    /// <param name="response">the object to serialize to json</param>
    /// <param name="statusCode">optional custom http status code</param>
    /// <param name="cancellation">optional cancellation token</param>
    protected Task SendAsync(TResponse response, int statusCode = 200, CancellationToken cancellation = default)
    {
        Response = response;
        return HttpContext.Response.SendAsync(response, statusCode, Configuration.SerializerContext, cancellation);
    }

    /// <summary>
    /// send a 201 created response with a location header containing where the resource can be retrieved from.
    /// <para>HINT: if pointing to an endpoint with multiple verbs, make sure to specify the 'verb' argument and if pointing to a multi route endpoint, specify the 'routeNumber' argument.</para>
    /// </summary>
    /// <typeparam name="TEndpoint">the type of the endpoint where the resource can be retrieved from</typeparam>
    /// <param name="routeValues">a route values object with key/value pairs of route information</param>
    /// <param name="responseBody">the content to be serialized in the response body</param>
    /// <param name="verb">only useful when pointing to a multi verb endpoint</param>
    /// <param name="routeNumber">only useful when pointing to a multi route endpoint</param>
    /// <param name="cancellation">optional cancellation token</param>
    protected Task SendCreatedAtAsync<TEndpoint>(object? routeValues, TResponse? responseBody, Http? verb = null, int? routeNumber = null, CancellationToken cancellation = default) where TEndpoint : IEndpoint
    {
        if (responseBody is not null)
            Response = responseBody;

        return HttpContext.Response.SendCreatedAtAsync<TEndpoint>(routeValues, responseBody, verb, routeNumber, Configuration.SerializerContext, cancellation);
    }

    /// <summary>
    /// send a 201 created response with a location header containing where the resource can be retrieved from.
    /// </summary>
    /// <param name="endpointName">the name of the endpoint to use for link generation (openapi route id)</param>
    /// <param name="routeValues">a route values object with key/value pairs of route information</param>
    /// <param name="responseBody">the content to be serialized in the response body</param>
    /// <param name="cancellation">cancellation token</param>
    protected Task SendCreatedAtAsync(string endpointName, object? routeValues, TResponse? responseBody, CancellationToken cancellation = default)
    {
        if (responseBody is not null)
            Response = responseBody;

        return HttpContext.Response.SendCreatedAtAsync(endpointName, routeValues, responseBody, Configuration.SerializerContext, cancellation);
    }

    /// <summary>
    /// send the supplied string content to the client.
    /// </summary>
    /// <param name="content">the string to write to the response body</param>
    /// <param name="statusCode">optional custom http status code</param>
    /// <param name="cancellation">optional cancellation token</param>
    protected Task SendStringAsync(string content, int statusCode = 200, CancellationToken cancellation = default)
    {
        return HttpContext.Response.SendStringAsync(content, statusCode, cancellation);
    }

    /// <summary>
    /// send an http 200 ok response without any body
    /// </summary>
    /// <param name="cancellation">optional cancellation token</param>
    protected Task SendOkAsync(CancellationToken cancellation = default)
    {
        return HttpContext.Response.SendOkAsync(cancellation);
    }

    /// <summary>
    /// send a 400 bad request with error details of the current validation failures
    /// </summary>
    /// <param name="cancellation"></param>
    protected Task SendErrorsAsync(CancellationToken cancellation = default)
    {
        return HttpContext.Response.SendErrorsAsync(ValidationFailures, Configuration.SerializerContext, cancellation);
    }

    /// <summary>
    /// send a 204 no content response
    /// </summary>
    /// <param name="cancellation">optional cancellation token</param>
    protected Task SendNoContentAsync(CancellationToken cancellation = default)
    {
        return HttpContext.Response.SendNoContentAsync(cancellation);
    }

    /// <summary>
    /// send a 404 not found response
    /// </summary>
    /// <param name="cancellation">optional cancellation token</param>
    protected Task SendNotFoundAsync(CancellationToken cancellation = default)
    {
        return HttpContext.Response.SendNotFoundAsync(cancellation);
    }

    /// <summary>
    /// send a 401 unauthorized response
    /// </summary>
    /// <param name="cancellation">optional cancellation token</param>
    protected Task SendUnauthorizedAsync(CancellationToken cancellation = default)
    {
        return HttpContext.Response.SendUnauthorizedAsync(cancellation);
    }

    /// <summary>
    /// send a 403 unauthorized response
    /// </summary>
    /// <param name="cancellation">optional cancellation token</param>
    protected Task SendForbiddenAsync(CancellationToken cancellation = default)
    {
        return HttpContext.Response.SendForbiddenAsync(cancellation);
    }

    /// <summary>
    /// send a byte array to the client
    /// </summary>
    /// <param name="bytes">the bytes to send</param>
    /// <param name="contentType">optional content type to set on the http response</param>
    /// <param name="lastModified">optional last modified date-time-offset for the data stream</param>
    /// <param name="enableRangeProcessing">optional switch for enabling range processing</param>
    /// <param name="cancellation">optional cancellation token</param>
    protected Task SendBytesAsync(byte[] bytes, string? fileName = null, string contentType = "application/octet-stream",
        DateTimeOffset? lastModified = null, bool enableRangeProcessing = false, CancellationToken cancellation = default)
    {
        return HttpContext.Response.SendBytesAsync(bytes, fileName, contentType, lastModified, enableRangeProcessing, cancellation);
    }

    /// <summary>
    /// send a file to the client
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <param name="contentType">optional content type to set on the http response</param>
    /// <param name="lastModified">optional last modified date-time-offset for the data stream</param>
    /// <param name="enableRangeProcessing">optional switch for enabling range processing</param>
    /// <param name="cancellation">optional cancellation token</param>
    protected Task SendFileAsync(FileInfo fileInfo, string contentType = "application/octet-stream", DateTimeOffset? lastModified = null,
        bool enableRangeProcessing = false, CancellationToken cancellation = default)
    {
        return HttpContext.Response.SendFileAsync(fileInfo, contentType, lastModified, enableRangeProcessing, cancellation);
    }

    /// <summary>
    /// send the contents of a stream to the client
    /// </summary>
    /// <param name="stream">the stream to read the data from</param>
    /// <param name="fileName">and optional file name to set in the content-disposition header</param>
    /// <param name="fileLengthBytes">optional total size of the file/stream</param>
    /// <param name="contentType">optional content type to set on the http response</param>
    /// <param name="lastModified">optional last modified date-time-offset for the data stream</param>
    /// <param name="enableRangeProcessing">optional switch for enabling range processing</param>
    /// <param name="cancellation">optional cancellation token</param>
    protected Task SendStreamAsync(Stream stream, string? fileName = null, long? fileLengthBytes = null,
        string contentType = "application/octet-stream", DateTimeOffset? lastModified = null, bool enableRangeProcessing = false,
        CancellationToken cancellation = default)
    {
        return HttpContext.Response.SendStreamAsync(stream, fileName, fileLengthBytes, contentType, lastModified, enableRangeProcessing, cancellation);
    }

    /// <summary>
    /// send an empty json object in the body
    /// </summary>
    /// <param name="cancellation">optional cancellation token</param>
    protected Task SendEmptyJsonObject(CancellationToken cancellation = default)
    {
        return HttpContext.Response.SendEmptyJsonObject(null, cancellation);
    }
}