$(document).ready(function () {


    //SE EJECUTA DESPUES DE HACER CLIC EN EL BOTON GUARDAR
    $("#btnAceptar").click(function () {


        var claveActual = document.getElementById("txtClaveActual").value;
        var claveNueva = document.getElementById("txtClaveNueva").value;
        var repetirClaveNueva = document.getElementById("txtRepetirClaveNueva").value;


        //validar longitud de claves
        if (claveActual.length >= 4 && claveNueva.length >= 4 && repetirClaveNueva.length >= 4) {

            //validar si la clave nueva coincide con la q repite
            if (claveNueva == repetirClaveNueva) {


                // se chequea si la clave actual es valida, si es valida cambiar clave sino no
                $.get("/Usuarios/ValidarClaveActual/?claveActual=" + claveActual, function (data) {


                    if (data == 1) {

                        alertify.success("clave actual válida");

                        cambiarClave(claveNueva);

                    } else {
                        alertify.error("La clave actual no es válida");
                        document.getElementById("txtClaveActual").value = "";
                    }


                });// fin get

            } else {//si las claves no coinciden error
                alertify.error("Las claves no coinciden");
            } // fin if chequear claves coincidentes

        } else {//error si no se valida longitud de claves
            alertify.error("Claves: minimo 4 caracteres");
        }// fin if chequear longitud de caracteres

    }); // fin btnAceptar clic

    $("#btnCancelar").click(function () {
        location.href = "/Usuarios/Index";
    });
}); // fin document ready


function cambiarClave(claveNueva) {

    var frm = new FormData();

    //relacionar el valor de cada elemento con la clase que le corresponde
    frm.append("USUARIO", null);
    frm.append("CLAVE", claveNueva);

    $.ajax({
        type: "POST",
        url: "/Usuarios/CodigoCambiarClave/",
        data: frm,
        contentType: false,
        processData: false,
        success: function (data) {

            if (data == 1) {

                alertify.success("Clave cambiada!");

                location.href = '/Usuarios/Index/';

            } else {
                //Declaraciones ejecutadas cuando ninguno de los valores coincide con el valor de la expresión

                alertify.error("No se pudo cambiar la clave");

            }






        }

    });// fin ajax


}