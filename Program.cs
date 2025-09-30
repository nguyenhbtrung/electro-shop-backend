using Asp.Versioning;
using CloudinaryDotNet;
using electro_shop_backend.Configurations;
using electro_shop_backend.Data;
using electro_shop_backend.Exceptions.Handlers;
using electro_shop_backend.Exceptions.Mappers;
using electro_shop_backend.Exceptions.Mappers.Interfaces;
using electro_shop_backend.Helpers;
using electro_shop_backend.Hubs;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IExceptionMapper, NotFoundExceptionMapper>();
builder.Services.AddSingleton<IExceptionMapper, ArgumentExceptionMapper>();

builder.Services.AddExceptionHandler<BasicExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader());
    //new HeaderApiVersionReader("X-Api-Version"));
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection")));

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Admin"));
});


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    //options.SignIn.RequireConfirmedAccount = true; // can phai confirm email khi dang ky
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireDigit = true; // password phai co it nhat 1 chu so
    options.Password.RequireLowercase = true; // password phai co it nhat 1 chu thuong
    options.Password.RequireNonAlphanumeric = true; // password phai co it nhat 1 ky tu dac biet
    options.Password.RequireUppercase = true; // password phai co it nhat 1 chu hoa
    options.Password.RequiredLength = 8; // password phai co it nhat 8 ky tu
    options.User.RequireUniqueEmail = true; // email phai la duy nhat
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders(); // de reset password, confirm email, ...

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!)
            )
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/chathub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });


var clientUrlsSection = builder.Configuration.GetSection("ClientUrls");
string[] allowedOrigins = (clientUrlsSection.Exists() && clientUrlsSection.Get<string[]>() is string[] origins && origins.Length > 0)
    ? origins
    : ["http://localhost:5173"];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddSignalR();


var imageStorage = builder.Configuration["ImageStorage"];

if (imageStorage == "Cloudinary")
{
    // Bind CloudinarySettings
    builder.Services.Configure<CloudinarySettings>(
        builder.Configuration.GetSection("CloudinarySettings")
    );

    // Register Cloudinary as Singleton
    builder.Services.AddSingleton(sp =>
    {
        var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;

        if (string.IsNullOrEmpty(config.CloudName) || string.IsNullOrEmpty(config.ApiKey) || string.IsNullOrEmpty(config.ApiSecret))
        {
            throw new InvalidOperationException("Cloudinary config không hợp lệ. Vui lòng kiểm tra appsettings.json.");
        }

        return new Cloudinary(new Account(config.CloudName, config.ApiKey, config.ApiSecret));
    });

    builder.Services.AddScoped<IImageService, CloudinaryImageService>();
}
else
{
    builder.Services.AddScoped<IImageService, ImageService>();
}

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProductViewHistoryService, ProductViewHistoryService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReturnService, ReturnService>();
builder.Services.AddScoped<IReturnReasonService, ReturnReasonService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IFilterService, FilterService>();
//builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IReturnStatusHistoryService, ReturnStatusHistoryService>();
builder.Services.AddScoped<IProductAttributeService, ProductAttributeService>();
builder.Services.AddScoped<IProductPricingService, ProductPricingService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddScoped<ISupportMessageService, SupportMessageService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();

builder.Services.AddHttpContextAccessor();

var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        // Build Swagger endpoints for each API version
        foreach (var desc in descriptions)
        {
            var url = $"/swagger/{desc.GroupName}/swagger.json";
            var name = $"{desc.GroupName.ToUpperInvariant()}";
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseExceptionHandler();

app.UseCors("AllowSpecificOrigin");

app.UseStaticFiles();

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.UseRouting();
app.MapHub<ChatHub>("/chathub");

using (var scope = app.Services.CreateScope())
{
    var voucherService = scope.ServiceProvider.GetRequiredService<IVoucherService>();
    await voucherService.CheckAndUpdateVoucherStatusAsync();
}

app.Run();