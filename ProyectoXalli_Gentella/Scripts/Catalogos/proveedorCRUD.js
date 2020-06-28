//FUNCION PARA ALMACENAR AL PROVEEDOR
function saveSeller() {
    //VARIABLES DE LA TABLA PROVEEDOR
    var NombreComercial, Telefono, RUC, Local, RetenedorIR, NombreProveedor, ApellidoProveedor, CedulaProveedor;
    //ASIGNANDO VALORES GENERALES
    Telefono = $("#telefono").val();
    RUC = $("#ruc").val();
    //EstadoProveedor = true;
    RetenedorIR = $(".ir").is(":checked");

    Local = $('.btn-group > .btn.active').attr("value");
    //ASIGNANDO VALORES SEGUN EL TIPO DE PROVEEDOR
    if (Local == "true") {
        NombreComercial = "0";
        NombreProveedor = $("#nombre").val();
        ApellidoProveedor = $("#apellido").val();
        CedulaProveedor = $("#cedula").val();
    } else {
        NombreComercial = $("#nombre").val();
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
            NombreComercial: NombreComercial, Telefono: Telefono, RUC: RUC, Local: Local, RetenedorIR: RetenedorIR, NombreProveedor: NombreProveedor, ApellidoProveedor: ApellidoProveedor, CedulaProveedor: CedulaProveedor
        },
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
}//FIN FUNCTION

//FUNCION PARA ACTUALIZAR CAMPOS DEL PROVEEDOR
function UpdateProvider(Id) {
    //VARIABLES DE LA TABLA PROVEEDOR
    var NombreComercial, Telefono, RUC, EstadoProveedor, Local, RetenedorIR, NombreProveedor, ApellidoProveedor, CedulaProveedor;
    //ASIGNANDO VALORES GENERALES
    Telefono = $("#telefono").val();
    RUC = $("#ruc").val();
    EstadoProveedor = $(".activo").is(":checked");
    RetenedorIR = $(".ir").is(":checked");
    Local = $('.btn-group > .btn.active').attr("value");
    //ASIGNANDO VALORES SEGUN EL TIPO DE PROVEEDOR
    if (Local == "true") {
        NombreProveedor = $("#nombre").val();
        ApellidoProveedor = $("#apellido").val();
        CedulaProveedor = $("#cedula").val();
    } else {
        NombreComercial = $("#nombre").val();
    }//FIN IF-ELSE

    //FUNCION AJAX
    $.ajax({
        type: "POST",
        url: "/Proveedores/UpdateProveedor/" + Id,
        data: {
            //VALORES A ALMACENAR
            Id, NombreComercial, Telefono, RUC, EstadoProveedor, Local, RetenedorIR, NombreProveedor, ApellidoProveedor, CedulaProveedor
        },
        dataType: "JSON",
        success: function (data) {
            if (data.success) {
                $("#Table").DataTable().ajax.reload(); //RECARGAR DATATABLE PARA VER LOS CAMBIOS
                $("#small-modal").modal("hide"); //CERRAR MODAL
                Alert(data.message, "","success");//ALMACENADO CORRECTAMENTE
            } else
                Alert("Error", data.message, "error");//MENSAJE DE ERROR
        },
        error: function () {
            Alert("Error", "Intentelo de nuevo", "error");
        }
    });//FIN AJAX   
}