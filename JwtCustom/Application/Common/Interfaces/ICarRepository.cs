using JwtCustom.Domain.Car;

namespace JwtCustom.Application.Common.Interfaces
{
    public interface ICarRepository
    {
        Task<Guid> Create(Car car);
        Task<IEnumerable<Car>> GetList();
    }
}
