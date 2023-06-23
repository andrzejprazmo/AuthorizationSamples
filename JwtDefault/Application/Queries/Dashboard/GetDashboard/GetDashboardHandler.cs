using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;

namespace JwtDefault.Application.Queries.Dashboard.GetDashboard
{
    public class GetDashboardHandler : IRequestHandler<GetDashboardRequest, GetDashboardResponse>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetDashboardHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetDashboardResponse> Handle(GetDashboardRequest request, CancellationToken cancellationToken)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var accessToken = await httpContext!.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token");
            return new GetDashboardResponse
            {
                Items = new string[]
                {
                    "Jan", "Adam", "Barnaba"
                }
            };
        }
    }
}
