using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using DashboardAgro.Application.Queries;
using MediatR;

namespace DashboardAgro.Application.Handlers
{
    public class ListarRegioesHandler : IRequestHandler<ListarRegioesQuery, List<RegiaoDTO>>
    {
        private readonly IParametrosRepository _repository;
        public ListarRegioesHandler(IParametrosRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RegiaoDTO>> Handle(ListarRegioesQuery request, CancellationToken cancellationToken)
        {
            var queryRegiao = await _repository.GetAllRegiao();

            return [.. queryRegiao.Select(r => new RegiaoDTO
            {
                Id = r.Id,
                Descricao = r.Descricao
            })]; ;
        }
    }
}
