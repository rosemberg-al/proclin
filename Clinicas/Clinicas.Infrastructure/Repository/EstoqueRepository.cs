using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Clinicas.Infrastructure.Repository
{
    public class EstoqueRepository : RepositoryBase<ClinicasContext>, IEstoqueRepository
    {
        public EstoqueRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }

        public void ExcluirMaterial(Material material)
        {
            try
            {
               material.SetSituacao("Excluido");
               Context.Entry(material).State = EntityState.Modified;
               Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void ExcluirMovimentoEstoque(MovimentoEstoque mov)
        {
            try
            {
                mov.SetSituacao("Excluido");
                Context.Entry(mov).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void ExcluirTipoMaterial(TipoMaterial tipo)
        {
            try
            {
                tipo.SetSituacao("Excluido");
                Context.Entry(tipo).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public ICollection<Material> ListaMateriais(int idunidade)
        {
            return Context.Material.Where(x => x.Situacao=="Ativo" && x.IdUnidadeAtendimento == idunidade).ToList();
        }

        public ICollection<MovimentoEstoque> ListarMovimentoEstoque(int idunidade)
        {
            return Context.MovimentoEstoque.Include(x=>x.Material).Where(x => x.Situacao == "Ativo" && x.IdUnidadeAtendimento == idunidade).ToList();
        }

        public ICollection<TipoMaterial> ListaTipoMateriais(int idunidade)
        {
            return Context.TipoMaterial.Where(x => x.Situacao == "Ativo" && x.IdUnidadeAtendimento==idunidade).ToList();
        }

        public Material ObterMaterialPorId(int idmaterial)
        {
            return Context.Material.Include(x=>x.UnidadeAtendimento).Include(x=>x.TipoMaterial).FirstOrDefault(x => x.IdMaterial == idmaterial);
        }

        public MovimentoEstoque ObterMovimentoEstoquePorId(int idmovimentoestoque)
        {
            return Context.MovimentoEstoque.Include(x => x.Material).FirstOrDefault(x => x.IdMovimentoEstoque == idmovimentoestoque);
        }

        public TipoMaterial ObterTipoMaterialPorId(int idtipomaterial)
        {
            return Context.TipoMaterial.FirstOrDefault(x => x.IdTipoMaterial == idtipomaterial);
        }

        public Material SalvarMaterial(Material material)
        {
            try
            {
                if (material.IdMaterial > 0)
                {
                    Context.Entry(material).State = EntityState.Modified;
                }
                else
                {
                    Context.Material.Add(material);
                }
                Context.SaveChanges();
                return material;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public MovimentoEstoque SalvarMovimentoEstoque(MovimentoEstoque movimento)
        {
            try
            {
                if (movimento.IdMaterial > 0)
                {
                    Context.Entry(movimento).State = EntityState.Modified;
                }
                else
                {
                    Context.MovimentoEstoque.Add(movimento);
                }
                Context.SaveChanges();
                return movimento;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public TipoMaterial SalvarTipoMaterial(TipoMaterial tipomaterial)
        {
            try
            {
                if (tipomaterial.IdTipoMaterial > 0)
                {
                    Context.Entry(tipomaterial).State = EntityState.Modified;
                }
                else
                {
                    Context.TipoMaterial.Add(tipomaterial);
                }
                Context.SaveChanges();
                return tipomaterial;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }



    }
}