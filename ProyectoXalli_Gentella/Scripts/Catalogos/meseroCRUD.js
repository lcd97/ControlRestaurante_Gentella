//ALMACENA LOS MESEROS
function saveWaiter() {
    var nombres = $("#nombre").val(), apellidos = $("#apellido").val(), cedula = $("#cedula").val(), inss = $("#inss").val(),
        ruc = $("#ruc").val(), hentrada = $("#entrada").val(), hsalida = $("#salida").val(), hturno = $("#inicio").val(), fturno = $("#fin").val();

    $.ajax({
        type: "POST",
        url: "/Meseros/Create",
        data: 
            "Nombres=" + nombres + "&Apellido=" + apellidos + "&Cedula=" + cedula + "&INSS=" + inss + "&RUC=" + ruc +
            "&HoraEntrada=" + hentrada + "&HoraSalida=" + hsalida + "&InicioTurno=" + hturno + "&FinTurno=" + fturno
        ,
        dataType: "JSON",
        success: function (data) {
            if (data.success) {
                $("#Table").DataTable().ajax.reload(); //RECARGAR DATATABLE PARA VER LOS CAMBIOS
                $("#small-modal").modal("hide"); //CERRAR MODAL
                Alert("Almacenado Correctamente", "", "success");
            } else {
                Alert("Error", data.message, "error");
            }
        },
        error: function () {
            Alert("Error", "Revisar", "error");
        }
    });
}//FIN FUNCTION

//FUNCION PARA EDITAR EL MESERO SELECCIONADO
function editWaiter() {
    var name = $("#nombre").val(), lastName = $("#apellido").val(), RUC = $("#ruc").val(), entrada =$("#entrada").val(), salida = $("#salida").val(),
        inicio = $("#tInicio").val(), fin = $("#tFin").val(), cedula = $("#cedula").val(), estado = $("#activo").is(":checked");

    $.ajax({
        type: "POST",
        url: "/Meseros/Edit",
        data: "Nombres=" + name + "&Apellido=" + lastName + "&Cedula=" + cedula + "&RUC=" + RUC + "&HoraEntrada=" + entrada + "&HoraSalida=" + salida +
            "&InicioTurno=" + inicio + "&FinTurno=" + fin + "&Estado=" + estado,
        success: function (data) {
            if (data.success) {
                $("#Table").DataTable().ajax.reload();//RECARGAR DATATABLE PARA VER LOS CAMBIOS
                $("#small-modal").modal("hide");//OCULTAR LA MODAL
                Alert("Almacenado correctamente", "", "success");
            } else {
                Alert("Error", data.message, "error");
            }
        }
    });
}//FIN FUNCTION