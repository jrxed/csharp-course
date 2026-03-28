using System;
using System.Collections.Generic;

namespace CarApp
{
    public enum CarType
    {
        Unknown,
        Tesla,
        Toyota,
        BMW,
        Lada,
        Mercedes
    }

    public interface ICar
    {
        string GetDescription();
    }

    public interface IElectric
    {
        string GetPowerSource();
    }

    public interface IMechanical
    {
        string GetPowerSource();
    }

    public interface IMechanicalTransmission
    {
        string GetTransmission();
    }

    public interface IAutomaticTransmission
    {
        string GetTransmission();
    }

    public interface IInfotainment
    {
        string GetInfotainmentSystem();
    }

    public abstract class ACar : ICar
    {
        protected readonly string _brand;
        protected readonly int _seats;
        protected readonly int _doors;

        protected ACar(string brand, int seats, int doors = 4)
        {
            _brand = brand;
            _seats = seats;
            _doors = doors;
        }

        public virtual string GetBrand() => _brand;
        public virtual string GetSeatsInfo() => $"{_seats} местами";
        public virtual string GetDoorsInfo() => $"{_doors} дверьми";
        public virtual string GetSafetyFeatures() => "Подушки безопасности, ABS, ESP";
        public virtual string GetWarrantyInfo() => "3 года / 100 000 км";
        public virtual string GetAdditionalFeatures() => "";
        public virtual int GetHorsePower() => 150;

        protected virtual string FormatDescription(string power, string transmission, string infotainment = "")
        {
            var extra = string.IsNullOrEmpty(infotainment) ? "" : $", {infotainment}";
            var safety = $"безопасность: {GetSafetyFeatures()}";
            return $"> {GetBrand()}: {power} с {transmission}, {GetSeatsInfo()}, {GetDoorsInfo()}{extra}, {safety}";
        }

        public virtual string GetDescription()
        {
            return "> описание авто отсутствует";
        }
    }

    public abstract class AutomaticCar : ACar, IAutomaticTransmission
    {
        protected AutomaticCar(string brand, int seats, int doors = 4)
            : base(brand, seats, doors) { }

        public virtual string GetTransmission() => "автоматической коробкой передач";

        protected override string FormatDescription(string power, string transmission, string infotainment = "")
        {
            return base.FormatDescription(power, GetTransmission(), infotainment);
        }
    }

    public abstract class ManualCar : ACar, IMechanicalTransmission
    {
        protected ManualCar(string brand, int seats, int doors = 4)
            : base(brand, seats, doors) { }

        public virtual string GetTransmission() => "механической коробкой передач";

        protected override string FormatDescription(string power, string transmission, string infotainment = "")
        {
            return base.FormatDescription(power, GetTransmission(), infotainment);
        }
    }

    public class TeslaCar : AutomaticCar, IElectric, IInfotainment
    {
        public TeslaCar() : base("Tesla", 5) { }

        public string GetPowerSource() => "электрический автомобиль";
        public string GetInfotainmentSystem() => "Android + автопилот";

        public override string GetDescription()
        {
            return FormatDescription(GetPowerSource(), GetTransmission(), GetInfotainmentSystem());
        }

        public override int GetHorsePower() => 350;
    }

    public class ToyotaCar : AutomaticCar, IMechanical, IInfotainment
    {
        public ToyotaCar() : base("Toyota", 5) { }

        public string GetPowerSource() => "механический (гибридный) автомобиль";
        public string GetInfotainmentSystem() => "Toyota Safety Sense + Android";

        public override string GetDescription()
        {
            return FormatDescription(GetPowerSource(), GetTransmission(), GetInfotainmentSystem());
        }
    }

    public class BMWCar : AutomaticCar, IMechanical, IInfotainment
    {
        public BMWCar() : base("BMW", 5) { }

        public string GetPowerSource() => "механический автомобиль";
        public string GetInfotainmentSystem() => "iDrive 8 + управление жестами";

        public override string GetDescription()
        {
            return FormatDescription(GetPowerSource(), GetTransmission(), GetInfotainmentSystem());
        }

        public override int GetHorsePower() => 258;
    }

    public class LadaCar : ManualCar, IMechanical
    {
        public LadaCar() : base("Lada", 5) { }

        public string GetPowerSource() => "механический автомобиль";
        public override string GetAdditionalFeatures() => "Классика российского автопрома";

        public override string GetDescription()
        {
            var baseDesc = FormatDescription(GetPowerSource(), GetTransmission());
            return baseDesc + $", {GetAdditionalFeatures()}";
        }
    }

    public class MercedesCar : AutomaticCar, IMechanical, IInfotainment
    {
        public MercedesCar() : base("Mercedes", 5, 4) { }

        public string GetPowerSource() => "механический (премиальный) автомобиль";
        public string GetInfotainmentSystem() => "MBUX + Augmented Reality";

        public override string GetDescription()
        {
            return FormatDescription(GetPowerSource(), GetTransmission(), GetInfotainmentSystem());
        }

        public override string GetWarrantyInfo() => "5 лет / 150 000 км";
    }

    public class UnknownCar : ACar
    {
        public UnknownCar() : base("Неизвестная марка", 0) { }

        public override string GetDescription()
        {
            return "«описание авто»";
        }
    }

    public class CarFactory
    {
        private readonly Dictionary<CarType, Func<ICar>> _carCreators;

        public CarFactory()
        {
            _carCreators = new Dictionary<CarType, Func<ICar>>
            {
                { CarType.Tesla, () => new TeslaCar() },
                { CarType.Toyota, () => new ToyotaCar() },
                { CarType.BMW, () => new BMWCar() },
                { CarType.Lada, () => new LadaCar() },
                { CarType.Mercedes, () => new MercedesCar() }
            };
        }

        public ICar GetCar(CarType type)
        {
            return _carCreators.TryGetValue(type, out var creator)
                ? creator()
                : new UnknownCar();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var factory = new CarFactory();

            Console.WriteLine("===  Консольный автомобильный справочник ===");
            while (true)
            {
                Console.WriteLine("\nВведите марку автомобиля или 'done' для остановки:");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (input.ToLower() == "done")
                {
                    break;
                }

                CarType carType = input.ToLower() switch
                {
                    "тесла" => CarType.Tesla,
                    "илон маск" => CarType.Tesla,
                    "тойота" => CarType.Toyota,
                    "бмв" => CarType.BMW,
                    "бэха" => CarType.BMW,
                    "лада" => CarType.Lada,
                    "мерс" => CarType.Mercedes,
                    "мерседес" => CarType.Mercedes,
                    _ => CarType.Unknown
                };

                ICar car = factory.GetCar(carType);
                Console.WriteLine(car.GetDescription());

                if (car is ACar aCar && !(car is UnknownCar))
                {
                    Console.WriteLine($"   > Мощность: {aCar.GetHorsePower()} л.с.");
                    Console.WriteLine($"   > Гарантия: {aCar.GetWarrantyInfo()}");
                }
            }
        }
    }
}
