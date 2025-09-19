using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using System;
using System.Collections.Generic;
namespace DashboardAgro.Application.UseCases.LavouraPermanente
{
    public class GetDetalhesAnoUseCase
    {
        private readonly ILavouraPermanenteRepository _repository;

        public GetDetalhesAnoUseCase(ILavouraPermanenteRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ResumoAnoDTO>> ExecuteAsync(int ano)
        {
            var query = _repository.GetResumoAnualLavouraPermanenteAsync(ano).Result;

            return query;
        }
    }
}
