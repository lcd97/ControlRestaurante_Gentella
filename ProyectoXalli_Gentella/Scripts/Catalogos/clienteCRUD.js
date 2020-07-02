//FUNCION PARA ALMACENAR UN OBJETO CLIENTE
function saveCustomer() {
    var nombre, apellido, documento, ruc, email, telefono, tipoCliente, tipo;

    nombre = $("#nombre").val();
    apellido = $("#apellido").val();
    documento = $("#numero").val().toUpperCase();
    ruc = $("#ruc").val().toUpperCase();
    email = $("#email").val();
    telefono = $("#telefono").val();
    tipoCliente = $("#tipo").val();
    tipo = $("#documento").val();

    //alert(nombre + " " + apellido + " " + documento + " " + ruc + " " + email + " " + telefono + " " + tipoCliente + " " + tipo);

    //var data = "Nombre=" + nombre + "&Apellido=" + apellido + "&Documento=" + documento + "&RUC=" + ruc + "&Email=" + email +
    //    "&Telefono=" + telefono + "&TipoCliente=" + tipoCliente + "&Tipo=" + tipo;
    
    if (validado() == true) {
        //FUNCION AJAX
        $.ajax({
            type: "POST",
            url: "/Clientes/Create",
            dataType: "JSON",
            data: {
                Nombre: nombre, Apellido: apellido, Documento: documento, RUC: ruc,
                Email: email, Telefono: telefono, TipoCliente: tipoCliente, Tipo: tipo
            },//OTRA MANERA DE ENVIAR PARAMETROS AL CONTROLADOR
            success: function (data) {
                if (data.success) {
                    $("#Table").DataTable().ajax.reload(); //RECARGAR DATATABLE PARA VER LOS CAMBIOS
                    $("#small-modal").modal("hide"); //CERRAR MODAL
                    Alert("Almacenado correctamente", data.message, "success");//ALMACENADO CORRECTAMENTE
                } else
                    Alert("Error al almacenar", data.message, "error");//MENSAJE DE ERROR
            },
            error: function () {
                Alert("Error al almacenar", "Intentelo de nuevo", "error");
            }
        });//FIN AJAX
    } else {
        Alert("Error", "Campos vacios","error");
    }
}

//FUNCTION PARA EDITAR CLIENTE
function editCustomer(Id) {
    var Nombre, Apellido, tipoDocumento = 1, Documento, tipoCliente, ruc, Email, Telefono, Estado;

    Nombre = $("#nombre").val();
    Apellido = $("#apellido").val();
    Documento = $("#numero").val();
    ruc = $("#ruc").val();
    Email = $("#email").val();
    Telefono = $("#telefono").val();
    tipoCliente = $("#tipo").val();
    Estado = $("#estado").is(":checked");

    if ($("#documento").val() == "Pasaporte") {
        tipoDocumento = 2;
    }

    if (validado() === true) {
        $.ajax({
            type: "POST",
            url: "/Clientes/Edit",
            data: {
                Id : Id, Nombre: Nombre, Apellido: Apellido, Documento: Documento, RUC: ruc, Email: Email,
                Telefono: Telefono, TipoCliente: tipoCliente, TipoDocumento: tipoDocumento, Estado: Estado
            },
            success: function (data) {
                if (data.success) {
                    $("#Table").DataTable().ajax.reload(); //RECARGAR DATATABLE PARA VER LOS CAMBIOS
                    $("#small-modal").modal("hide"); //CERRAR MODAL
                    Alert("Almacenado Correctamente", "", "success");
                } else {
                    Alert("Error", data.data[0].message, "error");
                }
            },
            error: function () {
                Alert("Error", "Intentelo de nuevo", "error");
            }
        });
    } else {
        Alert("Error", "Campos vacios", "error");
    }
}//FIN FUNCTION

//FUNCION PARA VALIDAR CAMPOS VACIOS
function validado() {
    if ($("#nombre").val() != "" && $("#apellido").val() != "" && $("#numero").val() != "") {
        return true;
    } else
        return false;
}//FIN FUNCTION