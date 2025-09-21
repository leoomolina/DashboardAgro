using DashboardAgro.Application.DTOs;
using MediatR;

namespace DashboardAgro.Application.Queries
{
    public class ListarUFsAsyncQuery : IRequest<List<UnidadeFederativaDTO>>
    {
        public int? Id { get; set; }
        public ListarUFsAsyncQuery(int? id) => Id = id;
    }
}
