mostrarTabla();


function mostrarTabla() {

    $.get("/Pedidos/JsonPedidosClientes/", function (data) {

        var contenido = "";

        contenido += "<table id='tabla-paginacion' class='table table-striped'>";
        contenido += "<thead>";
        contenido += "<tr>";
        contenido += "<th scope='col'><i class='fas fa-sort'></i>NRO PEDIDO</th>";
        contenido += "<th scope='col'><i class='fas fa-sort'></i>FECHA PEDIDO</th>";
        contenido += "<th scope='col'><i class='fas fa-sort'></i>ESTADO</th>";
        contenido += "<th scope='col'><i class='fas fa-sort'></i>ID CLIENTE</th>";
        contenido += "<th scope='col'><i class='fas fa-sort'></i>RAZON SOCIAL</th>";
        contenido += "<th scope='col'><i class='fas fa-sort'></i>APELLIDO y NOMBRE</th>";
        //contenido += "<th scope='col'><i class='fas fa-sort'></i>OBSERVACIONES</th>";

        contenido += "<th scope='col'>DETALLES</th>";
        contenido += "</tr>";
        contenido += "</thead>";
        contenido += "<tbody>";

        for (var i = 0; i < data.length; i++) {
            contenido += "<tr>";

            contenido += "<td>&nbsp;&nbsp;" + data[i].ID_PEDIDO + "</td>";
            contenido += "<td>&nbsp;&nbsp;" + parsearFecha(data[i].FECHA_PEDIDO) + "</td>";
            contenido += "<td>&nbsp;&nbsp;" + data[i].ESTADO_PEDIDO + "</td>";
            contenido += "<td>&nbsp;&nbsp;" + data[i].ID_CLIENTE + "</td>";
            contenido += "<td>&nbsp;&nbsp;" + data[i].RAZON_SOCIAL + "</td>";
            contenido += "<td>&nbsp;&nbsp;" + data[i].APELLIDO_NOMBRE + "</td>";
            //contenido += "<td>&nbsp;&nbsp;" + data[i].OBSERVACIONES + "</td>";
            contenido += "<td class='text-center' ><button id='btnDetalles' class='btn btn-primary' onclick='detalles(" + data[i].ID_CLIENTE + "," + data[i].ID_PEDIDO + ")' ><i class='fas fa-info-circle'></i></button></td>";

            contenido += "</tr>";
        }

        contenido += "</tbody>";
        contenido += "<tfoot>";
        contenido += "<tr>";
        contenido += "<th>&nbsp;NRO PEDIDO</th>";
        contenido += "<th>&nbsp;FECHA PEDIDO</th>";
        contenido += "<th>&nbsp;ESTADO</th>";
        contenido += "<th>&nbsp;ID CLIENTE</th>";
        contenido += "<th>&nbsp;RAZON SOCIAL</th>";
        contenido += "<th>APELLIDO y NOMBRE</th>";
        //contenido += "<th>&nbsp;OBSERVACIONES</th>";
        contenido += "<th>&nbsp;DETALLES</th>";
        contenido += "</tr>";
        contenido += "</tfoot>";
        contenido += "</table>";


        document.getElementById("idTabla-pedidosClientes").innerHTML = contenido;

        $("#tabla-paginacion").dataTable({
            "language":
            {
                "sProcessing": "Procesando...",
                "sLengthMenu": "Mostrar _MENU_ registros",
                "sZeroRecords": "No se encontraron resultados",
                "sEmptyTable": "Ningún dato disponible en esta tabla =(",
                "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sInfoPostFix": "",
                "sSearch": "Buscar:",
                "sUrl": "",
                "sInfoThousands": ",",
                "sLoadingRecords": "Cargando...",
                "oPaginate": {
                    "sFirst": "Primero",
                    "sLast": "Último",
                    "sNext": "Siguiente",
                    "sPrevious": "Anterior"
                },
                "oAria": {
                    "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                    "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                },
                "buttons": {
                    "copy": "Copiar",
                    "colvis": "Visibilidad"
                }
            }

        }); // fin datatable

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

function detalles(idCliente, idPedido) {

    //alertify.success("idcliente: " + idCliente + " idPedido: " + idPedido);

    location.href = "/Pedidos/DetallesPedidos/?idPedido=" + idPedido + "&idCliente=" + idCliente;
}