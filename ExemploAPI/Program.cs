using ExemploAPI.Data;
using ExemploAPI.Interfaces;
using ExemploAPI.Models;
using ExemploAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adicionar e Configurar o CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Adiciona Controladores Padrão
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
    {
        new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
            {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,

        },
        new List<string>()
        }
    });
});


// Add Serviço de Conexao com o Banco de Dados MS SQL SERVER
var strConn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(strConn));

// Serviço de EndPoints do Identity Framework
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Serviço de Autenticação
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Serviço de Configuração e Envio de Emails
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfig>();
// Criar um unica instancia da config do email
builder.Services.AddSingleton(emailConfig);
// Adicionar a Interface ao Escopo do Projeto
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Codigo para trabalhar o formulario de envio de e-mails no ASP.NET Core
/*********
 builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
*********/

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Mapear Endpoints do IdentyUsers a partir de Grupos
app.MapGroup("/identity").MapIdentityApi<IdentityUser>();
app.MapGroup("/roles").MapIdentityApi<IdentityRole>();

app.UseHttpsRedirection();

// Permitir autenticação e autorização a partir de qualquer origem
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");

app.MapControllers();
app.Run();
