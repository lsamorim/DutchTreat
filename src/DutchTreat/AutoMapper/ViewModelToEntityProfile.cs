using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;

namespace DutchTreat.AutoMapper
{
    public class ViewModelToEntityProfile : Profile
    {
        public ViewModelToEntityProfile()
        {
            CreateMap<OrderViewModel, Order>()
                .ForMember(entity => entity.Id,
                           opt => opt.MapFrom(viewModel => viewModel.OrderId))
                .ReverseMap();

            CreateMap<OrderItemViewModel, OrderItem>()
                .ReverseMap();
        }
    }
}
