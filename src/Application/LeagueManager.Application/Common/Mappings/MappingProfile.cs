using AutoMapper;
using LeagueManager.Application.Player.Commands.CreatePlayer;
using LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddPlayerToLineup;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution;
using System;
using System.Linq;
using System.Reflection;

namespace LeagueManager.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
            CreateMap<TeamLeagueMatches.Commands.AddPlayerToLineup.PlayerDto, CreatePlayerCommand>();
            CreateMap<AddPlayerToLineupCommand, GetPlayerForTeamCompetitorQuery>()
                .ForMember(m => m.PlayerName, opt => opt.MapFrom(src => src.Player.FullName));
            CreateMap<AddPlayerToLineupCommand, AddPlayerToTeamCompetitorCommand>()
                .ForMember(m => m.PlayerName, opt => opt.MapFrom(src => src.Player.FullName))
                .ForMember(m => m.PlayerNumber, opt => opt.MapFrom(src => src.Number));
            CreateMap<AddPlayerToLineupDto, AddPlayerToLineupCommand>();
            CreateMap<UpdateTeamLeagueMatchSubstitutionDto, UpdateTeamLeagueMatchSubstitutionCommand>();
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes();
            var interfaceType = typeof(IMapFrom<>);
            var methodName = nameof(IMapFrom<object>.Mapping);
            var argumentTypes = new Type[] { typeof(Profile) };

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
                    .ToList();

                // Has the type implemented any IMapFrom<T>?
                if (interfaces.Count > 0)
                {
                    // Yes, then let's create an instance
                    var instance = Activator.CreateInstance(type);

                    // and invoke each implementation of `.Mapping()`
                    foreach (var i in interfaces)
                    {
                        var methodInfo = i.GetMethod(methodName, argumentTypes);

                        methodInfo?.Invoke(instance, new object[] { this });
                    }
                }
            }
        }
    }
}