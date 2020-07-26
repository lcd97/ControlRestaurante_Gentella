﻿$(document).ready(function () {
    //CARGA TODOS LO PLATILLOS DEL MENU
    $.ajax({
        type: "GET",
        url: "/Menus/GetData",
        dataType: "JSON",
        success: function (data) {
            var agregarPlatillo = "";
            if (data.length <= 0) {//SI NO HAY NINGUN ELEMENTO
                $("#filtro").attr("disabled", true);//DESHABILITAR EL INPUT FILTRAR

                agregarPlatillo = '<h2 id="txt" style="text-align:center;">Sin elementos disponibles</h2>';//AGREGA LETRERO
            } else {
                $("#filtro").removeAttr("disabled");//HABILITAR EL INPUT FILTRAR

                for (var i = 0; i < data.length; i++) {
                    agregarPlatillo += '<div class="col-md-55 items" id="' + data[i].Id + '">' +
                        '<div class="thumbnail">' +
                        '<div class="image view view-first">' +
                        '<img style="width: 100%; height:100%; display: block;" src="' + data[i].Imagen + '"alt="' + data[i].DescripcionPlatillo + '" />' +
                        '<div class="mask no-caption">' +
                        '<div class="tools tools-bottom">' +
                        '<a onclick=CargarParcial("/Menus/Edit/' + data[i].Id + '")><i class="fa fa-pencil"></i></a>' +
                        '<a onclick=CargarParcial("/Menus/Details/' + data[i].Id + '")><i class="fa fa-eye"></i></a>' +
                        '<a onclick=deleteAlert("/Menus/Delete/",' + data[i].Id + ')><i class="fa fa-trash"></i></a>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '<div class="caption" id="data">' +
                        '<p>' +
                        '<strong>' + data[i].DescripcionPlatillo + '</strong>' +
                        '</p>' +
                        '<p> $ ' + data[i].Precio + '</p>' +
                        '</div>' +
                        '</div >' +
                        '</div >';
                }//FIN FOR
            }//IF IF-ELSE

            //AGREGA LOS ELEMENTOS
            $("#menuAdd").html(agregarPlatillo);
        }//FIN SUCCESS
    });
});

//TIPO DE METODO A UTILIZAR
function comprobar() {
    //AGARRAR EL CODIGO DE LA VISTA
    var codigoMenu = $("#codigoMenu").val();

    $.ajax({
        type: "GET",
        url: "/Menus/Comprobar/",
        data: { codigoMenu },
        success: function (data) {
            validar(data);
        }
    });
}//FIN FUNCTION

//FUNCION VALIDA QUE NO EXISTAN CAMPOS VACIOS
function validar(nuevo) {
    var a = $("#platillo").val();
    var img = $("#file")[0].files[0];
    var code = $("#codigoMenu").val();
    var precio = $("#precio").val();
    var time = $("#tiempo").val();
    var ingredients = $("#ingredientes").val();

    //SI ES VERDADERO
    if (nuevo == true) {
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
}//FIN FUNCTION

//FUNCION PARA ALMACENAR PLATILLO DEL MENU
function saveMenuItem() {
    //SE CREA UN OBJETO DE LA CLASE FORMDATA
    var formData = new FormData();
    var a = $("#platillo").val();

    //USANDO EL METODO APPEND(CLAVE, VALOR) SE AGREGAN LOS PARAMETROS A ENVIAR
    formData.append("urlImage", $("#file")[0].files[0]);
    formData.append("codigoMenu", $("#codigoMenu").val());
    //ASIGNAR DESCRIPCION DE MENU CON LA PRIMERA LETRA EN MAYUSCULA
    formData.append("descripcionMenu", a.charAt(0).toUpperCase() + a.slice(1).toLowerCase());
    formData.append("precio", $("#precio").val());
    formData.append("categoriaId", $("#categoria").val());
    formData.append("tiempo", $("#tiempo").val());
    formData.append("ingredientes", $("#ingredientes").val());

    $.ajax({
        type: "POST",
        url: "/Menus/Create/",
        data: formData, //SE ENVIA TODO EL OBJETO FORMDATA CON LOS PARAMETROS EN EL APPEND ,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success === true) {
                $("#small-modal").modal("hide"); //CERRAR MODAL
                swal({
                    title: "Completado",
                    text: data.message,
                    icon: "success",
                    buttons: false,
                    timer: 1500
                });
                agregarItem(data.Id);//AGREGAR EL ELEMENTO CREADO
            } else {
                Alert("Error", data.message, "error");//ALMACENADO CORRECTAMENTE
            }
        }
    });
}

//FUNCION PARA ALMACENAR PLATILLO DEL MENU
function editMenuItem() {
    //SE CREA UN OBJETO DE LA CLASE FORMDATA
    var formData = new FormData();
    var a = $("#platillo").val();

    //USANDO EL METODO APPEND(CLAVE, VALOR) SE AGREGAN LOS PARAMETROS A ENVIAR
    formData.append("urlImage", $("#file")[0].files[0]);
    formData.append("codigoMenu", $("#codigoMenu").val());
    //ASIGNAR DESCRIPCION DE MENU CON LA PRIMERA LETRA EN MAYUSCULA
    formData.append("descripcionMenu", a.charAt(0).toUpperCase() + a.slice(1).toLowerCase());
    formData.append("precio", $("#precio").val());
    formData.append("categoriaId", $("#categoria").val());
    formData.append("tiempo", $("#tiempo").val());
    formData.append("ingredientes", $("#ingredientes").val());
    //TOMAR EL VALOR DEL SWITCHERY
    formData.append("estado", $("#activo").is(":checked"));

    //FUNCION AJAX
    $.ajax({
        type: "POST",
        url: "/Menus/Edit",
        data: formData,//SE ENVIA TODO EL OBJETO FORMDATA CON LOS PARAMETROS EN EL APPEND 
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success) {
                $("#small-modal").modal("hide"); //CERRAR MODAL
                modificarItem(data.Id);//MODIFICAR EL REGISTRO EN LA VISTA INDEX
                swal({
                    title: "Completado",
                    text: data.message,
                    icon: "success",
                    buttons: false,
                    timer: 1500
                });
            } else
                Alert("Error al modificar", data.message, "error");//MENSAJE DE ERROR
        },
        error: function () {
            Alert("Error al modificar", "Intentelo de nuevo", "error");
        }
    });//FIN AJAX
}//FIN FUNCTION

//MODIFICAR EL DIV DEL ELEMENTO DEL MENU
function modificarItem(id) {
    $.ajax({
        type: "GET",
        url: "/Menus/getMenuItem/" + id,
        success: function (data) {
            //ASIGNAR NUEVOS VALORES EN LA VISTA
            $("#" + id).find("strong").text(data.Platillo);
            $("#" + id).find("p:last-child").text(data.Precio);
        },
        error: function () {
            Alert("Error al modificar", "Intentelo de nuevo", "error");
        }
    });//FIN AJAX      
}//FIN FUNCTION

//AGREGAR UN CAMPO MAS AL INDEX
function agregarItem(id) {

    $.ajax({
        type: "POST",
        url: "/Menus/getMenuItem/" + id,
        success: function (data) {
            //CREAR HTML DE OBJETO CREADO
            var agregarMenu = '<div class="col-md-55 items" id="' + data.menu.PlatilloId + '">' +
                '<div class="thumbnail">' +
                '<div class="image view view-first">' +
                '<img style="width: 100%; height:100%; display: block;" src="' + data.menu.Ruta + '"alt="' + data.menu.Platillo + '" />' +
                '<div class="mask no-caption">' +
                '<div class="tools tools-bottom">' +
                '<a onclick=CargarParcial("/Menus/Edit/' + data.menu.PlatilloId + '")><i class="fa fa-pencil"></i></a>' +
                '<a onclick=CargarParcial("/Menus/Detail/' + data.menu.PlatilloId + '")><i class="fa fa-eye"></i></a>' +
                '<a onclick=deleteAlert("/Menus/Delete/",' + data.menu.PlatilloId + ')><i class="fa fa-trash"></i></a>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '<div class="caption" id="data">' +
                '<p>' +
                '<strong>' + data.menu.Platillo + '</strong>' +
                '</p>' +
                '<p> $ ' + data.menu.Precio + '</p>' +
                '</div>' +
                '</div >' +
                '</div >';

            $("#menuAdd").prepend(agregarMenu);//AGREGARLO DE PRIMERO
        },
        error: function () {
            Alert("Error al almacenar", "Intentelo de nuevo", "error");
        }
    });//FIN AJAX      
}//FIN FUNCTION

//EVENTO KEY-UP PARA FILTRAR
$("#filtro").keyup(function () {

    var value = $(this).val().toLowerCase().trim();//TOMA EL VALOR DIGITADO
    filtrar(value);//ENVIO VALOR PARA FILTRAR COINCIDENCIAS
});

//FUNCION FILTRA LOS ELEMENTOS DEL DIV
function filtrar(value) {
    $(".items").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);//SI ES DIFERENTE A -1 ES QUE ENCONTRO
    });
}

//FUNCION PARA CARGAR LA MODAL DEL CRUD -- SOLO PARA MOSTRAR LOS CAMPOS DEL OBJETO
function CargarParcial(url) { //RECIBE LA URL DE LA UBICACION DEL METODO
    $("#small-modal").modal("show"); //MUESTRA LA MODAL
    $("#VistaParcial").html("");//LIMPIA LA MODAL POR DATOS PRECARGADOS

    //AGREGAR EL TITULO A LA MODAL
    if (url.includes("Edit")) {
        $("#modal-title").html("Editar");
    } else
        if (url.includes("Create")) {
            $("#modal-title").html("Ingresar nuevo");
        } else
            if (url.includes("Details")) {
                $("#modal-title").html("Detalle");
            }

    $.ajax({
        "type": "GET", //TIPO DE ACCION
        "url": url, //URL DEL METODO A USAR
        success: function (parcial) {
            $("#VistaParcial").html(parcial);//CARGA LA PARCIAL CON ELEMENTOS QUE CONTEGA
        }//FIN SUCCESS
    });//FIN AJAX
}//FIN FUNCTION

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

//FUNCION POST PARA ELIMINAR UN REGISTRO (CRUD) -- SOLO PARA ELIMINAR (RECARGA LA PAGINA CON AJAX -OJO-)
function Delete(uri, id) {

    //COMIENZO DE LA PETICION AJAX PARA ELIMINAR REGISTRO
    $.ajax({
        type: "POST", //TIPO DE ACCION
        url: uri,//ACCION O METODO A REALIZAR
        data: { "id": id }, //SERIALIZACION DE LOS DATOS A ENVIAR
        success: function (data) {
            if (data.success) {//SI SE ELIMINO CORRECTAMENTE
                $("#" + id).remove(); //ELIMINA EL DIV DEL PLATILLO
                swal({
                    title: data.message,
                    text: "El registro ha sido eliminado",
                    icon: "success",
                    buttons: false,
                    timer: 1500
                });
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