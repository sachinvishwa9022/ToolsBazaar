using AspNetCore.Authentication.ApiKey;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.OrderAggregate;
using ToolsBazaar.Domain.ProductAggregate;
using ToolsBazaar.Persistence;
using ToolsBazaar.Web.Helpers;
using ToolsBazaar.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ICustomerHelper, CustomerHelper>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
            .AddApiKeyInHeader<APIKeyProvider>(options =>
            {
                options.Realm = "Tools Bazaar API";
                options.KeyName = "X-API-KEY";
                options.IgnoreAuthenticationIfAllowAnonymous = true;
            });



var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

var requestCulture = new RequestCulture("en-US");
requestCulture.Culture.DateTimeFormat.ShortDatePattern = "MM-dd-yyyy";
app.UseRequestLocalization(new RequestLocalizationOptions
                           {
                               DefaultRequestCulture = requestCulture
                           });

app.MapControllerRoute("default",
                       "{controller}/{action=Index}/{id?}");

app.Run();