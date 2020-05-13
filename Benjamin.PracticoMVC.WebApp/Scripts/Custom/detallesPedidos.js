var idPedido = document.getElementById("txtIdPedido").value;
var idCliente = document.getElementById("txtIdCliente").value;

rellenarDatos();

rellenarTabla();

function rellenarTabla() {
    var ruta = "/Pedidos/JsonDetallesPedidos/?idPedido=";
    ruta += idPedido.toString();
    ruta += "&idCliente=";
    ruta += idCliente.toString();


    $.get(ruta, function (data) {


        var total = 0;
        var contenido = "";


        contenido += "<table id='tabla-paginacion-usuarios' class='table table-striped'>";
        contenido += "<thead>";
        contenido += "<tr>";
        contenido += "<th scope='col'>ITEM</th>";
        contenido += "<th scope='col' class='text-center'>CODIGO</th>";
        contenido += "<th scope='col' class='text-center'>PRODUCTO</th>";
        contenido += "<th scope='col' class='text-right' >&nbsp;&nbsp;&nbsp;&nbsp;PRECIO UNITARIO</th>";
        contenido += "<th scope='col' class='text-center' >&nbsp;&nbsp;&nbsp;&nbsp;CANTIDAD</th>";
        contenido += "<th scope='col' class='text-right' >&nbsp;&nbsp;&nbsp;&nbsp;SUBTOTAL</th>";
        contenido += "</tr>";
        contenido += "</thead>";
        contenido += "<tbody>";

        for (var i = 0; i < data.length; i++) {

            contenido += "<tr>";
            contenido += "<td>&nbsp;&nbsp;" + data[i].ITEM + "</td>";
            contenido += "<td class='text-center' >" + data[i].ID_PRODUCTO + "</td>";
            codProducto = parseInt(data[i].ID_PRODUCTO);
            contenido += "<td class='text-center' >" + data[i].MARCA + " - " + data[i].PRODUCTO + "</td>";
            contenido += "<td class='text-right'>" + parsearMoneda(data[i].PRECIO_UNITARIO) + "</td>";
            contenido += "<td class='text-center'> " + data[i].CANTIDAD + "</td>";

            contenido += "<td class='text-right'>" + parsearMoneda(data[i].SUBTOTAL) + "</td>";
            //calcular el total con los subtotales
            total = total + data[i].SUBTOTAL;

            contenido += "</tr>";
        }

        contenido += "</tbody>";

        contenido += "<tfoot>";
        contenido += "<tr>";
        contenido += "<th>&nbsp;</th>";
        contenido += "<th>&nbsp;</th>";
        contenido += "<th>&nbsp;</th>";
        contenido += "<th>&nbsp;</th>";
        contenido += "<th>&nbsp;</th>";
        contenido += "<th class='text-right' >&nbsp;TOTAL: " + parsearMoneda(total) + "</th>";

        contenido += "</tr>";
        contenido += "</tfoot>";

        contenido += "</table>";


        document.getElementById("tabla-detallesPedidosPorCliente").innerHTML = contenido;

    });

}

function rellenarDatos() {
    //JsonDetallesPedidoCliente(int idPedido, int idCliente)

    var ruta = "/Pedidos/JsonDetallesPedidoCliente/?idPedido=";
    ruta += idPedido.toString();
    ruta += "&idCliente=";
    ruta += idCliente.toString();

    $.get(ruta, function (data) {

        
        //document.getElementById("txtRazonSocial").value = data.ID_PEDIDO;
        //document.getElementById("txtApellidosNombres").value = data.ID_CLIENTE;
        document.getElementById("txtRazonSocial").value = data.RAZON_SOCIAL;
        document.getElementById("txtApellidosNombres").value = data.APELLIDO_NOMBRE;
        document.getElementById("txtFecha").value = parsearFecha(data.FECHA_PEDIDO);
        document.getElementById("txtEstado").value = data.ESTADO_PEDIDO;
        document.getElementById("txtObservaciones").value = data.OBSERVACIONES;


    })

}

function parsearMoneda(decimal) {

    return new Intl.NumberFormat("es-AR", { style: "currency", currency: "ARS" }).format(decimal);
}

function parsearFecha(fecha) {


    if (fecha != null) {

        moment.locale("es");

        return moment(fecha).format('L');

    } else {

        return "No Aplica";
    }

}
