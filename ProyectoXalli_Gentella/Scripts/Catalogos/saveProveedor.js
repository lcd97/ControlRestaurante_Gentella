var tipo = false;

//EVENTO CLICK DEL BOTON COMERCIO PARA DESACTIVAR CAMPOS
$("#comercio").click(function () {
    //DESACTIVO LOS CAMPOS
    $("#apellido").attr("disabled", true);
    $("#cedula").attr("disabled", true);

    //LIMPIO LOS VALORES DE LOS CAMPOS DESACTIVADOS
    $("#apellido").val("");
    $("#cedula").val("");

    //AGREGO LA CLASE ACTIVO AL BOTON SELECCIONADO
    if (!$("#comercio").hasClass("active")) {
        $("#comercio").toggleClass("active");
        $("#local").removeClass("active");
    }

    //OBTIENE EL VALOR DEL BOTON SELECCIONADO
    tipo = false;

});

//EVENTO CLICK DEL BOTON COMERCIO PARA ACTIVAR CAMPOS
$("#local").click(function () {
    //ACTIVO LOS CAMPOS
    $("#apellido").attr("disabled", false);
    $("#cedula").attr("disabled", false);

    //AGREGO LA CLASE ACTIVO AL BOTON SELECCIONADO
    if (!$("#local").hasClass("active")) {
        $("#local").toggleClass("active");
        $("#comercio").removeClass("active");
    }

    //OBTIENE EL VALOR DEL BOTON SELECCIONADO
    tipo = true;
});

function saveSeller() {
    //VARIABLES DE LA TABLA PROVEEDOR
    var NombreComercial, Telefono, RUC, EstadoProveedor, Local, RetenedorIR, NombreProveedor, ApellidoProveedor, CedulaProveedor;
    //ASIGNANDO VALORES GENERALES
    Telefono = $("#telefono").val();
    RUC = $("#ruc").val();
    EstadoProveedor = true;
    RetenedorIR = $(".ir").is(":checked");
    //ASIGNANDO VALORES SEGUN EL TIPO DE PROVEEDOR
    if (tipo) {
        NombreComercial = "0";        
        Local = true;
        NombreProveedor = $("#nombre").val();
        ApellidoProveedor = $("#apellido").val();
        CedulaProveedor = $("#cedula").val();
    } else {
        NombreComercial = $("#nombre").val();        
        Local = false;
        NombreProveedor = "0";
        ApellidoProveedor = "0";
        CedulaProveedor = "0";
    }//FIN IF-ELSE

    //FUNCION AJAX
    $.ajax({
        type: "POST",
        url: "/Proveedores/Create",
        data: {
            //VALORES A ALMACENAR
            NombreComercial, Telefono, RUC, EstadoProveedor, Local, RetenedorIR, NombreProveedor, ApellidoProveedor, CedulaProveedor
        },
        success: function (data) {
            if (data.data) {
                $("#Table").DataTable().ajax.reload(); //RECARGAR DATATABLE PARA VER LOS CAMBIOS
                $("#small-modal").modal("hide"); //CERRAR MODAL
                Alert("Almacenado correctamente", "", "success");
            } else
                Alert("Error al almacenar", "", "error");
        },
        error: function () {
            Alert("Error al almacenar", "Intentelo de nuevo", "error");
        }
    });//FIN AJAX
}//FIN FUNCTION

function getSellerInfo(id) {

    $("#small-modal").modal("show"); //MUESTRA LA MODAL
    $("#VistaParcial").html("");//LIMPIA LA MODAL POR DATOS PRECARGADOS

    $.ajax({
        type: "GET",
        url: "/Proveedores/Edit/id",
        data: {
            id: id
        },
        success: function (list) {
            $("#VistaParcial").html(parcial);
            if (list.Local) {
                $("#nombre").val(list.NombreProveedor);
                $("#apellido").val(list.ApellidoProveedor);
                $("#cedula").val(list.CedulaProveedor);
                $("#telefono").val(list.Telefono);
                $("#ruc").val(list.RUC);
                $("#estado").val(list.RUC);
                $("#ir").val(list.RUC);
                $("#tipo").val(list.Local);
            } else {
                $("#nombre").val(list.NombreProveedor);
                $("#telefono").val(list.Telefono);
                $("#ruc").val(list.RUC);
                $("#estado").val(list.RUC);
                $("#ir").val(list.RUC);
                $("#tipo").val(list.Local);
            }
        }
    });
}

function Alert(message, algo, status) {
    swal({
        title: message,
        text: algo,
        icon: status
    });//FIN DEL SWEET ALERT
}//FIN FUNCION