using Asee.Services;
using Asee.Data;
using Asee.Rules;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext with SQLite
builder.Services.AddDbContext<FeeDbContext>(options =>
    options.UseSqlite("Data Source=fee_calculation.db"));

builder.Services.AddScoped<FeeCalculator>();
builder.Services.AddScoped<FeeHistoryService>();


builder.Services.AddScoped<IFeeRule, PosFixedRule>();
builder.Services.AddScoped<IFeeRule, ECommerceCapRule>();
builder.Services.AddScoped<IFeeRule, CreditScoreDiscountRule>();

// Before `builder.Build();`
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
