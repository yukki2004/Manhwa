using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetHomeRankings
{
    public class GetHomeRankingsQueryHandler : IRequestHandler<GetHomeRankingsQuery, HomeRankingsDto>
    {
        private readonly ICacheService _cacheService;
        private readonly IStoryRepository _storyRepository;
        public GetHomeRankingsQueryHandler(ICacheService cacheService, IStoryRepository storyRepository)
        {
            _cacheService = cacheService;
            _storyRepository = storyRepository;
        }
        public async Task<HomeRankingsDto> Handle(GetHomeRankingsQuery query, CancellationToken ct)
        {
            var dailyKey = await _cacheService.ResolveRankingKeyAsync(RankingType.Daily, ct);
            var weeklyKey = await _cacheService.ResolveRankingKeyAsync(RankingType.Weekly, ct);
            var monthlyKey = await _cacheService.ResolveRankingKeyAsync(RankingType.Monthly, ct);
            var allTimeKey = "ranking:all_time";

            var dailyTask = _cacheService.GetSortedSetPagedAsync(dailyKey, 1, 10, ct);
            var weeklyTask = _cacheService.GetSortedSetPagedAsync(weeklyKey, 1, 10, ct);
            var monthlyTask = _cacheService.GetSortedSetPagedAsync(monthlyKey, 1, 10, ct);
            var allTimeTask = _cacheService.GetSortedSetPagedAsync(allTimeKey, 1, 10, ct);

            await Task.WhenAll(dailyTask, weeklyTask, monthlyTask, allTimeTask);

            var dailyIds = dailyTask.Result.IDs.Select(long.Parse).ToList();
            var weeklyIds = weeklyTask.Result.IDs.Select(long.Parse).ToList();
            var monthlyIds = monthlyTask.Result.IDs.Select(long.Parse).ToList();
            var allTimeIds = allTimeTask.Result.IDs.Select(long.Parse).ToList();

            var allUniqueIds = dailyIds.Concat(weeklyIds)
                                       .Concat(monthlyIds)
                                       .Concat(allTimeIds)
                                       .Distinct().ToList();

            if (!allUniqueIds.Any()) return new HomeRankingsDto();

            var stories = await _storyRepository.GetActiveStoriesByIdsAsync(allUniqueIds, ct);

            return new HomeRankingsDto
            {
                Daily = MapAndSort(dailyIds, stories),
                Weekly = MapAndSort(weeklyIds, stories),
                Monthly = MapAndSort(monthlyIds, stories),
                AllTime = MapAndSort(allTimeIds, stories)
            };
        }
        private List<GetHomeRankingsStoryDto> MapAndSort(List<long> ids, List<Manhwa.Domain.Entities.Story> stories)
        {
            return ids
                .Select(id => stories.FirstOrDefault(s => s.StoryId == id))
                .Where(s => s != null) 
                .Select(s => new GetHomeRankingsStoryDto
                {
                    StoryId = s!.StoryId,
                    Title = s.Title,
                    Slug = s.Slug,
                    Release = s.ReleaseYear,
                    Thumbnail = s.ThumbnailUrl,

                })
                .ToList();
        }
    }
}
