var contadorCargaImagenes = 0;



//sirve para plasmar el nombre del archivo q se sube en una file input
$('.custom-file-input').on('change', function (event) {

    var inputFile = event.currentTarget;
    var nombreArchivo = inputFile.files[0].name;


    $(inputFile).parent()
        .find('.custom-file-label')
        .html("Img Seleccionada");
    document.getElementById("divFoto").innerHTML = "<img class='img-fluid img-thumbnail' src='/Images/productos/" + inputFile.files[0].name + "'  />";

    document.getElementById("idRuta").value = "/Images/productos/" + nombreArchivo;
    //habilitar etiqueta ubicacion
    document.getElementById("idRutaPadre").style.display = "block";

    contadorCargaImagenes = contadorCargaImagenes + 1;
});


mostrarTabla();

function mostrarTabla() {

    $.get("/Productos/Listar", function (data) {

        var cadenaBoolean = "";


        var contenido = "";

        contenido += "<table id='tabla-paginacion-usuarios' class='table table-striped'>";
        contenido += "<thead>";
        contenido += "<tr>";
        contenido += "<th scope='col'><i class='fas fa-sort'></i>CODIGO</th>";
        contenido += "<th scope='col'><i class='fas fa-sort'></i>NOMBRE</th>";
        contenido += "<th scope='col'><i class='fas fa-sort'></i>MARCA</th>";
        contenido += "<th scope='col' class='text-right'><i class='fas fa-sort'></i>PRECIO</th>";
        contenido += "<th scope='col'><i class='fas fa-sort'></i>ACTIVO</th>";
        contenido += "<th scope='col'>EDITAR</th>";
        contenido += "</tr>";
        contenido += "</thead>";
        contenido += "<tbody>";

        for (var i = 0; i < data.length; i++) {
            contenido += "<tr>";

            contenido += "<td>&nbsp;&nbsp;" + data[i].CODIGO + "</td>";
            codigo = parseInt(data[i].CODIGO);
            contenido += "<td>&nbsp;&nbsp;" + data[i].NOMBRE + "</td>";
            contenido += "<td>&nbsp;&nbsp;" + data[i].MARCA + "</td>";
            contenido += "<td class='text-right' >" + parsearMoneda(data[i].PRECIO) + "</td>";
            contenido += "<td>&nbsp;&nbsp;" + convertirBooleanToString(data[i].ACTIVO.toString()) + "</td>";
            contenido += "<td>&nbsp;&nbsp;<button id='btnEditar' class='btn btn-primary' onclick='abrirModal(" + codigo + ")' data-toggle='modal' data-target='#exampleModal'><i class='fas fa-edit'></i></button></td>";


            contenido += "</tr>";
        }

        contenido += "</tbody>";
        contenido += "<tfoot>";
        contenido += "<tr>";
        contenido += "<th>&nbsp;CODIGO</th>";
        contenido += "<th>&nbsp;NOMBRE</th>";
        contenido += "<th>&nbsp;MARCA</th>";
        contenido += "<th class='text-right'>&nbsp;PRECIO</th>";
        contenido += "<th>&nbsp;ACTIVO</th>";
        contenido += "<th>EDITAR</th>";
        contenido += "</tr>";
        contenido += "</tfoot>";
        contenido += "</table>";


        document.getElementById("tabla-usuarios").innerHTML = contenido;

        $("#tabla-paginacion-usuarios").dataTable({
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


rellenarComboBox("Marcas", "Listar", "cboMarcas");

function rellenarComboBox(controlador, jsonAccion, stringID) {

    var ruta = "/";
    ruta += controlador + "/";
    ruta += jsonAccion + "/";


    $.get(ruta, function (data) {

        //string q representa las etiquetas html
        var contenido = "<option value='0'>--Seleccione--</option>";

        for (var i = 0; i < data.length; i++) {

            contenido += "<option value='" + data[i].Id + "'>";
            contenido += data[i].Nombre;
            contenido += "</option>";
        }

        //transformar la cadena en html e insertar dentro del id del combo box
        document.getElementById(stringID).innerHTML = contenido;


    });

}


function convertirBooleanToString(cadenaBoolean) {


    if (cadenaBoolean == "true") {

        return "Activo";
    }

    if (cadenaBoolean == "false") {

        return "Baja";
    }

}


function parsearMoneda(decimal) {

    return new Intl.NumberFormat("es-AR", { style: "currency", currency: "ARS" }).format(decimal);
};


function abrirModal(id) {

    //cuando se abre el modal se setea en cero para decir que por defecto coloque imagen no disponible
    contadorCargaImagenes = 0;

    //apenas abre el modal la etiqueta debe quedar con examinar.. si selecciona algun archivo poner el nombre del archivo
    document.getElementById("lblNombreArchivo").innerHTML = "Examinar...";



    //Si el ID es cero usamos el modal para agregar
    if (id == 0) {

        //modificar titulo del modal
        document.getElementById("tituloModal").innerHTML = "<strong>Agregar Producto</strong>";

        limpiarDatos();

        //chkEstado predefinirlo activo
        document.getElementById("chkActivo").checked = true;

        //por defecto la foto es la de no disponible
        document.getElementById("divFoto").innerHTML = "<img class='img-fluid img-thumbnail' src='/Images/productos/no-disponible.png'  />";

        //deshabilitar etiqueta ubicacion
        document.getElementById("idRutaPadre").style.display = "none";

    }//Si el ID distinto de cero usamos el modal para editar
    else {
        //habilitar etiqueta ubicacion
        document.getElementById("idRutaPadre").style.display = "block";


        document.getElementById("tituloModal").innerHTML = "<strong>Editar Producto</strong>";

        obtenerRegistro("Productos", "Detalle", id);


    }

}


function limpiarDatos() {
    limpiarTextBoxes();
    limpiarComboBoxes();
    limpiarFileInput();
}

function limpiarTextBoxes() {

    //se limpian todos los textboxes dejando un string vacio
    var controles = document.getElementsByClassName("limpiar");

    for (var i = 0; i < controles.length; i++) {

        controles[i].value = "";
    }
}

function limpiarComboBoxes() {

    //se limpian todos los comboboxes dejando string vacio
    var controles = document.getElementsByClassName("limpiarCbo");

    for (var i = 0; i < controles.length; i++) {

        controles[i].value = 0;
    }

}

function limpiarFileInput() {

}
function obtenerRegistro(controlador, jsonAccion, id) {

    //ruta = /Controlador/Accion/?id=parametro
    var ruta = "/";
    ruta += controlador + "/";
    ruta += jsonAccion + "/";
    ruta += "?id=" + id;

    console.log(ruta);

    $.get(ruta, function (data) {



        document.getElementById("txtCodigo").value = data.Codigo;
        document.getElementById("txtNombre").value = data.Nombre;
        document.getElementById("txtDescripcion").value = data.Descripcion;
        document.getElementById("cboMarcas").value = data.IdMarca;
        document.getElementById("txtPrecioUnitario").value = data.PrecioUnitario;
        document.getElementById("chkActivo").checked = data.Activo;
        document.getElementById("idRuta").value = data.UrlImange;

        var ubicacionFoto = "";
        ubicacionFoto += "<img class='img-fluid img-thumbnail' src='";
        ubicacionFoto += data.UrlImange;
        ubicacionFoto += "' />";

        document.getElementById("divFoto").innerHTML = ubicacionFoto;



    });
}


function guardar() {



    if (obligatorio() == true) {
        var frm = new FormData();

        //colocar en una variable el valor de cada elemento
        var codigo = document.getElementById("txtCodigo").value;
        var nombre = document.getElementById("txtNombre").value;
        var descripcion = document.getElementById("txtDescripcion").value;
        var idMarca = document.getElementById("cboMarcas").value;
        var precioUnitario = document.getElementById("txtPrecioUnitario").value;
        var activo = document.getElementById("chkActivo").checked;
        var ruta = document.getElementById("idRuta").value;


        

        //si el codigo es cero significa que es crear un nuevo objeto
        if (codigo == 0) {

            //se va a colocar imagen por defecto de no disponible en que caso de que no se seleccione ninguna
            var imgNombreArchivo = "no-disponible.png";
            //carpeta donde se guardan las fotos por defecto
            var carpeta = "/Images/productos/";

            //si se carga un archivo para cargarlo en imagenes la primera vez va a ser "no-disponible.png"
            //una vez empice a cargar archivos se tomara en cuenta el nombre del archivo que se cargo
            if (contadorCargaImagenes > 0) {

                imgNombreArchivo = document.getElementById("imgNombreArchivo").value.replace('C:\\fakepath\\', "");
            }

           
            //compone la ruta completa o ubicacion del archivo con su nombre
            ruta = carpeta + imgNombreArchivo;
        }


       


      


        console.log(codigo);
        console.log(nombre);
        console.log(descripcion);
        console.log(idMarca);
        console.log(precioUnitario);
        console.log(activo);
        console.log(imgNombreArchivo);
        console.log(carpeta);
        console.log(ruta);





        //relacionar el valor de cada elemento con la clase que le corresponde
        frm.append("Codigo", codigo);
        frm.append("Nombre", nombre);
        frm.append("Descripcion", descripcion);
        frm.append("IdMarca", idMarca);
        frm.append("PrecioUnitario", precioUnitario.replace(".", ","));
        frm.append("Activo", activo);
        frm.append("UrlImange", ruta);

        ////////////////////////////////////////////////////

        alertify.confirm('Productos', //titulo
            '¿Desea Guardar cambios?', //mensaje
            function () { //cuando se presiona OK

                /////////////////////ajax//////////////////
                $.ajax({
                    type: "POST",
                    url: "/Productos/Guardar/",
                    data: frm,
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        if (data != 0) {

                            mostrarTabla();

                            if (codigo == 0) {

                                alertify.success('Agregado exitosamente!');

                            } else {

                                alertify.success('Editado exitosamente!');
                            }

                            $("#btnCerrar").click();

                        }
                        else {
                            alertify.error('Error');
                        }

                    }//fin success

                }) // fin de ajax



                /////////////////fin ajax////////////////////////


            },
            function () {/* alertify.error('No se realizó el reset clave') */ }); //cuando se presiona Cancel



    }



}

function resetearClave(idUsuario) {



    /*
     * @message {String or DOMElement} The dialog contents.
     * @onok {Function} Invoked when the user clicks OK button.
     * @oncancel {Function} Invoked when the user clicks Cancel button or closes the dialog.
     *
     *  alertify.confirm(message, onok, oncancel);
     *  
     * alertify.confirm('titulo','Confirm Message', function(){ alertify.success('Ok') }, function(){ alertify.error('Cancel')});
     */
    alertify.confirm('ID Usuario: ' + idUsuario, //titulo
        '¿Desear resetear la clave?', //mensaje
        function () { //cuando se presiona OK

            //obtener codigo de reset clave, si es 1 se reseteo correctamente
            //va a quedar usuario y clave iguales
            $.get("/Usuarios/CodigoResetearClave/?idUsuario=" + idUsuario, function (data) {

                if (data == 1) {
                    alertify.success('Clave reseteada con exito');
                } else {
                    alertify.error('No se realizó el reset clave');
                }

            });


        },
        function () {/* alertify.error('No se realizó el reset clave') */ }); //cuando se presiona Cancel



}




function obligatorio() {

    var contador = 0;

    //colocar en una variable el valor de cada elemento
    //   var codigo = document.getElementById("txtCodigo").value;
    var nombre = document.getElementById("txtNombre").value;
    var descripcion = document.getElementById("txtDescripcion").value;
    var idMarca = document.getElementById("cboMarcas").value;
    var precioUnitario = document.getElementById("txtPrecioUnitario").value;
    // var activo = document.getElementById("chkActivo").checked;

    if (validarCampoTexto(nombre) == false) {
        contador = contador + 1;
        alertify.error("Ingrese correctamente el nombre");
    }


    if (validarCampoTexto(descripcion) == false) {
        contador = contador + 1;
        alertify.error("Ingrese correctamente la descripcion");
    }

    if (idMarca <= 0) {
        contador = contador + 1;
        alertify.error("Seleccione Marca");
    }

    if (validarNumeroDecimal(precioUnitario) == false) {
        contador = contador + 1;
        alertify.error("Ingrese correctamente el precio");
    }



    if (contador == 0) {
        return true;
    } else {
        return false;
    }
}


function validarCampoTexto(cadena) {

    //Permite de 3 a 99 caracteres alfanuméricos, incluyendo la ñ
    //y espacios en blanco puntos y guiones.
    //NO puede comenzar con espacios en blanco

    var reg_ex = /^(?!\s)([A-Za-zñÑ0-9-.\s]{3,99})/;

    return reg_ex.test(cadena);
}

function validarNumeroDecimal(cadena) {
    //Numeros decimales de 1 a 18 digitos.
    //Incluye puntos y coma

    var reg_ex = /[\d.,]{1,18}/;

    return reg_ex.test(cadena);

}

/*

var btnOcultar = document.getElementById("btnOcultar");

btnOcultar.onclick = function ()
{
    alertify.success("putito");
    $("#btnCerrar").click();
}

*/



function guardar7() {

    var filasAfectadas = 0;

    // chequear campos obligatorios
    if (obligatorio() == true) {

        var frm = new FormData();

        //colocar en una variable el valor de cada elemento
        var codigo = document.getElementById("txtCodigo").value;
        var nombre = document.getElementById("txtNombre").value;
        var descripcion = document.getElementById("txtDescripcion").value;
        var idMarca = document.getElementById("cboMarcas").value;
        var precioUnitario = document.getElementById("txtPrecioUnitario").value;
        var activo = document.getElementById("chkActivo").checked;


        var imgNombreArchivo = "no-disponible.png";

        //si se carga un archivo para cargarlo en imagenes la primera vez va a ser imagen no disponible
        //una vez empice a cargar archivos se tomara en cuenta el nombre del archivo que se cargo
        if (contadorCargaImagenes != 0) {
            imgNombreArchivo = document.getElementById("imgNombreArchivo").value.replace('C:\\fakepath\\', "");
        }
        //carpeta donde se guardan las fotos por defecto
        var carpeta = "/Images/productos/";
        //compone la ruta completa o ubicacion del archivo con su nombre
        var ruta = carpeta + imgNombreArchivo;


        console.log(codigo);
        console.log(nombre);
        console.log(descripcion);
        console.log(idMarca);
        console.log(precioUnitario);
        console.log(activo);
        console.log(imgNombreArchivo);
        console.log(carpeta);
        console.log(ruta);





        //relacionar el valor de cada elemento con la clase que le corresponde
        frm.append("Codigo", codigo);
        frm.append("Nombre", nombre);
        frm.append("Descripcion", descripcion);
        frm.append("IdMarca", idMarca);
        frm.append("PrecioUnitario", precioUnitario.replace(".", ","));
        frm.append("Activo", activo);
        frm.append("UrlImange", ruta);




        alertify.confirm("¿Desea Guardar cambios?", function (e) {
            if (e) {
                //after clicking OK

                $.ajax({
                    type: "POST",
                    url: "/Productos/Guardar/",
                    data: frm,
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        if (data != 0) {

                            mostrarTabla();

                            if (codigo == 0)
                                alertify.success('Agregado exitosamente!');
                            else
                                alertify.success('Editado exitosamente!');


                            document.getElementById("btnCancelar").click();


                        } else {
                            alertify.error('Error');
                        }

                    }

                })







                //else de alertify despues de evento cancel 
            } else {
                //after clicking Cancel          
            }
        });// fin alertify



    }



}
