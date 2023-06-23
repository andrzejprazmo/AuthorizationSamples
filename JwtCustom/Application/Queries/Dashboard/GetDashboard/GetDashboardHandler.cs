using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using JwtCustom.Application.Common.Interfaces;

namespace JwtCustom.Application.Queries.Dashboard.GetDashboard
{
    public class GetDashboardHandler : IRequestHandler<GetDashboardRequest, GetDashboardResponse>
    {
        private readonly ICarRepository carRepository;

        public GetDashboardHandler(ICarRepository carRepository)
        {
            this.carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
        }

        public async Task<GetDashboardResponse> Handle(GetDashboardRequest request, CancellationToken cancellationToken)
        {
            var carList = await carRepository.GetList();
            return new GetDashboardResponse
            {
                Items = carList.Select(c=>c.Name).ToArray(),
            };
        }
    }
}
