using Microsoft.Extensions.Options;
using Spectre.Console;
using Task3.Options;

namespace Task3.Renderers;

public class Renderer : IRenderer
{
    private readonly HttpClient client;
    private readonly IOptionsMonitor<DisplayOptions> options;
    private DisplayOptions displayOptions;

    public Renderer(HttpClient httpClient, IOptionsMonitor<DisplayOptions> display)
    {
        client = httpClient;
        options = display;
        displayOptions = options.CurrentValue;
        options.OnChange(async updateOptions =>
        {
            AnsiConsole.Clear();
            displayOptions = updateOptions;
            await Render();
        });
    }

    public async Task Render()
    {
        switch (displayOptions.ContentType)
        {
            case "FIGlet":
                FigletRender();
                break;
            case "Base64":
                BaseRender();
                break;
            case "URL":
                await UrlRender();
                break;
        }
    }

    private void FigletRender()
    {
        FigletText figlet = new FigletText(displayOptions.Content)
            .Centered();
        AnsiConsole.Write(figlet);
    }

    private void BaseRender()
    {
        byte[] image = Convert.FromBase64String(displayOptions.Content);
        var canvas = new CanvasImage(image);
        canvas.MaxWidth(100);
        AnsiConsole.Write(canvas);
    }

    private async Task UrlRender()
    {
        byte[] image = await client.GetByteArrayAsync(displayOptions.Content);
        var canvas = new CanvasImage(image);
        canvas.MaxWidth(100);
        AnsiConsole.Write(canvas);
    }
}