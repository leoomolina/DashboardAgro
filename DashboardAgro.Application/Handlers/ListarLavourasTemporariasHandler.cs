using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using DashboardAgro.Application.Queries;
using MediatR;

namespace DashboardAgro.Application.Handlers
{
    public class ListarLavourasTemporariasHandler : IRequestHandler<ListarLavourasTemporariasQuery, IEnumerable<LavouraDTO>>
    {
        private readonly ILavouraTemporariaRepository _repository;

        public ListarLavourasTemporariasHandler(ILavouraTemporariaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LavouraDTO>> Handle(ListarLavourasTemporariasQuery request, CancellationToken cancellationToken)
        {
            var lavouras = await _repository.GetAllAsync();
            return lavouras.Select(l => new LavouraDTO
            {
                Id = l.IdProducao,
                Descricao = l.Producao
            });
        }
    }
}
