using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;

namespace DutchTreat.AutoMapper
{
    public class EntityToViewModelProfile : Profile
    {
        public EntityToViewModelProfile()
        {
            CreateMap<Order, OrderViewModel>()
                .ForMember(viewModel => viewModel.OrderId,
                           opt => opt.MapFrom(entity => entity.Id));
        }
    }
}
