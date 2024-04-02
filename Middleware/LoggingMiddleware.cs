public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;

    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 讀取請求內容
        context.Request.EnableBuffering();
        var request = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;
        _logger.LogInformation($"Request body: {request}");

        // 呼叫下一個中間件並捕獲回應


        await using var responseBody = new MemoryStream();
        var originalBodyStream = context.Response.Body;
        context.Response.Body = responseBody;
        await _next(context);

        // 讀取回應內容
        responseBody.Seek(0, SeekOrigin.Begin);
        var response = await new StreamReader(responseBody).ReadToEndAsync();
        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);

        _logger.LogInformation($"Response body: {response}");
    }
}
