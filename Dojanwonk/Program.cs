using BLL;
using DAL;
using DAL.Modelos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================
//   CONFIG TESTING
// ==========================
var isTesting = builder.Environment.IsEnvironment("Testing");

// ==========================
//   BASE DE DATOS
// ==========================
if (!isTesting)
{
    builder.Services.AddDbContext<DbDojankwonContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB")));
}

// ==========================
//   JWT
// ==========================
var jwtKey = builder.Configuration["Jwt:SecretKey"]
    ?? throw new InvalidOperationException("JWT SecretKey no configurada");
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// ==========================
//   AUTORIZACIÓN POR ROLES
// ==========================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdmin", p => p.RequireRole("Administrador"));
    options.AddPolicy("AdminOInstructor", p => p.RequireRole("Administrador", "Instructor"));
    options.AddPolicy("AdminORecepcion", p => p.RequireRole("Administrador", "Recepcionista"));
    options.AddPolicy("TodoStaff", p => p.RequireRole("Administrador", "Instructor", "Recepcionista"));
    options.AddPolicy("SoloInstructor", p => p.RequireRole("Instructor"));
    options.AddPolicy("SoloRecepcion", p => p.RequireRole("Recepcionista"));
});

// ==========================
//   CORS
// ==========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ==========================
//   INYECCIÓN DE DEPENDENCIAS
//   REPOSITORIOS (DAL)
// ==========================

builder.Services.AddScoped<IDBUsuario, DBUsuario>();
builder.Services.AddScoped<IDBArticulo, DBArticulo>();
builder.Services.AddScoped<IDBPrestamo, DBPrestamo>();
builder.Services.AddScoped<IDBEstudiante, DBEstudiante>();
builder.Services.AddScoped<IDBGrupo, DBGrupo>();
builder.Services.AddScoped<IDBRango, DBRango>();
builder.Services.AddScoped<IDBPago, DBPago>();
builder.Services.AddScoped<IDBExamen, DBExamen>();

// ==========================
//   SERVICIOS (BLL)
// ==========================
builder.Services.AddScoped<IServiceUsuario, ServiceUsuario>();
builder.Services.AddScoped<IServiceArticulo, ServiceArticulo>();
builder.Services.AddScoped<IServicePrestamo, ServicePrestamo>();
builder.Services.AddScoped<IServiceEstudiante, ServiceEstudiante>();
builder.Services.AddScoped<IServiceRango, ServiceRango>();
builder.Services.AddScoped<IServicePago, ServicePago>();
builder.Services.AddScoped<IServiceExamen, ServiceExamen>();

// ==========================
//   SERVICIOS AUXILIARES
// ==========================
builder.Services.AddScoped<NotificacionesCorreo>();
builder.Services.AddScoped<TokenService>();

// ==========================
//   QUEST PDF
// ==========================
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

// ==========================
//   API BASICS
// ==========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==========================
//   MIDDLEWARE
// ==========================
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Necesario para Testing con WebApplicationFactory
public partial class Program { }