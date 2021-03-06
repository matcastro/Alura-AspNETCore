﻿using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IItemPedidoRepository itemPedidoRepository;
        private readonly ICadastroRepository cadastroRepository;
        public PedidoRepository(ApplicationContext contexto, IHttpContextAccessor contextAccessor,
            IItemPedidoRepository itemPedidoRepository, ICadastroRepository cadastroRepository) : base(contexto)
        {
            this.contextAccessor = contextAccessor;
            this.itemPedidoRepository = itemPedidoRepository;
            this.cadastroRepository = cadastroRepository;
        }

        public void AddItem(string codigo)
        {
            var produto = contexto.Set<Produto>()
                .Where(p => p.Codigo == codigo)
                .SingleOrDefault();
            if (produto == null)
            {
                throw new ArgumentException($"Produto {codigo} não foi encontrado.");
            }

            var pedido = GetPedido();
            var itemPedido = contexto.Set<ItemPedido>()
                .Where(i => i.Produto.Codigo == codigo
                && i.Pedido.Id == pedido.Id).SingleOrDefault();
            if (itemPedido == null)
            {
                itemPedido = new ItemPedido(pedido, produto, 1, produto.Preco);
                contexto.Set<ItemPedido>()
                    .Add(itemPedido);
                contexto.SaveChanges();
            }
        }

        public Pedido GetPedido()
        {
            var pedidoId = GetPedidoId();
            var pedido = dbSet
                .Include(p => p.Itens)
                    .ThenInclude(p => p.Produto)
                .Include(p => p.Cadastro)
                .Where(p => p.Id == pedidoId)
                .SingleOrDefault();

            if (pedido == null)
            {
                pedido = new Pedido();
                dbSet.Add(pedido);
                contexto.SaveChanges();
                SetPedidoId(pedido.Id);
            }
            return pedido;
        }

        public int? GetPedidoId()
        {
            return contextAccessor.HttpContext.Session.GetInt32("pedidoId");
        }

        public void SetPedidoId(int pedidoId)
        {
            contextAccessor.HttpContext.Session.SetInt32("pedidoId", pedidoId);
        }

        public Pedido UpdateCadastro(Cadastro cadastro)
        {
            var pedido = GetPedido();
            cadastroRepository.UpdateCadastro(pedido.Cadastro.Id, cadastro);
            return pedido;
        }

        public UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido)
        {
            var itemPedidoDB = itemPedidoRepository.GetItemPedido(itemPedido.Id);

            if (itemPedidoDB != null)
            {
                if (itemPedidoDB.Quantidade != itemPedido.Quantidade)
                {
                    itemPedidoDB.AtualizaQuantidade(itemPedido.Quantidade);
                    if(itemPedido.Quantidade == 0)
                    {
                        itemPedidoRepository.RemoveItemPedido(itemPedido.Id);
                    }
                    contexto.SaveChanges();

                    var carrinhoViewModel = new CarrinhoViewModel(GetPedido().Itens);
                    return new UpdateQuantidadeResponse(itemPedidoDB, carrinhoViewModel);
                }
            }
            throw new ArgumentException($"{nameof(itemPedido)} não encontrado!");
        }

    }
}
