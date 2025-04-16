using SallaStoreIntegration.BLL;
using SallaStoreIntegration.Setting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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



builder.Services.AddScoped<ISallaBLL, SallaBLL>();

#region Congfig
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
