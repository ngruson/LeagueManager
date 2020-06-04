using LeagueManager.Application.Interfaces.Dto;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class TeamDto : /*IMapFrom<Team>,*/ ITeamDto
    {
        public string Name { get; set; }
        //public string CountryName { get; set; }
        public string Logo { get; set; }

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<Team, ITeamDto>()
        //        //.ForMember(m => m.Country, opt => opt.MapFrom(src => src.Country != null ? src.Country.Name : null))
        //        .ForMember(m => m.Logo, opt => opt.MapFrom(src => src.Logo != null ?
        //            $"data:image/gif;base64,{Convert.ToBase64String(src.Logo)}" : null))
        //        .As<TeamDto>();
        //}
    }
}