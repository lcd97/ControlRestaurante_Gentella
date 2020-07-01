//FUNCION PARA ALMACENAR PLATILLO DEL MENU
function saveMenuItem() {
    //SE CREA UN OBJETO DE LA CLASE FORMDATA
    var formData = new FormData();
    var a = $("#platillo").val();

    //USANDO EL METODO APPEND(CLAVE, VALOR) SE AGREGAN LOS PARAMETROS A ENVIAR
    formData.append("urlImage", $("#file")[0].files[0]);
    formData.append("codigoMenu", $("#codigo").val());
    //ASIGNAR DESCRIPCION DE MENU CON LA PRIMERA LETRA EN MAYUSCULA
    formData.append("descripcionMenu", a.charAt(0).toUpperCase() + a.slice(1).toLowerCase());
    formData.append("precio", $("#precio").val());
    formData.append("categoriaId", $("#categoria").val());
    formData.append("tiempo", $("#tiempo").val());
    formData.append("ingredientes", $("#ingredientes").val());

    //ESTA ES OTRA MANERA DE MANDAR PARAMETROS AL CONTROLADOR. EN ESTE CASO NO FUNCIONO
    //var data = "urlImage=" + formData + "&codigoMenu=" + codigoMenu + "&descripcionMenu=" + descripcionMenu;
   
    $.ajax({
        type: "POST",
        url: "/Menus/Create",
        data: formData,//SE ENVIA TODO EL OBJETO FORMDATA CON LOS PARAMETROS EN EL APPEND 
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.data) {
                //$("#Table").DataTable().ajax.reload(); //RECARGAR DATATABLE PARA VER LOS CAMBIOS
                $("#small-modal").modal("hide"); //CERRAR MODAL
                //AGREGAR EL NUEVO PLATILLO AL INDEX
                agregarItem(data.Id);
                Alert("Almacenado correctamente", data.message, "success");//ALMACENADO CORRECTAMENTE                
            } else
                Alert("Error al almacenar", data.message, "error");//MENSAJE DE ERROR
        },
        error: function () {
            Alert("Error al almacenar", "Intentelo de nuevo", "error");
        }
    });//FIN AJAX   
    //    } else {
    //    }
    //} else {
    //    Alert("Error", "Campos Vacios", "error");
    //}

}//FIN FUNCTION

//FUNCION PARA ALMACENAR PLATILLO DEL MENU
function editMenuItem() {
    //SE CREA UN OBJETO DE LA CLASE FORMDATA
    var formData = new FormData();
    var a = $("#platillo").val();

    //USANDO EL METODO APPEND(CLAVE, VALOR) SE AGREGAN LOS PARAMETROS A ENVIAR
    formData.append("urlImage", $("#file")[0].files[0]);
    formData.append("codigoMenu", $("#codigo").val());
    //ASIGNAR DESCRIPCION DE MENU CON LA PRIMERA LETRA EN MAYUSCULA
    formData.append("descripcionMenu", a.charAt(0).toUpperCase() + a.slice(1).toLowerCase());
    formData.append("precio", $("#precio").val());
    formData.append("categoriaId", $("#categoria").val());
    formData.append("tiempo", $("#tiempo").val());
    formData.append("ingredientes", $("#ingredientes").val());
    //TOMAR EL VALOR DEL SWITCHERY
    formData.append("estado", $("#activo").is(":checked"));

    //VALIDACION
    var code = $("#codigo").val();
    var precio = $("#precio").val();
    var time = $("#tiempo").val();
    var ingredients = $("#ingredientes").val();

    
        //FUNCION AJAX
    $.ajax({
        type: "POST",
        url: "/Menus/Edit",
        data: formData,//SE ENVIA TODO EL OBJETO FORMDATA CON LOS PARAMETROS EN EL APPEND 
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.data) {
                //$("#Table").DataTable().ajax.reload(); //RECARGAR DATATABLE PARA VER LOS CAMBIOS
                $("#small-modal").modal("hide"); //CERRAR MODAL
                modificarItem(data.Id);//MODIFICAR EL REGISTRO EN LA VISTA INDEX
                Alert("Almacenado correctamente", data.message, "success");//ALMACENADO CORRECTAMENTE
            } else
                Alert("Error al almacenar", data.message, "error");//MENSAJE DE ERROR
        },
        error: function () {
            Alert("Error al almacenar", "Intentelo de nuevo", "error");
        }
    });//FIN AJAX
    //} else {
    //    Alert("Error", "Campos Vacios", "error");
    //}
}//FIN FUNCTION

//TIPO DE METODO A UTILIZAR
function comprobar() {
    //AGARRAR EL CODIGO DE LA VISTA
    var codigo = $("#codigo").val();

    $.ajax({
        type: "GET",
        url: "/Menus/Comprobar/",
        data: { codigo },
        success: function (data) {
            //if (data.completado) {
            //    saveMenuItem();//METODO PARA ALMACENAR UN NUEVO ELEMENTO
            //}
            //else
            //    editMenuItem();//METODO PARA EDITAR UN ELEMENTO
            validar(data.completado);
        }
    });
}

//AGREGAR UN CAMPO MAS AL INDEX
function agregarItem(id) {
    $.ajax({
        type: "POST",
        url: "/Menus/getDetail/" + id,
        success: function (data) {
            var agregarMenu = '<div class="col-md-55" id="' + data.data.Id + '">' +
                '<div class="thumbnail">' +
                '<div class="image view view-first">' +
                '<img style="width: 100%; height:100%; display: block;" src="' + data.data.Ruta + '"alt="' + data.data.Platillo +'" />' +
                '<div class="mask no-caption">' +
                '<div class="tools tools-bottom">' +
                '<a onclick=CargarParcial("/Menus/Edit/' + data.data.Id + '")><i class="fa fa-pencil"></i></a>' +
                '<a onclick=CargarParcial("/Menus/Detail/' + data.data.Id + '")><i class="fa fa-eye"></i></a>' +
                '<a onclick=deleteAlert("/Menus/Delete/",' + data.data.Id + ')><i class="fa fa-trash"></i></a>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '<div class="caption" id="data">' +
                '<p>' +
                '<strong>' + data.data.Platillo + '</strong>' +
                '</p>' +
                '<p> $ ' + data.data.Precio + '</p>' +
                '</div>' +
                '</div >' +
                '</div >';

            $("#menuAdd").append(agregarMenu);  
        },
        error: function () {
            Alert("Error al almacenar", "Intentelo de nuevo", "error");
        }
    });//FIN AJAX      
}

//MODIFICAR EL DIV DEL ELEMENTO DEL MENU
function modificarItem(id) {
    $.ajax({
        type: "GET",
        url: "/Menus/getDetail/" + id,
        success: function (data) {
            //ASIGNAR NUEVOS VALORES EN LA VISTA
            $("#" + id).find("strong").text(data.data.Platillo);
            $("#" + id).find("p:last-child").text(data.data.Precio);
        },
        error: function () {
            Alert("Error al almacenar", "Intentelo de nuevo", "error");
        }
    });//FIN AJAX      
}

//FUNCION POST PARA ELIMINAR UN REGISTRO (CRUD) -- SOLO PARA ELIMINAR (RECARGA LA PAGINA CON AJAX -OJO-)
function DeleteItem(uri, id) {

    //COMIENZO DE LA PETICION AJAX PARA ELIMINAR REGISTRO
    $.ajax({
        type: "POST", //TIPO DE ACCION
        url: uri,//ACCION O METODO A REALIZAR
        data: { "id": id }, //SERIALIZACION DE LOS DATOS A ENVIAR
        success: function (data) {
            if (data.success) {//SI SE ELIMINO CORRECTAMENTE
                //$("#Table").DataTable().ajax.reload(); //RECARGAR DATATABLE PARA VER LOS CAMBIOS
                //ELIMINAR EL DIV DEL OBJETO ELIMINADO
                $("div").remove("#" + id);
                swal(data.message, { icon: "success" });
            }//FIN IF
            else
                swal("Error", data.message, "error");
        },//FIN SUCCESS
        error: function () {
            swal("Error", "Vuelvalo a intentar", "error");
        }//FIN ERROR
    });//FIN AJAX

    return false;

}//FIN FUNCTION

//MANDAR EL SWEET ALERT PARA ELIMINAR
function deleteAlertItem(uri, id) {
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

                case "catch": DeleteItem(uri, id);
                    break;

                default:
                    swal.close();
            }//FIN SWITCH
        });//FIN THEN
}//FIN FUCTION DELETE

//FUNCION VALIDA QUE NO EXISTAN CAMPOS VACIOS
function validar(nuevo) {
    var a = $("#platillo").val();
    var img = $("#file")[0].files[0];
    var code = $("#codigo").val();
    var precio = $("#precio").val();
    var time = $("#tiempo").val();
    var ingredients = $("#ingredientes").val();

    //SI ES VERDADERO
    if (nuevo === true) {
        //VALIDACIONES SHAMPOO
        if (code !== "" && a !== "" && precio !== "" && time !== "" && ingredients !== "") {
            if (img != null) {
                saveMenuItem();//METODO PARA ALMACENAR UN NUEVO ELEMENTO
            } else {
                Alert("Error", "Adjunte una imagen", "error");
            }//FIN VALIDACION IMAGEN
        } else {
            Alert("Error", "Campos vacios", "error");
        }//FIN IF-ELSE VALIDACIONES
    } else {
        //VALIDACIONES SHAMPOO
        if (code !== "" && a !== "" && precio !== "" && time !== "" && ingredients !== "") {
            editMenuItem();//METODO PARA EDITAR UN ELEMENTO
        } else {
            Alert("Error", "Campos vacios", "error");
        }//FIN VALIDACIONES IF-ELSE
    }//FIN IF-ELSE NUEVO
}