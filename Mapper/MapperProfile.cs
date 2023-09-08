using AutoMapper;
using WorkOrderApi.Commands.Requests;
using WorkOrderApi.Models;

namespace WorkOrderApi.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreateWorkOrderRequest, WorkOrder>()
            .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.Name));
        CreateMap<UpdateWorkOrderRequest, WorkOrder>()
            .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.Name));
    }
}