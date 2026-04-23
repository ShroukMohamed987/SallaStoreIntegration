using SallaStoreIntegration.Repositories.Auth;
using SallaStoreIntegration.Repositories.Brand;
using SallaStoreIntegration.Repositories.Category;
using SallaStoreIntegration.Repositories.Customer;
using SallaStoreIntegration.Repositories.Order;
using SallaStoreIntegration.Repositories.Product;
using SallaStoreIntegration.Setting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

#region SallaConfig
builder.Services.AddHttpClient(ExternalStoresProviderEnum.Salla.ToString(), client =>
{
    client.BaseAddress = new Uri("https://api.salla.dev/admin/v2/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
#endregion

#region Repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
#endregion

#region Config
builder.Services.Configure<ExternalStoresSetting>(builder.Configuration.GetSection(AppSettingEnum.ExternalStoresSetting.ToString()));
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
