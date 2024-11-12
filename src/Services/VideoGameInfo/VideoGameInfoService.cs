namespace FusionTech.src.Services.VideoGamesInfo
{
    public class VideoGameInfoService : IVideoGameInfoService
    {
        protected readonly VideoGameInfoRepository _videoGameInfoRepo;
        protected readonly VideoGameVersionRepository _videoGameVersionRepo;
        protected readonly SystemAdminRepository _systemAdminRepository;
        protected readonly PersonRepository _personRepo;

        protected readonly IMapper _mapper;

        public VideoGameInfoService(
            VideoGameInfoRepository videoGameInfoRepository,
            VideoGameVersionRepository videoGameVersionRepository,
            PersonRepository personRepo,
            SystemAdminRepository systemAdminRepository,
            IMapper mapper
        )
        {
            _videoGameInfoRepo = videoGameInfoRepository;
            _videoGameVersionRepo = videoGameVersionRepository;
            _mapper = mapper;
            _personRepo = personRepo;
            _systemAdminRepository = systemAdminRepository;
        }

        public async Task<VideoGameInfoReadDto> CreateOneAsync(
            VideoGameInfoCreateDto createGameInfo,
            string email
        )
        {
            var originalPerson =
                await _personRepo.FindPersonByEmail(email)
                ?? throw CustomException.NotFound("Person not found.");

            var originalSystemAdmin = await _systemAdminRepository.GetByIdAsync(
                originalPerson.PersonId
            );

            if (!originalSystemAdmin!.ManageGames)
                throw CustomException.UnAuthorized("Unauthorized access to manage games.");

            var videoGameInfo = new VideoGameInfo
            {
                GameName = createGameInfo.GameName,
                Description = createGameInfo.Description,
                YearOfRelease = createGameInfo.YearOfRelease,
                TotalRating = createGameInfo.TotalRating,
                PublisherId = createGameInfo.PublisherId,
                GamePicturePath = createGameInfo.GamePicturePath,
                Categories = createGameInfo
                    .CategoryIds.Select(id => new Category { CategoryId = id })
                    .ToList(),
                GameStudios = createGameInfo
                    .GameStudioIds.Select(id => new GameStudio { GameStudioId = id })
                    .ToList(),
            };

            var createdGameInfo = await _videoGameInfoRepo.CreateOneAsync(videoGameInfo);

            foreach (var versionDto in createGameInfo.VideoGameVersions)
            {
                var videoGameVersion = new VideoGameVersion
                {
                    VideoGameVersionId = Guid.NewGuid(),
                    VideoGameInfoId = createdGameInfo.VideoGameInfoId,
                    GameConsoleId = versionDto.GameConsoleId,
                    Price = versionDto.Price,
                };
                await _videoGameVersionRepo.CreateOneAsync(videoGameVersion);
            }

            return _mapper.Map<VideoGameInfoReadDto>(createdGameInfo);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var foundGameInfo = await _videoGameInfoRepo.GetByIdAsync(id);
            if (foundGameInfo == null)
            {
                throw CustomException.NotFound($"Video game with ID {id} not found.");
            }
            bool isDeleted = await _videoGameInfoRepo.DeleteOnAsync(foundGameInfo);
            if (!isDeleted)
            {
                throw CustomException.InternalError("Failed to delete the video game.");
            }
            return await _videoGameInfoRepo.DeleteOnAsync(foundGameInfo);
        }

        public async Task<List<VideoGameInfoReadDto>> GetAllAsync(SearchParameters searchParameters)
        {
            var videoGameList = await _videoGameInfoRepo.GetAllAsync();

            if (videoGameList == null || videoGameList.Count == 0)
            {
                throw CustomException.NotFound("No video games found.");
            }

            searchParameters.IsValid(); // Throws an error incase of a wrong parameters
            videoGameList = videoGameList
                .Where(a =>
                    a.GameName!.Contains(
                        searchParameters.Search ?? string.Empty,
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                .ToList();

            videoGameList = searchParameters.SortBy switch
            {
                SortingTypes.ByRating => searchParameters.Descending
                    ? videoGameList.OrderByDescending(vg => vg.TotalRating).ToList()
                    : videoGameList.OrderBy(vg => vg.TotalRating).ToList(),

                SortingTypes.ByTitle => searchParameters.Descending
                    ? videoGameList.OrderByDescending(vg => vg.GameName).ToList()
                    : videoGameList.OrderBy(vg => vg.GameName).ToList(),

                _ => videoGameList,
            };

            videoGameList = videoGameList
                .Skip(searchParameters.Offset)
                .Take(searchParameters.Limit)
                .ToList();

            return _mapper.Map<List<VideoGameInfo>, List<VideoGameInfoReadDto>>(videoGameList);
        }

        public async Task<List<VideoGameWithVersionDto>> GetAllWithVersionAsync(
            SearchParameters searchParameters
        )
        {
            var videoGameList = await _videoGameInfoRepo.GetAllWithVersionAsync();

            if (videoGameList == null || videoGameList.Count == 0)
            {
                throw CustomException.NotFound("No video games found.");
            }

            searchParameters.IsValid(); // Throws an error incase of a wrong parameters
            videoGameList = videoGameList
                .Where(a =>
                    a.GameName!.Contains(
                        searchParameters.Search ?? string.Empty,
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                .ToList();

             videoGameList = videoGameList
                .Where(vg =>
                    vg.VideoGameVersions.Any(version =>
                        version.Price >= searchParameters.MinPrice
                        && version.Price <= searchParameters.MaxPrice
                    )
                )
                .ToList();
 
            videoGameList = searchParameters.SortBy switch
            {
                SortingTypes.ByRating => searchParameters.Descending
                    ? videoGameList.OrderByDescending(vg => vg.TotalRating).ToList()
                    : videoGameList.OrderBy(vg => vg.TotalRating).ToList(),

                SortingTypes.ByTitle => searchParameters.Descending
                    ? videoGameList.OrderByDescending(vg => vg.GameName).ToList()
                    : videoGameList.OrderBy(vg => vg.GameName).ToList(),

                SortingTypes.ByPrice => searchParameters.Descending
                    ? videoGameList
                        .OrderByDescending(vg => vg.VideoGameVersions.Max(version => version.Price))
                        .ToList()
                    : videoGameList
                        .OrderBy(vg => vg.VideoGameVersions.Min(version => version.Price))
                        .ToList(),

                _ => videoGameList,
            };

            videoGameList = videoGameList
                .Skip(searchParameters.Offset)
                .Take(searchParameters.Limit)
                .ToList();

            return _mapper.Map<List<VideoGameInfo>, List<VideoGameWithVersionDto>>(videoGameList);
        }

        public async Task<VideoGameDetailedDto> GetByIdAsync(Guid id)
        {
            var foundGameInfo = await _videoGameInfoRepo.GetByIdAsync(id);
            if (foundGameInfo == null)
            {
                throw CustomException.NotFound($"Video game with ID {id} not found.");
            }
            return _mapper.Map<VideoGameInfo, VideoGameDetailedDto>(foundGameInfo);
        }

        public async Task<VideoGameDetailedDto> GetVideoGameVersionByIdAsync(Guid id)
        {
            var foundGameInfo = await _videoGameInfoRepo.GetVideoGameVersionByIdAsync(id);
            if (foundGameInfo == null || !foundGameInfo.Any())
            {
                throw CustomException.NotFound($"No video game versions found for ID {id}.");
            }
            return _mapper.Map<VideoGameInfo, VideoGameDetailedDto>(foundGameInfo.First());
        }

        public async Task<List<VideoGameRatingReadDto>> GetVideoGameRatingsByIdAsync(Guid id)
        {
            var foundGameInfoRatings = await _videoGameInfoRepo.GetVideoGameRatingsByIdAsync(id);
            if (foundGameInfoRatings == null || foundGameInfoRatings.Count == 0)
            {
                throw CustomException.NotFound($"No video game ratings found for ID {id}.");
            }
            return foundGameInfoRatings;
        }

        public async Task<bool> UpdateGameNameAsync(Guid id, string newGameName)
        {
            var videoGame = await _videoGameInfoRepo.GetByIdAsync(id);
            if (videoGame == null)
            {
                throw CustomException.NotFound($"Video game with ID {id} not found.");
            }
            videoGame.GameName = newGameName;
            bool isUpdated = await _videoGameInfoRepo.UpdateOnAsync(videoGame);
            if (!isUpdated)
            {
                throw CustomException.InternalError("Failed to update the game name.");
            }

            return await _videoGameInfoRepo.UpdateOnAsync(videoGame);
        }

        public async Task<bool> UpdateYearOfReleaseAsync(Guid id, string newYearOfRelease)
        {
            var videoGame = await _videoGameInfoRepo.GetByIdAsync(id);
            if (videoGame == null)
            {
                throw CustomException.NotFound($"Video game with ID {id} not found.");
            }

            videoGame.YearOfRelease = newYearOfRelease;
            bool isUpdated = await _videoGameInfoRepo.UpdateOnAsync(videoGame);
            if (!isUpdated)
            {
                throw CustomException.InternalError("Failed to update the year of release.");
            }

            return await _videoGameInfoRepo.UpdateOnAsync(videoGame);
        }

        public async Task<int> CountGamesInfosAsync()
        {
            return await _videoGameInfoRepo.CountAsync();
        }
    }
}
