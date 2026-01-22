using Task3.Renderers;

namespace Task3.Service;

public class ConsoleService : IConsoleService
{
    private readonly IRenderer renderer;

    public ConsoleService(IRenderer contentRenderer)
    {
        renderer = contentRenderer;
    }

    public async Task Run(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                await renderer.Render();
                await Task.Delay(3000, token);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }
}