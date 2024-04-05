using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            //9. Import Suppliers
            //string inputSuppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, inputSuppliersXml));

            //10. Import parts
            //string inputPartsXml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context,inputPartsXml));

            //11. Import Cars
            //string inputCarsXml = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, inputCarsXml));

            //12.Import Customers
            //string inputCustomersXml = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, inputCustomersXml));

            //13. Import Sales
            //string inputSalesXml = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, inputSalesXml));


            //14. Export Cars With Distance
            //Console.WriteLine(GetCarsWithDistance(context));

            //18. Export Total Sales by Customer
            Console.WriteLine(GetTotalSalesByCustomer(context));

        }

        //How to configure Automapper in Method
        private static Mapper GetMapper()
        {
            var cfg = new MapperConfiguration(c => c.AddProfile<CarDealerProfile>());
            return new Mapper(cfg);
        }

        //9. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            //1. Create xml serializer
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDTO[]), new XmlRootAttribute("Suppliers"));

            //2. Deserializing
            using var reader = new StringReader(inputXml);

            ImportSupplierDTO[] importSupplierDTOs = xmlSerializer.Deserialize(reader) as ImportSupplierDTO[];

            //3.We have to Map suppliers
            var mapper = GetMapper();
            Supplier[] suppliers = mapper.Map<Supplier[]>(importSupplierDTOs);

            //4. Add to EF context
            context.AddRange(suppliers);

            //5. Comming changes to DB
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        //10. Import parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartsDTO[]), new XmlRootAttribute("Parts"));

            using StringReader inputReader = new(inputXml);

            var mapper = GetMapper();
            ImportPartsDTO[] importPartsDTOs = xmlSerializer.Deserialize(inputReader) as ImportPartsDTO[];

            var supplierIds = context.Suppliers
                .Select(x => x.Id)
                .ToArray();

            Part[] parts = mapper.Map<Part[]>(importPartsDTOs.Where(p => supplierIds.Contains(p.SupplierId)));

            context.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";
        }

        //11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCarDTO[]), new XmlRootAttribute("Cars"));

            using StringReader stringReader = new(inputXml);

            ImportCarDTO[] importCarDTOs = xmlSerializer.Deserialize(stringReader) as ImportCarDTO[];

            var mapper = GetMapper();
            List<Car> cars = new List<Car>();

            foreach (var carDTO in importCarDTOs)
            {
                Car car = mapper.Map<Car>(carDTO);

                int[] carPartIds = carDTO.PartsId
                    .Select(x => x.Id)
                    .Distinct()
                    .ToArray();

                var carParts = new List<PartCar>();
                foreach (var id in carPartIds)
                {
                    carParts.Add(new PartCar
                    {
                        Car = car,
                        PartId = id

                    });
                }

                car.PartsCars = carParts;
                cars.Add(car);
            }

            context.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomerDTO[]), new XmlRootAttribute("Customers"));

            using StringReader stringReader = new(inputXml);

            ImportCustomerDTO[] importCustomerDTOs = xmlSerializer.Deserialize(stringReader) as ImportCustomerDTO[];

            var mapper = GetMapper();

            Customer[] customers = mapper.Map<Customer[]>(importCustomerDTOs);

            context.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }

        //13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSalesDTO[]), new XmlRootAttribute("Sales"));

            using StringReader inputReader = new StringReader(inputXml);

            ImportSalesDTO[] importSalesDTOs = xmlSerializer.Deserialize(inputReader) as ImportSalesDTO[];

            var mapper = GetMapper();

            //Have to take CarIds
            int[] carIds = context.Cars
                .Select(car => car.Id)
                .ToArray();

            Sale[] sales = mapper.Map<Sale[]>(importSalesDTOs)
                .Where(s => carIds.Contains(s.CarId))
                .ToArray();

            context.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}";

        }

        //14. Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var mapper = GetMapper();

            var carsWithDistance = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                    .ThenBy(c => c.Model)
                .ProjectTo<ExportCarsWithDistance>(mapper.ConfigurationProvider)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarsWithDistance[]), new XmlRootAttribute("cars"));

            //How to remove the whitespaces in the generated xml
            var xsn = new XmlSerializerNamespaces();
            xsn.Add(string.Empty, string.Empty);

            StringBuilder sb = new();

            using (StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, carsWithDistance, xsn);
            }

            return sb.ToString().Trim();
        }

        //18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var totalSales = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new SalesPerCustomerDTO
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count(),
                    SpentMoney = c.Sales.Sum(s =>
                        s.Car.PartsCars.Sum(pc =>
                                    Math.Round(c.IsYoungDriver ? pc.Part.Price * 0.95m : pc.Part.Price)
                              )
                        ).ToString("f2")
                })
                .OrderByDescending(x => x.SpentMoney)
                .ToArray();

            return SerializeToXml<SalesPerCustomerDTO[]>(totalSales, "customers");

        }

        //Generic method to serialize DTOs to XML
        private static string SerializeToXml<T>(T dto, string xmlRootAttribute)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttribute));

            StringBuilder stringBUilder = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(stringBUilder, CultureInfo.InvariantCulture))
            {
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);

                try
                {
                    xmlSerializer.Serialize(stringWriter, dto, xmlSerializerNamespaces);
                }
                catch (Exception)
                {

                    throw;
                }

                return stringBUilder.ToString();
            }
        }


    }
}