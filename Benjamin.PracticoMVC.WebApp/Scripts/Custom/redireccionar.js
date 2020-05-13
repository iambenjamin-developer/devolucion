$.get("/Pedidos/MiPedidoPendiente/", function (data) {

    if (data != 0) {

        location.href = '/Pedidos/Carrito/?idPedido=' + data.toString();

    }

});