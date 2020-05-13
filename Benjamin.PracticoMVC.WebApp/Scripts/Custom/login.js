$(document).ready(function () {


    //SE EJECUTA DESPUES DE HACER CLIC EN EL BOTON INICIAR SESION
    $("#btnLogin").click(function () {


        //colocar en una variable el valor de cada elemento
        var usuario = document.getElementById("txtUsuario").value;
        var clave = document.getElementById("txtClave").value;

        
        if (validarCampos(usuario, clave) == true) {


            var frm = new FormData();

            //relacionar el valor de cada elemento con la clase que le corresponde
            frm.append("USUARIO", usuario);
            frm.append("CLAVE", clave);


            $.ajax({
                type: "POST",
                url: "/Usuarios/CodigoLogin/",
                data: frm,
                contentType: false,
                processData: false,
                success: function (data) {

                    if (data == 1) {
                        //Declaraciones ejecutadas cuando el resultado de expresión coincide con el valor1
                        location.href = '/Usuarios/Index/';

                    } else if (data == 2) {
                        //Declaraciones ejecutadas cuando el resultado de expresión coincide con el valor2

                        location.href = '/Usuarios/CambiarClave/';

                    } else {
                        //Declaraciones ejecutadas cuando ninguno de los valores coincide con el valor de la expresión

                        alertify.error("Usuario y/o Contraseña Incorrectos");
                        document.getElementById("txtClave").value = "";
                    }
                }
            });// fin ajax


        } //fin if


    });// fin click


});

function validarCampos(user, pass) {

    console.log(user);
    console.log(pass);

    var contarErrores = 0;

    // Validate length
    if (user.length >= 4) {
        //   alertify.success("Clave: minimo 4 caracteres");
    } else {
        alertify.error("Usuario: minimo 4 caracteres");
        contarErrores += 1;
    }


    // Validate length
    if (pass.length >= 4) {
     //   alertify.success("Clave: minimo 4 caracteres");
    } else {
        alertify.error("Clave: minimo 4 caracteres");
        contarErrores += 1;
    }

 

    /*

    // Validate capital letters
    var upperCaseLetters = /[A-Z]/g;
    if (pass.match(upperCaseLetters)) {
        alertify.success("Clave: debe incluir Mayúsculas");
    } else {
        alertify.error("Clave: debe incluir Mayúsculas");
        contarErrores += 1;
    }

    // Validate lowercase letters
    var lowerCaseLetters = /[a-z]/g;
    if (pass.match(lowerCaseLetters)) {
        alertify.success("Clave: debe incluir minúsculas");
    } else {
        alertify.error("Clave: debe incluir minúsculas");
        contarErrores += 1;
    }

    // Validate numbers
    var numbers = /[0-9]/g;
    if (pass.match(numbers)) {
        alertify.success("Clave: debe incluir números");
    } else {
        alertify.error("Clave: debe incluir números");
        contarErrores += 1;
    }

    */
  

    //si hay mas de 1 error la validacion da falsa
    if (contarErrores == 0) {
        return true;
    } else {

        return false;
    }

}


