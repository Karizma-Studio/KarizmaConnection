using KarizmaConnection.Server.Base;

namespace KarizmaConnection.Server.Extensions;

public static class WebApplicationExtensions
{
    public static void MapKarizmaConnectionHub(this WebApplication webApplication, string mainPath)
    {
        webApplication.MapHub<BaseHub>(mainPath);
    }
}