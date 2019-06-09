﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Models.ViewModels
{
    public class CarrinhoViewModel
    {
        public IList<ItemPedido> Itens { get; }
        public decimal Total { get; }

        public CarrinhoViewModel(IList<ItemPedido> itens)
        {
            Itens = itens;
            Total = Itens.Sum(i => i.PrecoUnitario * i.Quantidade );
        }
    }
}
