var idSeleccionado = document.getElementById("cboRol").value;

document.getElementById("txtIdRol").value = idSeleccionado;

document.getElementById("txtIdRol").style.display = "none";

function obtenerIdUsuario(id) {

    alertify.success("ID: " + id.toString());

}


$('#cboRol').change(function () {
    /* Obtener el valor de tus dropdownlist */
    var selectedId = $(this).val();
    document.getElementById("txtIdRol").value = selectedId;
});