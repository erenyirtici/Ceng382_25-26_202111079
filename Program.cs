var builder = WebApplication.CreateBuilder(args);

// ✅ Step 1: Add Session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Oturum süresi 30 dakika
    options.Cookie.HttpOnly = true;                  // JavaScript erişemesin
    options.Cookie.IsEssential = true;               // Zorunlu cookie
});

// Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Exception handler
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ✅ Step 2: Use Session middleware
app.UseSession();

app.UseRouting();
app.UseAuthorization();

// Static files
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
