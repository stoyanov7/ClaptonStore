namespace ClaptonStore.Infrastructure
{
    using AutoMapper;
    using Models;
    using Models.ViewModels;
    using Services;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Game, AllGamesViewModel>();

            this.CreateMap<Game, GameDetailsViewModel>()
                .ForMember(d => d.Developer, cfg => cfg.MapFrom(dev => dev.Developer.Title))
                .ForMember(x => x.Genre, cfg => cfg.MapFrom(genre => genre.GameGenreType.GetDisplayName()));
        }
    }
}