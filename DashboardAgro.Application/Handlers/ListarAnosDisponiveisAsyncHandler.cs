using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.Queries;
using MediatR;

namespace DashboardAgro.Application.Handlers
{
    public class ListarAnosDisponiveisAsyncHandler : IRequestHandler<ListarAnosDisponiveisAsyncQuery, IEnumerable<int>>
    {
        private readonly IImportarDados _repo;

        public ListarAnosDisponiveisAsyncHandler(IImportarDados repo) => _repo = repo;

        public async Task<IEnumerable<int>> Handle(ListarAnosDisponiveisAsyncQuery request, CancellationToken cancellationToken)
            => await _repo.GetAnosDisponiveisAsync();
    }
}
