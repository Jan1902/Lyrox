using Lyrox.Framework.Core.Abstraction.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lyrox.Plugins.WebView;

public class WebViewManager
{
    private readonly ILyroxConfiguration _lyroxConfiguration;
    private readonly ILogger<WebViewManager> _logger;

    public WebViewManager(ILyroxConfiguration lyroxConfiguration, ILogger<WebViewManager> logger)
    {
        _lyroxConfiguration = lyroxConfiguration;
        _logger = logger;

        Init();
    }

    private void Init()
    {
        _lyroxConfiguration.CustomOptions.TryGetValue("webViewPort", out var port);

        var builder = WebApplication.CreateBuilder();
        builder.Services.AddRazorPages();
        builder.Services.AddControllersWithViews();
        builder.Logging.ClearProviders();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        //app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.MapGet("/", () => "Welcome to Lyrox Web View!");

        app.MapDefaultControllerRoute();
        app.MapRazorPages();
        app.RunAsync();

        _logger.LogInformation($"Web View running on port {port}");
    }
}
