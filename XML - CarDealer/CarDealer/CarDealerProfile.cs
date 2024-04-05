using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<ImportSupplierDTO, Supplier>();
            CreateMap<ImportPartsDTO, Part>();
            CreateMap<ImportCarDTO, Car>();
            CreateMap<ImportCustomerDTO, Customer>();
            CreateMap<ImportSalesDTO, Sale>();
            CreateMap<Car,ExportCarsWithDistance>();
            CreateMap<Customer,SalesPerCustomerDTO>();

        }
    }
}
