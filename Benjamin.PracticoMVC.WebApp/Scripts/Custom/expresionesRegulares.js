boton = document.getElementById("btnProbar");

boton.onclick = function () {

    var cadena = document.getElementById("txtProbar").value;

    console.log("validarNumeroDecimal");
    console.log(validarNumeroDecimal(cadena));
    console.log("validarDescripcion");
    console.log(validarDescripcion(cadena));
}

function validarNumeroDecimal(cadena) {
    //Numeros decimales de 1 a 18 digitos.
    //Incluye puntos y coma

    var reg_ex = /[\d.,]{1,18}/;

    return reg_ex.test(cadena);

}

function validarTitulo(cadena) {

    //Permite de 3 a 99 caracteres alfanuméricos, incluyendo la ñ
    //y espacios en blanco puntos y guiones.
    //NO puede comenzar con espacios en blanco

    var reg_ex = /^(?!\s)([A-Za-zñÑ0-9-.\s]{3,99})/;

    return reg_ex.test(cadena);
}

function validarDescripcion(cadena) {

    //Permite de 3 a 200 caracteres alfanuméricos, incluyendo la ñ
    //y espacios en blanco puntos y guiones.
    //NO puede comenzar con espacios en blanco

    var reg_ex = /^(?!\s)([A-Za-zñÑ0-9-.\s]{3,200})/;

    return reg_ex.test(cadena);
}