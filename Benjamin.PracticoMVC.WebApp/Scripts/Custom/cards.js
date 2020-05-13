var cantidadCarrito = 0;

//$.get("/Pedidos/VerCantidadProductosEnCarrito/", function (data) {

    
//    cantidadCarrito = parseInt(data);

//    document.getElementById("idCantidadCarrito").innerHTML = "(" + cantidadCarrito + ")";
//});





$.get("/Productos/ListarCards/", function (data) {

    var contenido = "";



    for (var i = 0; i < data.length; i++) {


        contenido += "<div class='card' style='width:300px'>";

        contenido += "<img class='card-img-top' src='" + data[i].UBICACION + "' alt='Card image'>";

        contenido += "<div class='card-body text-center'>";
        //contenido += "<small id='idCodigo'>" + data[i].CODIGO + "</small>";


        contenido += "<h4 class='card-title'>" + data[i].NOMBRE + " - " + data[i].MARCA + "</h4>";

        contenido += "<p class='card-text'>" + data[i].DESCRIPCION + "</p>";
        contenido += "<h3 class='card-title'>" + parsearMoneda(data[i].PRECIO_UNITARIO) + "</h3>";
        contenido += "<button id='btnAgregarAlCarrito' class='btn btn-primary' onclick='agregarAlCarrito(" + data[i].CODIGO + ")' >Agregar al carrito</button>";

        contenido += "</div>";
        contenido += "</div>";

    }



    document.getElementById("divRenderizado").innerHTML = contenido;


});

function parsearMoneda(decimal) {

    return new Intl.NumberFormat("es-AR", { style: "currency", currency: "ARS" }).format(decimal);
};


function agregarAlCarrito(idProducto) {

    //alertify.success(idProducto.toString());


    //cantidadCarrito = cantidadCarrito + 1;

    //document.getElementById("idCantidadCarrito").innerHTML = "(" + cantidadCarrito + ")";
    var TIPO_DATO = new FormData();

    TIPO_DATO.append("ENTERO", idProducto);


    $.ajax({
        type: "POST",
        url: "/Pedidos/AgregarAlCarrito/",
        data: TIPO_DATO,
        contentType: false,
        processData: false,
        success: function (data) {


            var cantidadCarrito = parseInt(data);
            document.getElementById("idCantidadCarrito").innerHTML = "(" + cantidadCarrito + ")";
            //alert(cantidadCarrito);
            if (cantidadCarrito == 0) {
                alertify.error("El producto ya esta en el carrito!");
             
            } else {
                alertify.success("Se agrego al carrito!");
                //refrescar tabla
                tablaDetallePedido();
                
            }


        }//fin success

    }) // fin de ajax

}