//TIPO DE PROVEEDOR
var tipo = false;

//FUNCION PARA CARGAR LA MODAL DEL CRUD -- SOLO PARA MOSTRAR LOS CAMPOS DEL OBJETO
function CargarParcial(url) { //RECIBE LA URL DE LA UBICACION DEL METODO
    $("#small-modal").modal("show"); //MUESTRA LA MODAL
    $("#VistaParcial").html("");//LIMPIA LA MODAL POR DATOS PRECARGADOS
    $.ajax({
        "type": "GET", //TIPO DE ACCION
        "url": url, //URL DEL METODO A USAR
        success: function (parcial) {
            $("#VistaParcial").html(parcial);//CARGA LA PARCIAL CON ELEMENTOS QUE CONTEGA
        }//FIN SUCCESS
    });//FIN AJAX
}//FIN FUNCTION

//FUNCION PARA HACER EL CRUD A BODEGA POR MEDIO DEL MODAL (RECIBE UN FORM = FORMULARIO)
function SubmitForm(form) {
    $.validator.unobtrusive.parse(form);
    //if ($(form).valid()) {
        $.ajax({
            type: "POST", //TIPO DE ACCION
            url: form.action, //ACCION O METODO A REALIZAR
            data: $(form).serialize(), //SERIALIZACION DE LOS DATOS A ENVIAR
            success: function (data) {
                if (data.success) {//SI SE REALIZO CORRECTAMENTE
                    $("#Table").DataTable().ajax.reload(); //RECARGAR DATATABLE PARA VER LOS CAMBIOS
                    $("#small-modal").modal("hide"); //CERRAR MODAL

                    //MOSTRANDO EL SWEET ALERT
                    Alert(data.message, "", "success");

                }//FIN IF
                else
                    //MANDAR EL ERROR DEL CONTROLADOR
                    Alert("Error", data.message ,"error");
            },//FIN SUCCESS
            error: function () {
                //AQUI MANDAR EL MENSAJE DE ERROR
                Alert("Error al almacenarlo", "Intentelo de nuevo", "error");
            }//FIN ERROR
        });//FIN AJAX
    //}//FIN DEL IF FORM VALID
    return false; //EVITA SALIRSE DEL METODO ACTUAL
}//FIN FUNCTION

//FUNCION POST PARA ELIMINAR UN REGISTRO (CRUD) -- SOLO PARA ELIMINAR (RECARGA LA PAGINA CON AJAX -OJO-)
function Delete(uri, id) {

    //COMIENZO DE LA PETICION AJAX PARA ELIMINAR REGISTRO
    $.ajax({
        type: "POST", //TIPO DE ACCION
        url: uri,//ACCION O METODO A REALIZAR
        data: { "id": id }, //SERIALIZACION DE LOS DATOS A ENVIAR
        success: function (data) {
            if (data.success) {//SI SE ELIMINO CORRECTAMENTE
                $("#Table").DataTable().ajax.reload(); //RECARGAR DATATABLE PARA VER LOS CAMBIOS
                swal(data.message, { icon: "success" });
            }//FIN IF
            else
                swal("Error", data.message, "error");
        },//FIN SUCCESS
        error: function() {
            swal("Error", "Vuelvalo a intentar", "error");
        }//FIN ERROR
    });//FIN AJAX

    return false;

}//FIN FUNCTION

//FUNCION PARA CERRAR LA MODAL
function CerrarModal() {
    $("#small-modal").modal("hide"); //CERRAR MODAL                
    $("#xs-modal").modal("hide"); //CERRAR MODAL                
}//FIN FUNCION

//MANDAR EL SWEET ALERT PARA CREAR/EDITAR
function Alert(message, algo, status) {
    swal({
        title: message,
        text: algo,
        icon: status
    });//FIN DEL SWEET ALERT
}//FIN FUNCION

//MANDAR EL SWEET ALERT PARA ELIMINAR
function deleteAlert(uri, id) {
    swal({
        title: "¿Desea eliminar el registro?",
        text: "Una vez eliminado no se podrá volver a recuperar",
        icon: "warning",
        buttons: {
            cancel: "Cancelar",
            catch: {
                text: "Eliminar",
                value: "catch"
            }
        }//FIN DE BUTTONS
    })//FIN DEL SWAL

        .then((value) => {
            switch (value) {

                case "catch": Delete(uri, id);
                    break;

                default:
                    swal.close();
            }//FIN SWITCH
        });//FIN THEN
}//FIN FUCTION DELETE

//FUNCION PARA ALMACENAR AL PROVEEDOR
function saveSeller() {
    //VARIABLES DE LA TABLA PROVEEDOR
    var NombreComercial, Telefono, RUC, EstadoProveedor, Local, RetenedorIR, NombreProveedor, ApellidoProveedor, CedulaProveedor;
    //ASIGNANDO VALORES GENERALES
    Telefono = $("#telefono").val();
    RUC = $("#ruc").val();
    EstadoProveedor = true;
    RetenedorIR = $(".ir").is(":checked");

    tipo = $('.btn-group > .btn.active').attr("value");
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
    if (Local=="true") {
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
                Alert("Almacenado correctamente", data.message, "success");//ALMACENADO CORRECTAMENTE
            } else
                Alert("Error al almacenar", data.message, "error");//MENSAJE DE ERROR
        },
        error: function () {
            Alert("Error al almacenar", "Intentelo de nuevo", "error");
        }
    });//FIN AJAX   
}

$(document).ready(function () {
    var screen = $('#Cargando'); //obtiene modal Cargando
    configureLoadingScreen(screen); //llamada a metodo usando AJAX
});

function configureLoadingScreen(screen) {  // metodo para mostrar Loader
    $(document)
        .ajaxStart(function () { //muestra imagen
            screen.modal("show");
        })
        .ajaxStop(function () { //oculta imagen
            screen.modal("hide");
        });
}

