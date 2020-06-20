using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Interfaces;
using GetTeamLeagueRounds = LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Infrastructure.HttpHelpers;
using Microsoft.Extensions.Options;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using UpdateTeamLeagueMatch = LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using UpdateTeamLeagueMatchScore = LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.Config;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueCompetitors;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry;
using UpdateTeamLeagueMatchLineupEntry = LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeague;
using Newtonsoft.Json;
using LeagueManager.Infrastructure.Json;
using LeagueManager.Application.Interfaces.Dto;
using System;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution;

namespace LeagueManager.Infrastructure.Api
{
    public class CompetitionApi : ICompetitionApi
    {
        private readonly IHttpRequestFactory httpRequestFactory;
        private readonly ApiSettings apiSettings;
        private readonly string teamLeagueApiUrl;

        public CompetitionApi(IHttpRequestFactory httpRequestFactory,
            IOptions<ApiSettings> options)
        {
            this.httpRequestFactory = httpRequestFactory;
            this.apiSettings = options.Value;
            this.teamLeagueApiUrl = $"{apiSettings.TeamLeagueApiUrl}"; 
        }

        public async Task<bool> Configure(DbConfig dbConfig, string accessToken)
        {
            var response = await httpRequestFactory.Put(
                $"{apiSettings.CompetitionApiUrl}/configuration",
                dbConfig,
                accessToken
            );

            return response.IsSuccessStatusCode;
        }

        public async Task CreateTeamLeague(CreateTeamLeagueCommand command)
        {
            var response = await httpRequestFactory.Post($"{apiSettings.CompetitionApiUrl}/teamleague", command);
            if (!response.IsSuccessStatusCode)
                throw new CreateTeamLeagueException(command.Name);
        }

        public async Task<IEnumerable<Application.Competitions.Queries.GetCompetitions.CompetitionDto>> GetCompetitions(GetCompetitionsQuery query)
        {
            var response = await httpRequestFactory.Get($"{apiSettings.CompetitionApiUrl}");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<IEnumerable<Application.Competitions.Queries.GetCompetitions.CompetitionDto>>();
            return new List<Application.Competitions.Queries.GetCompetitions.CompetitionDto>();
        }

        public async Task<Application.Competitions.Queries.GetCompetition.CompetitionDto> GetCompetition(GetCompetitionQuery query)
        {
            var response = await httpRequestFactory.Get($"{apiSettings.CompetitionApiUrl}/{query.Name}");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<Application.Competitions.Queries.GetCompetition.CompetitionDto>();
            return new Application.Competitions.Queries.GetCompetition.CompetitionDto();
        }

        public async Task<IEnumerable<CompetitorDto>> GetCompetitors(GetTeamLeagueCompetitorsQuery query)
        {
            var response = await httpRequestFactory.Get($"{apiSettings.TeamLeagueApiUrl}/{query.LeagueName}/competitors");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<IEnumerable<CompetitorDto>>();

            return new List<CompetitorDto>();
        }

        public async Task<IEnumerable<CompetitorPlayerDto>> GetPlayersForTeamCompetitor(GetPlayersForTeamCompetitorQuery query)
        {
            var response = await httpRequestFactory.Get($"{apiSettings.TeamLeagueApiUrl}/{query.LeagueName}/competitors/{query.TeamName}/players");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<IEnumerable<CompetitorPlayerDto>>();
            return null;
        }

        public async Task<GetTeamLeagueVm> GetTeamLeague(string leagueName)
        {
            string requestUri = $"{teamLeagueApiUrl}/{leagueName}";
            var response = await httpRequestFactory.Get(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    Converters =
                    {
                        new JsonInterfaceConverter<GetTeamLeagueRounds.RoundDto, IRoundDto<GetTeamLeagueRounds.TeamMatchDto>>(),
                        new JsonInterfaceConverter<GetTeamLeagueRounds.TeamMatchDto, ITeamMatchDto<ITeamMatchEntryDto>>(),
                        new JsonInterfaceConverter<GetTeamLeagueRounds.TeamMatchEntryDto, ITeamMatchEntryDto>(),
                        new JsonInterfaceConverter<GetTeamLeagueRounds.TeamDto, ITeamDto>(),
                        new JsonInterfaceConverter<GetTeamLeagueRounds.IntegerScoreDto, IIntegerScoreDto>()
                    }
                };
                return response.ContentAsType<GetTeamLeagueVm>(settings);
            }
                
            return null;
        }

        public async Task<GetTeamLeagueRounds.GetTeamLeagueRoundsVm> GetTeamLeagueRounds(GetTeamLeagueRounds.GetTeamLeagueRoundsQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/rounds");
            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    Converters =
                    {
                        new JsonInterfaceConverter<GetTeamLeagueRounds.RoundDto, IRoundDto<GetTeamLeagueRounds.TeamMatchDto>>()
                    }
                };
                return response.ContentAsType<GetTeamLeagueRounds.GetTeamLeagueRoundsVm>(settings);
            }
                
            return null;
        }

        public async Task<GetTeamLeagueTableVm> GetTeamLeagueTable(GetTeamLeagueTableQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/table");
            if (response.IsSuccessStatusCode)
            {
                return response.ContentAsType<GetTeamLeagueTableVm>();
            }

            return null;
        }

        public async Task<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch.TeamMatchDto> GetTeamLeagueMatch(GetTeamLeagueMatchQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/matches/{query.Guid}");
            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    Converters =
                    {
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch.TeamMatchEntryDto, ITeamMatchEntryDto>(),
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch.TeamDto, ITeamDto>(),
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch.IntegerScoreDto, IIntegerScoreDto>()
                    }
                };
                return response.ContentAsType<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch.TeamMatchDto>(settings);
            }
                
            return null;
        }

        public async Task<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.TeamMatchDto> GetTeamLeagueMatchDetails(GetTeamLeagueMatchDetailsQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/matches/{query.Guid}/details");
            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    Converters =
                    {
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.TeamMatchEntryDto, ITeamMatchEntryWithDetailsDto>(),
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.TeamMatchDto, ITeamMatchDto<ITeamMatchEntryWithDetailsDto>>(),
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.TeamDto, ITeamDto>(),
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.IntegerScoreDto, IIntegerScoreDto>(),
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.LineupEntryDto, ILineupEntryDto>(),
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.PlayerDto, IPlayerDto>(),
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.GoalDto, IGoalDto>(),
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.SubstitutionDto, ISubstitutionDto>()
                    }
                };
                return response.ContentAsType<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.TeamMatchDto>(settings);
            }
                
            return null;
        }

        public async Task<UpdateTeamLeagueMatch.TeamMatchDto> UpdateTeamLeagueMatch(UpdateTeamLeagueMatch.UpdateTeamLeagueMatchCommand command)
        {
            var response = await httpRequestFactory.Put(
                $"{teamLeagueApiUrl}/{command.LeagueName}/matches/{command.Guid}",
                command
            );

            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    Converters =
                    {
                        new JsonInterfaceConverter<UpdateTeamLeagueMatch.TeamMatchDto, ITeamMatchDto<ITeamMatchEntryDto>>(),
                        new JsonInterfaceConverter<UpdateTeamLeagueMatch.TeamMatchEntryDto, ITeamMatchEntryDto>(),
                        new JsonInterfaceConverter<UpdateTeamLeagueMatch.TeamDto, ITeamDto>(),
                        new JsonInterfaceConverter<UpdateTeamLeagueMatch.IntegerScoreDto, IIntegerScoreDto>()
                    }
                };
                return response.ContentAsType<UpdateTeamLeagueMatch.TeamMatchDto>(settings);
            }
                
            return null;
        }

        public async Task<UpdateTeamLeagueMatchScore.TeamMatchDto> UpdateTeamLeagueMatchScore(string leagueName, Guid guid, UpdateTeamLeagueMatchScore.UpdateTeamLeagueMatchScoreDto dto)
        {
            var response = await httpRequestFactory.Put(
                $"{teamLeagueApiUrl}/{leagueName}/matches/{guid}/score",
                dto
            );

            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    Converters =
                    {
                        new JsonInterfaceConverter<UpdateTeamLeagueMatchScore.TeamMatchDto, ITeamMatchDto<ITeamMatchEntryDto>>(),
                        new JsonInterfaceConverter<UpdateTeamLeagueMatchScore.TeamMatchEntryDto, ITeamMatchEntryDto>(),
                        new JsonInterfaceConverter<UpdateTeamLeagueMatchScore.TeamDto, ITeamDto>(),
                        new JsonInterfaceConverter<UpdateTeamLeagueMatchScore.IntegerScoreDto, IIntegerScoreDto>()
                    }
                };

                return response.ContentAsType<UpdateTeamLeagueMatchScore.TeamMatchDto>(settings);
            }
                
            return null;
        }

        public async Task<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry.LineupEntryDto> GetTeamLeagueMatchLineupEntry(GetTeamLeagueMatchLineupEntryQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/matches/{query.MatchGuid}/lineup/{query.LineupEntryGuid}");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType< Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry.LineupEntryDto >();
            return null;
        }

        public async Task<Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry.LineupEntryDto> UpdateTeamLeagueMatchLineupEntry(UpdateTeamLeagueMatchLineupEntry.UpdateTeamLeagueMatchLineupEntryCommand command)
        {
            var response = await httpRequestFactory.Put(
                $"{teamLeagueApiUrl}/{command.LeagueName}/matches/{command.MatchGuid}/matchEntries/{command.TeamName}/lineup/{command.LineupEntryGuid}",
                new
                {
                    command.PlayerNumber,
                    command.PlayerName
                }
            );

            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    Converters =
                    {
                        new JsonInterfaceConverter<UpdateTeamLeagueMatchLineupEntry.LineupEntryDto, ILineupEntryDto>(),
                        new JsonInterfaceConverter<UpdateTeamLeagueMatchLineupEntry.PlayerDto, IPlayerDto>()
                    }
                };

                return response.ContentAsType<UpdateTeamLeagueMatchLineupEntry.LineupEntryDto>(settings);
            }
                
            return null;
        }

        public async Task<MatchEventsDto> GetTeamLeagueMatchEvents(GetTeamLeagueMatchEventsQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/matches/{query.MatchGuid}/matchEntries/{query.TeamName}/events");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<MatchEventsDto>();
            return null;
        }

        public async Task<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal.GoalDto> GetTeamLeagueMatchGoal(GetTeamLeagueMatchGoalQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/matches/{query.MatchGuid}/goals/{query.GoalGuid}");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal.GoalDto>();
            return null;
        }

        public async Task<Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal.GoalDto> UpdateTeamLeagueMatchGoal(UpdateTeamLeagueMatchGoalCommand command)
        {
            var response = await httpRequestFactory.Put(
                $"{teamLeagueApiUrl}/{command.LeagueName}/matches/{command.MatchGuid}/matchEntries/{command.TeamName}/goals/{command.GoalGuid}",
                new
                {
                    command.Minute,
                    command.PlayerName
                }
            );

            if (response.IsSuccessStatusCode)
                return response.ContentAsType<Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal.GoalDto>();
            return null;
        }

        public async Task<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution.SubstitutionDto> GetTeamLeagueMatchSubstitution(GetTeamLeagueMatchSubstitutionQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/matches/{query.MatchGuid}/matchEntries/{query.TeamName}/substitutions/{query.SubstitutionGuid}");
            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    Converters =
                    {
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution.SubstitutionDto, ISubstitutionDto>(),
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution.PlayerDto, IPlayerDto>()
                    }
                };

                return response.ContentAsType<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution.SubstitutionDto>(settings);
            }
                
            return null;
        }

        public async Task<Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution.SubstitutionDto> UpdateTeamLeagueMatchSubstitution(
            string leagueName, Guid matchGuid, string teamName, Guid substitutionGuid,
            UpdateTeamLeagueMatchSubstitutionDto dto)
        {
            var response = await httpRequestFactory.Put(
                $"{teamLeagueApiUrl}/{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/substitutions/{substitutionGuid}",
                dto
            );

            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    Converters =
                    {
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution.SubstitutionDto, ISubstitutionDto>(),
                        new JsonInterfaceConverter<Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution.PlayerDto, IPlayerDto>()
                    }
                };

                return response.ContentAsType<Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution.SubstitutionDto>(settings);
            }
                
            return null;
        }
    }
}