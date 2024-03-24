using Microsoft.AspNetCore.Http.Extensions;

var builder = WebApplication.CreateBuilder(args);


var corsAllowedOrigins = builder.Configuration.GetValue<string>("CorsAllowedOrigins");
var circitApiBaseUrl = builder.Configuration.GetSection("ExternalApis").GetValue<string>("CircitApiBaseUrl");


builder.Services.AddCors(p =>
    p.AddPolicy("corsapp",
        builder => {
            builder.WithOrigins(corsAllowedOrigins.Split(";"))
            .AllowAnyMethod()
            .AllowAnyHeader();
        }
    ));


var app = builder.Build();

app.UseRouting();
app.UseCors("corsapp");

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("weather", async (context) =>
    {
        string pathAndQuery = context.Request.GetEncodedPathAndQuery();

        ResponseInfo responseInfo = await GetResponseInfo(circitApiBaseUrl, pathAndQuery);

        context.Response.StatusCode = responseInfo.StatusCode;
        await context.Response.WriteAsync(responseInfo.Result);
    });

    endpoints.MapGet("timezone", async (context) =>
    {
        string pathAndQuery = context.Request.GetEncodedPathAndQuery();

        ResponseInfo responseInfo = await GetResponseInfo(circitApiBaseUrl, pathAndQuery);

        context.Response.StatusCode = responseInfo.StatusCode;
        await context.Response.WriteAsync(responseInfo.Result);
    });

    endpoints.MapGet("astronomy", async (context) =>
    {
        string pathAndQuery = context.Request.GetEncodedPathAndQuery();

        ResponseInfo responseInfo = await GetResponseInfo(circitApiBaseUrl, pathAndQuery);

        context.Response.StatusCode = responseInfo.StatusCode;
        await context.Response.WriteAsync(responseInfo.Result);
    });
});

app.Run(async (HttpContext context) => {
    await context.Response.WriteAsync("Page Not Found");
});

app.Run();



static async Task<ResponseInfo> GetResponseInfo(string circitApiBaseUrl, string pathAndQuery)
{
    ResponseInfo responseInfo = new ResponseInfo();

    using (var httpClient = new HttpClient())
    {
        var response = await httpClient.GetAsync(circitApiBaseUrl + pathAndQuery);
        var result = await response.Content.ReadAsStringAsync();

        responseInfo.StatusCode = (int)response.StatusCode;
        responseInfo.Result = result;
    }

    return responseInfo;
}

class ResponseInfo
{
    public int StatusCode { get; set; }

    public string Result { get; set; }
}