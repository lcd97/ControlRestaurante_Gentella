//CARGA EL CODIGO DE LA ORDEN AUTOMATICAMENTE
function cargarCodigo() {
    $.ajax({
        type: "GET",
        url: "/Ordenes/OrdenesCode",
        success: function (data) {
            $("#codigo").val(data.data);
        }
    });
}

//CARGA DE ORDEN EL CODIGO A LA VISTA
$(document).ready(function () {
    cargarCodigo();

    //INICIALIZAR EL SELECT2 CATEGORIA
    $("#categoria").val("-1");
    $('#categoria').trigger('change'); // Notify any JS components that the value changed

    //AGREGAR EL MENSAJE PRINCIOAL DE SECCION MENU
    var agregar = '<h2 id="nada" style="text-align:center;" col-md-12 col-sm-12 col-xs-12>Seleccione una categoría</h2>';
    $("#menuAdd").append(agregar);
});

//INICIALIZADOR DE DATEPICKER
$('#fechaOrden').datetimepicker({
    format: 'DD/MM/YYYY',
    defaultDate: new Date()
});

//AGREGAR LA OPCION DONDE IRA EL PLACEHOLDER DEL SELECT 2
$(".js-example-basic-single").prepend("<option value='-1' readonly></option>");

//INICIALIZADOR DE LENGUAJE SELECT 2
$('.js-example-basic-single').select2({
    placeholder: { id: "-1", text: "Seleccione" },//CARGAR PRIMERO EL PLACEHOLDER
    //MODIFICAR LAS FRASES DEFAULT DE SELECT2
    language: {

        noResults: function () {

            return "No hay resultado";
        },
        searching: function () {

            return "Buscando...";
        }
    }
});

//CAMBIAR EL MENU POR CATEGORIAS
$("#categoria").change(function () {
    //ALMACENO EL ID DEL SELECT 2
    var categoriaId = $("#categoria").val();

    //SI CATEGORIA ES DIFERENTE AL PLACEHOLDER
    if (categoriaId != "") {
        $.ajax({
            type: "GET",
            url: "/Ordenes/MenuByCategoria/" + categoriaId,
            dataType: "JSON",
            success: function (data) {
                //SI NO HAY ELEMENTOS
                if (data.data.length <= 0) {
                    $("#filtro").attr("disabled", true);//DESACTIVO EL INPUT DE FILTRO

                    //LIMPIAR BUSQUEDA ANTERIOS
                    deleteRows();

                    var agregar = '<h2 id="nada" style="text-align:center;" col-md-12 col-sm-12 col-xs-12>Sin elementos disponibles</h2>';
                    $("#menuAdd").append(agregar);
                } else if (data.data.length > 0) {//SI HAY ELEMENTOS
                    var agregarMenu = "";
                    $("#filtro").removeAttr("disabled");

                    //ELIMINAR BUSQUEDA ANTERIOR
                    deleteRows();

                    var paginarAdd = '<nav id="paginar" aria-label="Page navigation example" class="col-md-12 col-sm-12 col-xs-12">' +
                        '<ul class="pagination" id="myPager"></ul>' +
                        '</nav>';

                    //RECORRER TODOS LOS ELEMENTOS A MOSTRAR
                    for (var i = 0; i < data.data.length; i++) {
                        agregarMenu += '<div class="col-md-4 items" id="' + data.data[i].Id + '">' +//SE LE ASIGNA UN IDENTIFICADOR PARA REALIZAR EL CRUD Y ACTUALIZAR VISTA
                            '<div class="thumbnail">' +
                            '<div class="image view view-first">' +
                            '<img style="width: 100%; height:100%; display: block;" src="' + data.data[i].Imagen + '"alt="' + data.data[i].DescripcionPlatillo + '" />' +
                            '<div class="mask no-caption">' +
                            '<div class="tools tools-bottom">' +
                            '<a onclick=CargarParcial("/Menus/Detail/' + data.data[i].Id + '")><i class="fa fa-plus"></i></a>' +
                            '<a onclick=CargarParcial("/Menus/Detail/' + data.data[i].Id + '")><i class="fa fa-eye"></i></a>' +
                            '</div>' +
                            '</div>' +
                            '</div>' +
                            '<div class="caption" id="data">' +
                            '<p>' +
                            '<strong>' + data.data[i].Platillo + '</strong>' +
                            '</p>' +
                            '<p> $ ' + data.data[i].Precio + '</p>' +
                            '</div>' +
                            '</div >' +
                            '</div >';
                    }

                    $("#menuAdd").append(agregarMenu);
                    $("#menuAdd").append(paginarAdd);

                    cargarPaginacion();
                }//FIN IF-ELSE ELEMENTOS
            }
        });
    } else {
        $("#filtro").attr("disabled", true);//DESACTIVO EL INPUT DE FILTRO

        //LIMPIAR BUSQUEDA ANTERIOS
        deleteRows();

        var agregar = '<h2 id="nada" style="text-align:center;" col-md-12 col-sm-12 col-xs-12>Seleccione una categoría</h2>';
        $("#menuAdd").append(agregar);
    }
});

//ELIMINA TODOS LOS ELEMENTOS DENTRO DE UN DIV
function deleteRows() {
    //TOMO EL DIV PRINCIPAL (PADRE)
    var element = document.getElementById("menuAdd");

    //RECORRO EL ELEMENTO Y BORRO TODOS LOS DIV CHILD
    while (element.firstChild) {
        element.removeChild(element.firstChild);
    }//FIN CICLO

    //ELIMINAR LA PAGINACION
    $('#paginar').remove();
}

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

//CARGA LA PAGINACION DE LOS ELEMENTOS CARGADOS
function cargarPaginacion() {
    $('#menuAdd').pageMe({ pagerSelector: '#myPager', showPrevNext: true, hidePageNumbers: false, perPage: 3 });
}

$("#filtro").on("keyup", function () {
    var value = $(this).val().toLowerCase().trim();

    //SI EL INPUT TIENE VALORES VACIOS
    if (value == "") {
        deletePagination();//ELIMINO LA PAGINACION
        agregarPagination();//AGREGO E INICIALIZO LA PAGINACION
        
    } else {//SI EL INPUT ES DIFERENTE A VACIO
        deletePagination();//ELIMINO LA PAGINACION
        //FILTRA LOS DATOS A BUSCAR
        filtrar($(this).val().toLowerCase());
    }
});

//FUNCION PARA FILTRAR ELEMENTOS DEL MENU
function filtrar(value) {
    $(".items").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);//SI ES DIFERENTE A -1 ES QUE ENCONTRO
    });
}

//FUNCION PARA CREAR LA PAGINACION
function agregarPagination() {
    //CREAR LA PAGINACION
    var paginationAdd = '<nav id="paginar" aria-label="Page navigation example" class="col-md-12 col-sm-12 col-xs-12">' +
        '<ul class="pagination" id="myPager"></ul>' +
        '</nav>';

    //AGREGAR LA PAGINACION AL FINAL
    $('#menuAdd').append(paginationAdd);

    //INICIALIZAR PAGINACION
    cargarPaginacion();
}//FIN FUNCTION

//FUNCION PARA LIMPIAR PAGINACION
function deletePagination() {
    $("#paginar").remove();
}//FIN FUNCTION