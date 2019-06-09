using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class CadastroRepository : BaseRepository<Cadastro>, ICadastroRepository
    {
        public CadastroRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public Cadastro UpdateCadastro(int cadastroId, Cadastro novoCadastro)
        {
            Cadastro cadastroDb = dbSet.Where(c => c.Id == cadastroId).SingleOrDefault();
            if(cadastroDb == null)
            {
                throw new ArgumentNullException(nameof(novoCadastro));
            }
            cadastroDb.Update(novoCadastro);
            contexto.SaveChanges();
            return cadastroDb;
        }
    }
}
