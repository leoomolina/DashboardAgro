using DashboardAgro.Application.DTOs;
using MediatR;

namespace DashboardAgro.Application.Queries
{
    public class ListarProducoesAsyncQuery : IRequest<IEnumerable<ProducaoDTO>>
    {
    }
}
