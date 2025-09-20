using DashboardAgro.Application.DTOs;
using MediatR;

namespace DashboardAgro.Application.Queries
{
    public class ListarLavourasTemporariasQuery : IRequest<IEnumerable<LavouraDTO>>
    {
    }
}
