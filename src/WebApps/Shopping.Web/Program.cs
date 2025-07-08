using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shopping.Web.Services.Handlers;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Registrar IHttpContextAccessor para acesso ao HttpContext
builder.Services.AddHttpContextAccessor();

// Registrar o AuthTokenHandler
builder.Services.AddTransient<AuthTokenHandler>();

// Configura sessões para armazenar estado de autenticação
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.IdleTimeout = TimeSpan.FromHours(1); // Tempo de vida da sessão
});

// Persist antiforgery keys to keep cookies valid after restarts
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "keys")))
    .SetApplicationName("Shopping.Web");

// Adiciona Razor Pages e o filtro global
builder.Services.AddRazorPages(options =>
{
    options.Conventions.ConfigureFilter(new AuthenticationPageFilter());
});

// Configuração para autenticação baseada em JWT, mas usada principalmente para verificação de token, não para o frontend direto
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found")))
    };
});

// Adiciona Refit para comunicar com as APIs, usando o AuthTokenHandler para adicionar o token nas requisições
builder.Services.AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
    });
builder.Services.AddRefitClient<IBasketService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
    });
builder.Services.AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
    });
builder.Services.AddRefitClient<IUserService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
    })
    .AddHttpMessageHandler<AuthTokenHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware de Sessão para manter o estado de autenticação
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
