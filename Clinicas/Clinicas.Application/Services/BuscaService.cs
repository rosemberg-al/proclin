using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.DTO;
using Clinicas.Domain.ViewModel;
using Clinicas.Domain.Mail;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Net.Mail;

namespace Clinicas.Application.Services
{
    public class BuscaService : IBuscaService
    {
        private readonly IBuscaRepository _repository;

        public BuscaService(IBuscaRepository repository)
        {
            _repository = repository;
        }

        public ICollection<BuscaViewModel> Busca(string search)
        {
            return _repository.Busca(search);
        }
    }
}