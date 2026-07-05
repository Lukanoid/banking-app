using BankingApp.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using BankingApp.Api.Requests;
using BankingApp.Api.Responses;
using BankingApp.Api.Persistence;
using BankingApp.Api.Endpoints;

namespace BankingApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddSingleton<BankSystem>();
            builder.Services.AddSingleton<IBankStorage, JsonBankStorage>();

            var app = builder.Build();

            BankSystem bankSystem = app.Services.GetRequiredService<BankSystem>();
            IBankStorage storage = app.Services.GetRequiredService<IBankStorage>();

            List<BankAccount> savedAccounts = storage.LoadAccounts();
            bankSystem.LoadAccounts(savedAccounts);

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapGet("/", () => "Banking API is running");

            app.MapAccountEndpoints();

            app.Run();
        }
    }
}
