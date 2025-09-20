using DashboardAgro.Application.DTOs;
using MediatR;

namespace DashboardAgro.Application.Queries
{
    public class ListarUFsQuery : IRequest<List<UnidadeFederativaDTO>>
    {
        public int? Id { get; set; }
        public ListarUFsQuery(int? id) => Id = id;
    }
}
