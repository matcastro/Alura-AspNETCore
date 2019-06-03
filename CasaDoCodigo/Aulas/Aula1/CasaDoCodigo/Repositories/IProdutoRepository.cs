﻿using CasaDoCodigo.Models;
using System.Collections.Generic;

namespace CasaDoCodigo.Repositories
{
    public interface IProdutoRepository
    {
        void SaveLivros(List<Livro> livros);
        IList<Produto> GetProdutos();
    }
}