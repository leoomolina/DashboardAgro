using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using DashboardAgro.Application.Queries;
using MediatR;

namespace DashboardAgro.Application.Handlers
{
    public class ListarLavourasPermanentesHandler : IRequestHandler<ListarLavourasPermanentesQuery, IEnumerable<LavouraDTO>>
    {
        private readonly ILavouraPermanenteRepository _repository;

        public ListarLavourasPermanentesHandler(ILavouraPermanenteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LavouraDTO>> Handle(ListarLavourasPermanentesQuery request, CancellationToken cancellationToken)
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
