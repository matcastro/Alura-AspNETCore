class Carrinho {
    clickIncremento(btn) {
        var data = this.getData(btn);
        data.quantidade++;
        console.log(data);
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
        $.ajax({
            url: '/pedido/updatequantidade',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data)
        })
    }
}

var carrinho = new Carrinho();