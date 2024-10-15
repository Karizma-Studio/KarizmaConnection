using KarizmaCon.Server.Base;

namespace KarizmaCon.Server.Extensions;

public static class WebApplicationExtensions
{
    public static void MapKarizmaConHub(this WebApplication webApplication, string mainPath)
    {
        webApplication.MapHub<BaseHub>(mainPath);
    }
}