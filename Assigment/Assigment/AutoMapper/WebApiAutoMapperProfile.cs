using Assigment.Models;
using Assigment.ModelsDao;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;

namespace Assigment.AutoMapper
{
    ///<inheritdoc/>
    public class WebApiAutoMapperProfile : Profile
    {

        public WebApiAutoMapperProfile()
        {
            CreateMap<UserModel, CreateUserDto>().ReverseMap();
            CreateMap<UserModel, LoginUserDto>().ReverseMap();
            CreateMap<UserModel, UserDto>().ReverseMap();
            CreateMap<UserModel, UserDao>().ReverseMap();

            CreateMap<Party, CreatePartyDto>().ReverseMap();
            CreateMap<Party, PartyDto>().ReverseMap();
            CreateMap<Party, PartyDao>().ReverseMap();

            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<Item, CreateItemDto>().ReverseMap();
            CreateMap<Item, ItemFilterDto>().ReverseMap();
            CreateMap<Item, ItemDao>();
            CreateMap<ItemDao, Item>()
            .ForMember(dest => dest.PartyIds, opt => opt.MapFrom(src => src.ItemParties.Select(ip => ip.PartyId).ToList()));

            CreateMap<Proposal, CreateProposalDto>().ReverseMap();
            CreateMap<Proposal, ProposalDto>().ReverseMap();
            CreateMap<Proposal, ProposalDao>().ReverseMap();

            CreateMap<CounterProposal, CreateCounterProposalDto>().ReverseMap();
            CreateMap<CounterProposal, CounterProposalDto>().ReverseMap();
            CreateMap<CounterProposal, CounterProposalDao>().ReverseMap();

            CreateMap<InvolvedParties, InvolvedPartiesDto>().ReverseMap();
            CreateMap<InvolvedParties, InvolvedPartiesDao>().ReverseMap();
        }
    }
}
