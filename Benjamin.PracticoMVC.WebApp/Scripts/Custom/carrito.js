var estadoPedido = "";
var idPedido = document.getElementById("txtIdPedido").value;
var idCliente = document.getElementById("txtIdCliente").value;

//ocultamos idCliente, ya q solo se trataba de obtener su valor y no mostrarlo
document.getElementById("txtIdCliente").style.display = "none";



rellenarDatos();


function tablaDetallePedidoEditable() {


    $.get("/Pedidos/JsonObtenerDetallesPedido/?idPedido=" + idPedido, function (data) {

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
        contenido += "<th scope='col' class='text-center'>ELIMINAR</th>";
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
            contenido += "<td class='text-center'> <input id='txtCantidad" + data[i].ITEM + "' type='number'  min='1' step='1' value='" + data[i].CANTIDAD + "' onchange='modificarCantidad(" + data[i].ITEM + ")'> </td>";

            contenido += "<td class='text-right'>" + parsearMoneda(data[i].SUBTOTAL) + "</td>";
            //calcular el total con los subtotales
            total = total + data[i].SUBTOTAL;
            contenido += "<td class='text-center' ><button id='btnEliminar' class='btn btn-danger' onclick='eliminarItem(" + codProducto + ")' ><i class='fas fa-trash-alt'></i></button></td>";

            contenido += "</tr>";
        }

        contenido += "</tbody>";

        contenido += "<tfoot>";
        contenido += "<tr>";
        contenido += "<th>&nbsp;</th>";
        contenido += "<th>&nbsp;</th>";
        contenido += "<th>&nbsp;</th>";
        contenido += "<th>&nbsp;</th>";
        contenido += "<th class='text-right' >&nbsp;TOTAL: " + parsearMoneda(total) + "</th>";
        contenido += "<th>&nbsp;</th>";
        contenido += "</tr>";
        contenido += "</tfoot>";

        contenido += "</table>";


        document.getElementById("tabla-detalle-pedidos").innerHTML = contenido;



    });



}


function tablaDetallePedido() {

    //dejar como solo lectura el campo observaciones
    document.getElementById("txtObservaciones").readOnly = true;
    document.getElementById("btnSeguirComprando").style.display = "none";
    document.getElementById("btnCompletarPedido").style.display = "none";
    document.getElementById("divEstado").innerHTML = "<br /> <label>Estado de Pedido: <input type='text'  value='" + estadoPedido + "' readonly class='form-control' /></label>";




    $.get("/Pedidos/JsonObtenerDetallesPedido/?idPedido=" + idPedido, function (data) {

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
            contenido += "<td class='text-center'> " + data[i].CANTIDAD + " </td>";

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


        document.getElementById("tabla-detalle-pedidos").innerHTML = contenido;



    });



}


function rellenarDatos() {
    //JsonDetallesPedidoCliente(int idPedido, int idCliente)

    var ruta = "/Pedidos/JsonDetallesPedidoCliente/?idPedido=";
    ruta += idPedido.toString();
    ruta += "&idCliente=";
    ruta += idCliente.toString();

    $.get(ruta, function (data) {


        //document.getElementById("txtRazonSocial").value = data.RAZON_SOCIAL;
        //document.getElementById("txtApellidosNombres").value = data.APELLIDO_NOMBRE;
        document.getElementById("txtFechaPedido").value = parsearFecha(data.FECHA_PEDIDO);
        //document.getElementById("txtEstado").value = data.ESTADO_PEDIDO;
        document.getElementById("txtObservaciones").value = data.OBSERVACIONES;

        estadoPedido = data.ESTADO_PEDIDO;

        if (estadoPedido == "PENDIENTE") {
            //solo si esta pendiente se pueden editar cantidades
            tablaDetallePedidoEditable();
        } else {//sino queda la tabla como solo lectura
            tablaDetallePedido();
        }


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

function eliminarItem(codProducto) {

    //alertify.error("Pedido Nro:" + idPedido + " - Eliminar Item Nro:" + codProducto);

    alertify.confirm('Carrito', //titulo
        '¿Desea Eliminar item?', //mensaje
        function () { //cuando se presiona OK

            var obj = new FormData();

            obj.append("ID_PEDIDO", idPedido);
            obj.append("ID_PRODUCTO", codProducto);

            $.ajax({
                type: "POST",
                url: "/Pedidos/EliminarItemPedido/",
                data: obj,
                contentType: false,
                processData: false,
                success: function (data) {


                    var filasAfectadas = parseInt(data);

                    if (filasAfectadas == 1) {

                        //refrescar tabla
                        tablaDetallePedido();
                        alertify.success("Item eliminado");
                    } else {
                        alert("error");
                    }


                }//fin success

            }) // fin de ajax

        },
        function () {//cuando se presiona Cancel
            alertify.error("Cancelado")
        });



}


function modificarCantidad(codProducto) {


    var cantidad = document.getElementById("txtCantidad" + codProducto.toString()).value;

    //alertify.success("Pedido Nro:" + idPedido + " - Item Nro:" + codProducto + " - Cantidad: " + cantidad);

    var obj = new FormData();

    obj.append("ID_PEDIDO", idPedido);
    obj.append("ITEM", codProducto);
    obj.append("CANTIDAD", cantidad);



    $.ajax({
        type: "POST",
        url: "/Pedidos/Recalcular/",
        data: obj,
        contentType: false,
        processData: false,
        success: function (data) {


            var filasAfectadas = parseInt(data);

            if (filasAfectadas == 1) {

                //refrescar tabla


                if (estadoPedido == "PENDIENTE") {
                    //solo si esta pendiente se pueden editar cantidades
                    tablaDetallePedidoEditable();
                } else {//sino queda la tabla como solo lectura
                    tablaDetallePedido();
                }


            } else {
                alert("error");
            }


        }//fin success

    }) // fin de ajax


}


$(document).ready(function () {
    $("#btnSeguirComprando").click(function () {
        location.href = "/Productos/Cards/";
    });

    $("#btnCompletarPedido").click(function () {


        alertify.confirm('Pedidos', //titulo
            '¿Desea Enviar el Pedido y Finalizarlo?', //mensaje
            function () { //cuando se presiona OK


                //////////////inicio/////////////////


                var observaciones = document.getElementById("txtObservaciones").value;

                if (observaciones.length > 80) {
                    alertify.error("No puede ingresar mas de 80 caracteres");
                    return;
                }

                var obj = new FormData();

                //relacionar el valor de cada elemento con la clase que le corresponde
                obj.append("ID_PEDIDO", idPedido);
                obj.append("ID_CLIENTE", idCliente);
                obj.append("OBSERVACIONES", observaciones);
                obj.append("ESTADO_PEDIDO", "(F)");



                $.ajax({
                    type: "POST",
                    url: "/Pedidos/FinalizarPedido/",
                    data: obj,
                    contentType: false,
                    processData: false,
                    success: function (data) {

                        if (data == 1) {
                            //Declaraciones ejecutadas cuando el resultado de expresión coincide con el valor1
                            alertify.success("Pedido Finalizado");

                            location.href = "/Pedidos/MisPedidos/";

                        } else {
                            //Declaraciones ejecutadas cuando ninguno de los valores coincide con el valor de la expresión

                            alertify.error("Error");

                        }
                    }
                });// fin ajax



                ///////////////fin//////////////////



            },
            function () {/* alertify.error('No se realizó el reset clave') */ }); //cuando se presiona Cancel





    });
});


//$(document).ready(function () {
//    $("#btnLogin").click(function () { });

//    $("#btnLogin").click(function () { });
//});