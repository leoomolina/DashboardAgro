using DashboardAgro.Application.DTOs;
using MediatR;

namespace DashboardAgro.Application.Queries
{
    public class ListarRegioesQuery : IRequest<List<RegiaoDTO>>
    {
    }
}
