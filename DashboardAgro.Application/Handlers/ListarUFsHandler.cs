using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using DashboardAgro.Application.Queries;
using MediatR;

namespace DashboardAgro.Application.Handlers
{
    public class ListarUFsHandler : IRequestHandler<ListarUFsQuery, List<UnidadeFederativaDTO>>
    {
        private readonly IParametrosRepository _repository;
        public ListarUFsHandler(IParametrosRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UnidadeFederativaDTO>> Handle(ListarUFsQuery request, CancellationToken cancellationToken)
        {
            var queryRegiao = await _repository.GetAllUnidadesFederativas(request.Id);

            return [.. queryRegiao.OrderBy(r => r.NomeUF).Select(r => new UnidadeFederativaDTO
            {
                Id = r.Id,
                SiglaUF = r.SiglaUF,
                NomeUF = r.NomeUF,
                Regiao = new RegiaoDTO { Id = r.Regiao.Id, Descricao = r.Regiao.Descricao }
            })];
        }
    }
}
