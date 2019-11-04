namespace LeagueManager.Persistence.EntityFramework
{
    public interface IImageFileLoader
    {
        byte[] LoadImage(string filePath);
    }
}