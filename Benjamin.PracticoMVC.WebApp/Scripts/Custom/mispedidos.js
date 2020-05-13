

//mostramos los pedidos que tiene el cliente logueado
tablaMisPedidos();

function tablaMisPedidos() {

    $.get("/Pedidos/JsonMisPedidos/", function (data) {

        var total = 0;
        var contenido = "";
        var idPedido = 0;

        contenido += "<table id='tabla-paginacion-usuarios' class='table table-striped'>";
        contenido += "<thead>";
        contenido += "<tr>";
        contenido += "<th scope='col'>NRO PEDIDO</th>";
        contenido += "<th scope='col' class='text-center'>FECHA</th>";
        contenido += "<th scope='col' class='text-center'>ESTADO</th>";
        contenido += "<th scope='col' class='text-center' >&nbsp;&nbsp;&nbsp;&nbsp;OBSERVACIONES</th>";
        contenido += "<th scope='col' class='text-center'>DETALLES</th>";
        contenido += "</tr>";
        contenido += "</thead>";
        contenido += "<tbody>";

        for (var i = 0; i < data.length; i++) {
            contenido += "<tr>";
            contenido += "<td>&nbsp;&nbsp;" + data[i].ID_PEDIDO + "</td>";
            idPedido = parseInt(data[i].ID_PEDIDO);

            contenido += "<td class='text-center'>" + parsearFecha(data[i].FECHA_PEDIDO) + "</td>";

            contenido += "<td class='text-center'>" + parsearEstado(data[i].ESTADO_PEDIDO) + "</td>";

            contenido += "<td class='text-center'>" + data[i].OBSERVACIONES + "</td>";

            contenido += "<td class='text-center' ><button id='btnDetalles' class='btn btn-primary' onclick='detallePedido(" + idPedido + ")' ><i class='fas fa-info-circle'></i></button></td>";

            contenido += "</tr>";
        }

        contenido += "</tbody>";

        contenido += "</table>";


        document.getElementById("tabla-mispedidos").innerHTML = contenido;



    });

}

function parsearFecha(fecha) {


    if (fecha != null) {

        moment.locale("es");

        return moment(fecha).format('L');

    } else {

        return "No Aplica";
    }

}

function detallePedido(idPedido) {

    location.href = '/Pedidos/Carrito/?idPedido=' + idPedido;
}


function parsearMoneda(decimal) {

    return new Intl.NumberFormat("es-AR", { style: "currency", currency: "ARS" }).format(decimal);
}

function parsearEstado(cadena) {

    var resultado = "";
    switch (cadena) {
        case "(P)":
            resultado = "PENDIENTE";
            break;

        case "(C)":
            resultado = "CANCELADO";
            break;

        case "(F)":
            resultado = "FINALIZADO";
            break;

        default:
            resultado = "DESCONOCIDO";
    }

    return resultado;
}
