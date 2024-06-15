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

            CreateMap<EVStation, EVStationDTO>()
                 .ForMember(dest => dest.StationID, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand ?? src.Company.CompanyName))
                 .ForMember(dest => dest.StripeAccountID, opt => opt.MapFrom(src => src.Company.StripeAccountID))
                 .ForMember(dest => dest.TotalNumberOfConnectors, opt => opt.MapFrom(src => src.TotalNumberOfConnectors))
                 .ForMember(dest => dest.Distance, opt => opt.Ignore()) // Distance will be calculated separately
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Domain.DTOs.Address
                 {
                     Street = src.Street,
                     City = src.City,
                     Country = src.Country
                 }))
                 .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => new Contacts
                 {
                     Phone = src.AdminPhone,
                     Website = src.Website
                 }))
                 .ForMember(dest => dest.Position, opt => opt.MapFrom(src => new Position
                 {
                     Latitude = src.Latitude,
                     Longitude = src.Longitude
                 }))
                 .ForMember(dest => dest.ConnectorDetails, opt => opt.MapFrom(src => src.ConnectorDetail.Select(detail => new ConnectorDetailDto
                 {
                     ID = detail.Id,
                     SupplierName = detail.SupplierName,
                     ConnectorType = detail.ConnectorType,
                     ChargeCapacity = detail.ChargeCapacity,
                     MaxPowerLevel = detail.MaxPowerLevel,
                     CustomerChargeLevel = detail.CustomerChargeLevel,
                     CustomerConnectorName = detail.CustomerConnectorName,
                     Price = detail.Price,
                     ConnectorsStatuses = detail.ConnectorStatuses.Select(status => new ConnectorStatusDto
                     {
                         State = status.State,
                         PhysicalReference = status.PhysicalReference
                     }).ToList()
                 }).ToList()))
                 .ForMember(dest => dest.PaymentMethods, opt => opt.MapFrom(src => new PaymentMethodDTO
                 {
                     EPayment = new PaymentType
                     {
                         Accept = src.PaymentMethod.EPaymentAccept,
                         Types = src.PaymentMethod.EPaymentTypes != null ? new PaymentTypes { Type = SplitPaymentTypes(src.PaymentMethod.EPaymentTypes) } 
                                    : new PaymentTypes { Type = new List<string>() }
                     },
                     Other = new PaymentType
                     {
                         Accept = src.PaymentMethod.OtherPaymentAccept.GetValueOrDefault(),
                         Types = string.IsNullOrEmpty(src.PaymentMethod.OtherPaymentTypes) ? new PaymentTypes { Type = new List<string>() } 
                                : new PaymentTypes { Type = SplitPaymentTypes(src.PaymentMethod.OtherPaymentTypes) }

                     }
                 }));

            CreateMap<AddEVStationDTO, EVStation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.AdminPhone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL));
                
            CreateMap<PaymentMethodDTO, Domain.Entities.PaymentMethod>()
                 .ForMember(dest => dest.EPaymentAccept, opt => opt.MapFrom(src => src.EPayment.Accept))
                .ForMember(dest => dest.OtherPaymentAccept, opt => opt.MapFrom(src => src.Other.Accept))
                .ForMember(dest => dest.EPaymentTypes, opt => opt.MapFrom(src => string.Join(", ", src.EPayment.Types.Type)))
                .ForMember(dest => dest.OtherPaymentTypes, opt => opt.MapFrom(src => string.Join(", ", src.Other.Types.Type)));

            CreateMap<AddConnectorDetailDTO, ConnectorDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
                .ForMember(dest => dest.ConnectorType, opt => opt.MapFrom(src => src.ConnectorType))
                .ForMember(dest => dest.ChargeCapacity, opt => opt.MapFrom(src => src.ChargeCapacity))
                .ForMember(dest => dest.MaxPowerLevel, opt => opt.MapFrom(src => src.MaxPowerLevel))
                .ForMember(dest => dest.CustomerChargeLevel, opt => opt.MapFrom(src => src.CustomerChargeLevel))
                .ForMember(dest => dest.CustomerConnectorName, opt => opt.MapFrom(src => src.CustomerConnectorName));

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

        private List<string> SplitPaymentTypes(string types)
        {
            return types.Split(',').ToList();
        }

    }
}
