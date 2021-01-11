using AutoMapper;
using TeamSpeakWEB.Data.ViewModels;
using TeamSpeakWEB.Models;

namespace TeamSpeakWEB.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Tsserver, TsserverView>();
        }
    }
}