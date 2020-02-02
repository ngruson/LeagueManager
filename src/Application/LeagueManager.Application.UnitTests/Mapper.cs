using AutoMapper;
using LeagueManager.Application.AutoMapper;
using LeagueManager.Application.TeamLeagueMatches.Lineup;

namespace LeagueManager.Application.UnitTests
{
    public class Mapper
    {
        public static IConfigurationProvider MapperConfig()
        {
            return new MapperConfiguration(opts =>
            {
                opts.AddProfile<ApplicationProfile>();
                opts.AddProfile<LineupProfile>();
            });
        }

        public static IMapper CreateMapper()
        {
            return MapperConfig().CreateMapper();
        }
    }
}