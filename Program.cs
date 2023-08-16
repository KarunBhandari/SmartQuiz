using IQMania.Repository;
using IQMania.Repository.Completion;
using Microsoft.AspNetCore.Authentication.Cookies;
using IQMania.Repository.AdminRepository;
using Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
Configurationmanager.SetConfiguration(configuration);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        
    .AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromHours(24);
            options.LoginPath = "/Account/Login"; // Set the login path to your login page URL
            
        });
//builder.Services.AddControllersWithViews(options => 
//{ 
//    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());    
//});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    //You can set Time   
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    
});

builder.Services.AddMvc();
builder.Services.AddSingleton<IQuizServices, QuizServices>();
builder.Services.AddSingleton<IAccountServices, AccountServices>();
builder.Services.AddSingleton<ICompletionRepository, CompletionServices>();
builder.Services.AddSingleton<IAdminServices, AdminServices>(); 
//builder.Services.AddScoped<PermissionMiddleware>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions
    {
        SourceCodeLineCount = 10
    };
    app.UseDeveloperExceptionPage(developerExceptionPageOptions);
    //app.UseStatusCodePages();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    
}

app.UseSerilogRequestLogging();
app.UseStaticFiles();

//app.UseCookiePolicy();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Quiz}/{action=Index}/{id?}");

app.Run();//(async(HttpContext context)// =>
//{
//    string method = context.Request.Method;
//    string Path = context.Request.Path;
//    if(method=="Get"&& Path=="/CategoryWiseQuestions")
//    {
//        string id = "";
//        string name = "";
//        StreamReader Reader = new StreamReader(context.Request.Body);
//        string data = await Reader.ReadToEndAsync();
//        Dictionary<string, StringValues> dict = QueryHelpers.ParseQuery(data);

//        if(dict.ContainsKey("id"))
//        {
//            id = dict["id"];
//        }
//        if (dict.ContainsKey("name"))
//        {
//            id = dict["name"][0];
//        }
//        await context.Response.WriteAsync("Request body contains:" + "\nID:" + id + "\nName: "+ name );
//    }
//});
