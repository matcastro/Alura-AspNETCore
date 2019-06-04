using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoRepository pedidoRepository;

        public PedidoController(IProdutoRepository produtoRepository, IPedidoRepository pedidoRepository)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
        }

        public IActionResult Cadastro()
        {
            return View();
        }
        public IActionResult Carrinho(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                pedidoRepository.AddItem(codigo);
            }
            var pedido = pedidoRepository.GetPedido();
            return View(pedido.Itens);
        }
        public IActionResult Carrossel()
        {
            return View(produtoRepository.GetProdutos());
        }
        public IActionResult Resumo()
        {
            var pedido = pedidoRepository.GetPedido();
            return View(pedido);
        }
    }
}
