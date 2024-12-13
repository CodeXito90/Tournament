using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core;
using Tournament.Core.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public class TournamentMappings : Profile
    {
        public TournamentMappings()
        {
            // Mapping TournamentForCreationDto to TournamentDetails
            CreateMap<TournamentForCreationDto, TournamentDetails>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.Games, opt => opt.MapFrom(src => src.Games));


            // Map from TournamentDetails to TournamentDto
            CreateMap<TournamentDetails, TournamentDto>()
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.StartDate.AddMonths(3)));

            //Map from tournament entity to our DTOs
            CreateMap<TournamentDetails, TournamentDto>().ForMember(dest => dest.EndDate,
                  opt => opt.MapFrom(src => src.StartDate.AddMonths(3)));

            //Keynote: Allways implement reverse map from intital mapping
            //Map from DTO to our tournament entityt 

            CreateMap<TournamentDto, TournamentDetails>().ForMember(dest => dest.StartDate,
                  opt => opt.MapFrom(src => src.StartDate)).ForMember(dest => dest.Games,
                  opt => opt.MapFrom(src => src.Games));

            CreateMap<Game, GameDto>().ReverseMap();

            // Add mapping for GameForCreationDto to Game
            CreateMap<GameForCreationDto, Game>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id (assuming it's auto-generated)
                .ForMember(dest => dest.TournamentId, opt => opt.Ignore()); // Ignore navigation property

            CreateMap<GameForCreationDto, Game>().ReverseMap();
        }
    }
}
