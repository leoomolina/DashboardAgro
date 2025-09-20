using MediatR;

namespace DashboardAgro.Application.Queries
{
    public class ListarAnosDisponiveisAsyncQuery : IRequest<IEnumerable<int>>
    {
    }
}
