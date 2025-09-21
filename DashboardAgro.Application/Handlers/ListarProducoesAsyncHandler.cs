using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using DashboardAgro.Application.Queries;
using MediatR;

namespace DashboardAgro.Application.Handlers
{
    public class ListarProducoesAsyncHandler : IRequestHandler<ListarProducoesAsyncQuery, IEnumerable<ProducaoDTO>>
    {
        private readonly IParametrosRepository _repository;

        public ListarProducoesAsyncHandler(IParametrosRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProducaoDTO>> Handle(ListarProducoesAsyncQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllProducoes()
            .ContinueWith(t => t.Result.Select(p => new ProducaoDTO
            {
                Id = p.Id,
                Descricao = p.Descricao,
                TipoLavoura = p.TipoLavoura
            }).ToList());
        }
    }
}
