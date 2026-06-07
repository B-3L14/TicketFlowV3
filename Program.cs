using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TicketFlow.Contexts.Auth.Application.UseCases;
using TicketFlow.Contexts.Auth.Domain.Ports;
using TicketFlow.Contexts.Auth.Infrastructure.Adapters;
using TicketFlow.Contexts.Auth.Infrastructure.Data;
using TicketFlow.Contexts.Auth.Infrastructure.Repositories;
using TicketFlow.Contexts.Sales.Application.UseCases;
using TicketFlow.Contexts.Sales.Domain.Ports;
using TicketFlow.Contexts.Sales.Infrastructure.Adapters;
using TicketFlow.Contexts.Sales.Infrastructure.Data;
using TicketFlow.Contexts.Sales.Infrastructure.Repositories;

namespace TicketFlow
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<SalesDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(connectionString));

            // ── Auth ──────────────────────────────────────────────────────────────
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITokenService, JwtTokenService>();
            builder.Services.AddScoped<RegisterUseCase>();
            builder.Services.AddScoped<LoginUseCase>();

            builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            // ── Sales — Repositórios
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

            // ── Sales — Gateway de pagamento ─────────────────────────────────────
            // SimulatedPaymentGateway implementa o Must Have de simulação.
            // Para usar o MercadoPagoAdapter real, substitua a linha abaixo.
            builder.Services.AddScoped<IPaymentGateway, SimulatedPaymentGateway>();

            // ── Sales — Use Cases ─────────────────────────────────────────────────
            builder.Services.AddScoped<CheckoutUseCase>();          // fluxo monolítico (legado)
            builder.Services.AddScoped<CreateOrderUseCase>();        // POST /orders
            builder.Services.AddScoped<AddItemUseCase>();            // POST /orders/{id}/items
            builder.Services.AddScoped<RemoveItemUseCase>();         // DELETE /orders/{id}/items/{itemId}
            builder.Services.AddScoped<UpdateOrderStatusUseCase>();  // PATCH /orders/{id}/status
            builder.Services.AddScoped<SimulatePaymentUseCase>();    // POST /payments/simulate
            builder.Services.AddScoped<GetPaymentUseCase>();         // GET /payments/{id}
            builder.Services.AddScoped<GetTicketByHashUseCase>();    // GET /tickets/{hash}
            builder.Services.AddScoped<OrderHistoryUseCase>();       // GET /orders/history
            builder.Services.AddScoped<InvalidateTicketUseCase>();   // PATCH /tickets/{id}/invalidate
            builder.Services.AddScoped<CreateReservationUseCase>();  // POST /reservations
            builder.Services.AddScoped<CancelReservationUseCase>();  // DELETE /reservations/{id}
            builder.Services.AddScoped<GetReservationUseCase>();     // GET /reservations/{id}

            // ── JWT ───────────────────────────────────────────────────────────────
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
