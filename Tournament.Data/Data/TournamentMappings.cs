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
            //Map from tournament entity to our DTOs
            CreateMap<TournamentDetails, TournamentDto>().ForMember(dest => dest.EndDate,
                  opt => opt.MapFrom(src => src.StartDate.AddMonths(3)));

            //Keynote: Allways implement reverse map from intital mapping
            //Map from DTO to our tournament entityt 

            CreateMap<TournamentDto, TournamentDetails>().ForMember(dest => dest.StartDate,
                  opt => opt.MapFrom(src => src.StartDate)).ForMember(dest => dest.Games,
                  opt => opt.MapFrom(src => src.Games));

            CreateMap<Game, GameDto>().ReverseMap();
        }
    }
}
