namespace ClaptonStore.Infrastructure
{
    using AutoMapper;
    using Models;
    using Models.ViewModels;
    using Services;
    using Utilities;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Game, AllGamesViewModel>()
                .ForMember(x => x.Description,
                    cfg => cfg.MapFrom(desc => desc.Description.GetShortDescription()));

            this.CreateMap<Game, GameDetailsViewModel>()
                .ForMember(d => d.Developer, cfg => cfg.MapFrom(dev => dev.Developer.Title))
                .ForMember(x => x.Genre, cfg => cfg.MapFrom(genre => genre.GameGenreType.GetDisplayName()));
        }
    }
}