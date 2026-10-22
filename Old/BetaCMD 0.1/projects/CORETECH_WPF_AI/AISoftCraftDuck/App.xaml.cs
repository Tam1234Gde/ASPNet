using System.Net;
using System.Net.Http;
using System.Windows;
using AISoftCraftDuck.Services;
using AISoftCraftDuck.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;

namespace AISoftCraftDuck;

public partial class App : System.Windows.Application
{
    public IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        
        // Register IEnvService as singleton first
        services.AddSingleton<IEnvService, EnvService>();
        services.AddSingleton<FileDirectiveService>();
        services.AddSingleton<IProcessBridge, ProcessBridge>();
        services.AddSingleton<DockerService>();
        services.AddSingleton<IGitAutoCommitService, GitAutoCommitService>();
        services.AddSingleton<WorkspaceService>();
        services.AddSingleton<ProjectToolService>();

        // Configure HttpClient for ApiDuckClient
        services.AddHttpClient<IApiDuckClient, ApiDuckClient>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var envService = serviceProvider.GetRequiredService<IEnvService>();
                var settings = envService.Current;
                var baseUrl = settings.BaseUrl.TrimEnd('/') + "/";
                client.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
                client.Timeout = TimeSpan.FromMinutes(5);
            })
            .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
            {
                var envService = serviceProvider.GetRequiredService<IEnvService>();
                var proxy = envService.Current.Proxy;
                return string.IsNullOrWhiteSpace(proxy)
                    ? new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli }
                    : new HttpClientHandler { UseProxy = true, Proxy = new WebProxy(proxy), AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli };
            })
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.MaxRetryAttempts = 2;
                options.Retry.Delay = TimeSpan.FromSeconds(1);
                options.Retry.BackoffType = Polly.DelayBackoffType.Exponential;
                options.Retry.UseJitter = true;
                // Autonomous agent tasks can include long model reasoning and tool-call chains.
                options.AttemptTimeout.Timeout = TimeSpan.FromMinutes(10);
                options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(20);
                // The circuit-breaker sampling window must be at least twice the attempt timeout.
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromMinutes(20);
            });

        services.AddSingleton<MainViewModel>();
        Services = services.BuildServiceProvider();

        new MainWindow
        {
            DataContext = Services.GetRequiredService<MainViewModel>()
        }.Show();
    }
}
