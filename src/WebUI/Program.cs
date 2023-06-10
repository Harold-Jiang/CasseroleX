using Microsoft.AspNetCore.StaticFiles;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// serilog
builder.Host.UseSerilog((context, services, configuration) =>
               configuration
                   .Enrich.FromLogContext()
                   .WriteTo.Console());
// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();

var app = builder.Build();


app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    // ҳ���쳣
    app.UseStatusCodePagesWithReExecute("/Error/NoFound", "?statusCode={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();

// Set up custom content types - associating file extension to MIME type
var provider = new FileExtensionContentTypeProvider();
// Add new mappings
provider.Mappings[".less"] = "application/octet-stream";
provider.Mappings[".woff"] = "font/woff";
 
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider, 
});

//app.UseSwaggerUi3(settings =>
//{
//    settings.Path = "/api";
//    settings.DocumentPath = "/api/specification.json";
//});

app.UseRouting();
//ʹ�ÿ���
app.UseCors("CorsPolicy");
//������֤
app.UseAuthentication();
//��Ȩ����
app.UseAuthorization();
//����м����������־��¼
app.UseSerilogRequestLogging();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Index}/{action=Index}/{id?}");
app.MapControllers();
app.MapRazorPages();

app.Run();