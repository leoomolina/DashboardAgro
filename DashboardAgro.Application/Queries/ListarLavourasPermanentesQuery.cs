using DashboardAgro.Application.DTOs;
using MediatR;

namespace DashboardAgro.Application.Queries
{
    public class ListarLavourasPermanentesQuery : IRequest<IEnumerable<LavouraDTO>>
    {
    }
}
