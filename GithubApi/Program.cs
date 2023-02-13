using System.Net.Http.Headers;
using System.Reflection;
using AutoMapper;
using GithubApi.Controllers;
using GithubApi.Services;
using GithubApi.Services.Clients.Github;
using MediatR;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

//Adding the HttpClient to call Github api and also retry policy. 
builder.Services.AddHttpClient<IUserService, GithubUserService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("GithubApiConfiguration:Url") ?? throw new Exception("Unable to find Github configuration"));
    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", builder.Configuration.GetValue<string>("GithubApiConfiguration:Token"));
    c.DefaultRequestHeaders.Add("User-Agent","request");
}).AddPolicyHandler(GetRetryPolicy());

//We add a retry policy for transient errors also can be extend to add circuit breaker etc. 
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() => HttpPolicyExtensions
    .HandleTransientHttpError()
    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.InternalServerError)
    .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //We can log request/response HttpContext here only in dev.
    app.UseHttpLogging();
}

app.UseHttpsRedirection();

app.UseAuthorization();
    
app.MapControllers();

app.Run();