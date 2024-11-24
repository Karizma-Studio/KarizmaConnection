using KarizmaPlatform.Connection.Server.Base;

namespace KarizmaPlatform.Connection.Server.Extensions;

public static class WebApplicationExtensions
{
    public static void MapKarizmaConnectionHub(this WebApplication webApplication, string mainPath)
    {
        webApplication.MapHub<MainHub>(mainPath);
    }
}