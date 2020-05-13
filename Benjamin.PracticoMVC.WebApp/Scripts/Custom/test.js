

alertify.confirm('Productos', //titulo
    '¿Desea Guardar cambios?', //mensaje
    function () { //cuando se presiona OK
     alertify.error("OK") 
    },
    function () {//cuando se presiona Cancel
        alertify.error("Cancel") 
    }); 