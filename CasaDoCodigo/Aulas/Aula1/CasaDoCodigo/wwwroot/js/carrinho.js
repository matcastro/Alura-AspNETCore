class Carrinho {
    clickIncremento(btn) {
        var data = this.getData(btn);
        data.quantidade++;
        this.postQuantidade(data);
    }

    clickDecremento(btn) {
        var data = this.getData(btn);
        data.quantidade--;
        this.postQuantidade(data);
    }

    updateQuantidade(input) {
        var data = this.getData(input);
        this.postQuantidade(data);
    }

    getData(elemento) {
        var linhaProduto = $(elemento).parents('[item-id]');
        var itemId = $(linhaProduto).attr('item-id');
        var novaQtd = $(linhaProduto).find('input').val();
        return {
            id: itemId,
            quantidade: novaQtd
        }
    }

    postQuantidade(data) {
        let token = $('[name=__RequestVerificationToken]').val();
        let headers = {};
        headers['RequestVerificationToken'] = token;

        $.ajax({
            url: '/pedido/updatequantidade',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: headers
        })
            .done(function (response) {
                let itemPedido = response.itemPedido;
                let carrinhoViewModel = response.carrinhoViewModel;
                let linhaDoPedido = $('[item-id=' + itemPedido.id + ']');
                linhaDoPedido.find('input').val(itemPedido.quantidade);
                if (itemPedido.quantidade == 0) {
                    linhaDoPedido.remove();
                }
                linhaDoPedido.find('[subtotal]').html((itemPedido.subtotal).duasCasas());
                $('[numero-itens]').html('Total: ' + carrinhoViewModel.itens.length + ' itens');
                $('[total]').html((carrinhoViewModel.total).duasCasas());
            })
    }
}

var carrinho = new Carrinho();

Number.prototype.duasCasas = function () {
    return this.toFixed(2).replace('.', ',');
}
