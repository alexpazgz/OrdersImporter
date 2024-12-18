using AutoMapper;
using Domain.ApiKataEsPublico;
using Domain.Csv;
using Domain.Entities;
using Domain.Model;
using System.Globalization;

namespace Businnes.AutoMapper
{
    public class MappingProfile : Profile
    {
        readonly string[] FORMATS = { "M/d/yyyy", "M/dd/yyyy", "MM/d/yyyy", "MM/dd/yyyy" };

        public MappingProfile()
        {
            CreateMap<OnlineOrderApiKataResponse, OnlineOrderModel>();

            CreateMap<LinkSelfApiKataResponse, LinkSelfModel>();

            CreateMap<OnlineOrderApiKataResponse, OnlineOrderModel>()
            .ForMember(dest => dest.Link, act => act.MapFrom(src => src.Links));

            CreateMap<LinkSelfApiKataResponse, Link>();

            CreateMap<OnlineOrderApiKataResponse, OnlineOrder>()
                .ForMember(dest => dest.Link, act => act.MapFrom(src => src.Links));

            CreateMap<OnlineOrderModel, OnlineOrder>();

            CreateMap<LinkSelfModel, Link>();

            CreateMap<OnlineOrderModel, OnlineOrderCsv>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Priority, act => act.MapFrom(src => src.Priority))
                .ForMember(dest => dest.Date, act => act.MapFrom(src => DateTime.ParseExact(src.Date, FORMATS, CultureInfo.InvariantCulture, DateTimeStyles.None)))
                .ForMember(dest => dest.Region, act => act.MapFrom(src => src.Region))
                .ForMember(dest => dest.Country, act => act.MapFrom(src => src.Country))
                .ForMember(dest => dest.ItemType, act => act.MapFrom(src => src.ItemType))
                .ForMember(dest => dest.SalesChannel, act => act.MapFrom(src => src.SalesChannel))
                .ForMember(dest => dest.ShipDate, act => act.MapFrom(src => DateTime.ParseExact(src.ShipDate, FORMATS, CultureInfo.InvariantCulture, DateTimeStyles.None)))
                .ForMember(dest => dest.UnitsSold, act => act.MapFrom(src => src.UnitsSold))
                .ForMember(dest => dest.UnitPrice, act => act.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.UnitCost, act => act.MapFrom(src => src.UnitCost))
                .ForMember(dest => dest.TotalRevenue, act => act.MapFrom(src => src.TotalRevenue))
                .ForMember(dest => dest.TotalCost, act => act.MapFrom(src => src.TotalCost))
                .ForMember(dest => dest.TotalProfit, act => act.MapFrom(src => src.TotalProfit));
        }
    }
}
