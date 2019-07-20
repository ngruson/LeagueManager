namespace LeagueManager.Domain.Points
{
    public class PointSystem
    {
        public PointSystem() : this(3, 1, 0)
        {
        }
        public PointSystem(int win, int draw, int lost)
        {
            Win = win;
            Draw = draw;
            Lost = lost;
        }
        public int Id { get; set; }
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Lost { get; set; }
    }
}