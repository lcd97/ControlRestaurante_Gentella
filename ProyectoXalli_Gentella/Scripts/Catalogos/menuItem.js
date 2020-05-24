//FUNCION PARA ALMACENAR AL PROVEEDOR
function saveMenuItem() {
    //SE CREA UN OBJETO DE LA CLASE FORMDATA
    var formData = new FormData();

    //USANDO EL METODO APPEND(CLAVE, VALOR) SE AGREGAN LOS PARAMETROS A ENVIAR
    formData.append("urlImage", $("#file")[0].files[0]);
    formData.append("codigoMenu", $("#codigo").val());
    formData.append("descripcionMenu", $("#platillo").val());
    formData.append("precio", $("#precio").val());
    formData.append("categoriaId", $("#categoria").val());
    formData.append("tiempo", $("#tiempo").val());
    formData.append("ingredientes", $("#ingredientes").val());

    //ESTA ES OTRA MANERA DE MANDAR PARAMETROS AL CONTROLADOR. EN ESTE CASO NO FUNCIONO
    //var data = "urlImage=" + formData + "&codigoMenu=" + codigoMenu + "&descripcionMenu=" + descripcionMenu;

    //FUNCION AJAX
    $.ajax({
        type: "POST",
        url: "/Menus/Create",
        data: formData,//SE ENVIA TODO EL OBJETO FORMDATA CON LOS PARAMETROS EN EL APPEND 
        processData: false,
        contentType: false,
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