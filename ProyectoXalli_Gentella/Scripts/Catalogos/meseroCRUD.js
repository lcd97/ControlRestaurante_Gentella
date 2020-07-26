//ALMACENA LOS MESEROS
function saveWaiter() {
    var nombres = $("#nombre").val(), apellidos = $("#apellido").val(), cedula = $("#cedula").val().toUpperCase(), inss = $("#inss").val(),
        ruc = $("#ruc").val().toUpperCase(), hentrada = $("#entrada").val(), hsalida = $("#salida").val();

    if (validado() == true) {
        $.ajax({
            type: "POST",
            url: "/Meseros/Create",
            data:
                "Nombres=" + nombres + "&Apellido=" + apellidos + "&Cedula=" + cedula + "&INSS=" + inss + "&RUC=" + ruc +
                "&HoraEntrada=" + hentrada + "&HoraSalida=" + hsalida
            ,
            dataType: "JSON",
            success: function (data) {
                if (data.success) {
                    $("#Table").DataTable().ajax.reload(); //RECARGAR DATATABLE PARA VER LOS CAMBIOS
                    $("#small-modal").modal("hide"); //CERRAR MODAL
                    swal({
                        title: "Completado",
                        text: data.message,
                        icon: "success",
                        buttons: false,
                        timer: 1500
                    });                } else {
                    Alert("Error", data.message, "error");
                }
            },
            error: function () {
                Alert("Error", "Revisar", "error");
            }
        });
    } else
        Alert("Error", "Campos vacios", "error");
}//FIN FUNCTION

//FUNCION PARA EDITAR EL MESERO SELECCIONADO
function editWaiter() {
    var name = $("#nombre").val(), lastName = $("#apellido").val(), RUC = $("#ruc").val().toUpperCase(), entrada = $("#entrada").val(), salida = $("#salida").val(),
        cedula = $("#cedula").val().toUpperCase(), estado = $("#activo").is(":checked");

    if (validado() == true) {
        $.ajax({
            type: "POST",
            url: "/Meseros/Edit",
            data: "Nombres=" + name + "&Apellido=" + lastName + "&Cedula=" + cedula + "&RUC=" + RUC + "&HoraEntrada=" + entrada + "&HoraSalida=" + salida +
                "&Estado=" + estado,
            success: function (data) {
                if (data.success) {
                    $("#Table").DataTable().ajax.reload();//RECARGAR DATATABLE PARA VER LOS CAMBIOS
                    $("#small-modal").modal("hide");//OCULTAR LA MODAL
                    swal({
                        title: "Completado",
                        text: data.message,
                        icon: "success",
                        buttons: false,
                        timer: 1500
                    });                } else {
                    Alert("Error", data.message, "error");
                }
            },
            error: function () {
                Alert("Error", "Revisar", "error");
            }
        });
    } else {
        Alert("Error", "Campos vacios", "error");
    }
}//FIN FUNCTION

function validado() {
    if ($("#nombre").val() != "" && $("#apellido").val() != "" && $("#entrada").val() != "" && $("#salida").val() != "" && $("#cedula").val() != "" && $("#inss").val()) {
        return true;
    } else
        return false;
}