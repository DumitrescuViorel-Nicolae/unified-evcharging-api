using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Stripe;

namespace Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDTO>();

            CreateMap<EVStationDTO, EVStation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.TotalNumberOfConnectors, opt => opt.MapFrom(src => src.TotalNumberOfConnectors))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Contacts.Phone))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Contacts.Website))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Position.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Position.Longitude));

            CreateMap<PaymentMethodDTO, Domain.Entities.PaymentMethod>()
                 .ForMember(dest => dest.EPaymentAccept, opt => opt.MapFrom(src => src.EPayment.Accept))
                .ForMember(dest => dest.OtherPaymentAccept, opt => opt.MapFrom(src => src.Other.Accept))
                .ForMember(dest => dest.EPaymentTypes, opt => opt.MapFrom(src => string.Join(", ", src.EPayment.Types.Type)))
                .ForMember(dest => dest.OtherPaymentTypes, opt => opt.MapFrom(src => string.Join(", ", src.Other.Types.Type)));

            CreateMap<ConnectorDetailDto, ConnectorDetail>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
                .ForMember(dest => dest.ConnectorType, opt => opt.MapFrom(src => src.ConnectorType))
                .ForMember(dest => dest.ChargeCapacity, opt => opt.MapFrom(src => src.ChargeCapacity))
                .ForMember(dest => dest.MaxPowerLevel, opt => opt.MapFrom(src => src.MaxPowerLevel))
                .ForMember(dest => dest.CustomerChargeLevel, opt => opt.MapFrom(src => src.CustomerChargeLevel))
                .ForMember(dest => dest.CustomerConnectorName, opt => opt.MapFrom(src => src.CustomerConnectorName));
            CreateMap<ConnectorStatusDto, ConnectorStatus>()
                .ForMember(dest => dest.PhysicalReference, opt => opt.MapFrom(src => src.PhysicalReference))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));

            CreateMap<PaymentIntent, PaymentTransaction>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount / 100m)) 
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.PaymentMethodBrand, opt => opt.MapFrom(src => src.PaymentMethodTypes[0])) 
                .ForMember(dest => dest.PaymentMethodLast4, opt => opt.MapFrom(src => src.PaymentMethod.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ReceiptUrl, opt => opt.MapFrom(src => src.Charges.Data[0].ReceiptUrl))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Created));
        }
    }
}
