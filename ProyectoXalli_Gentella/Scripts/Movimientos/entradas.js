//CARGA EL CODIGO DE LA ENTRADA AUTOMATICAMENTE
function cargarCodigo() {
    $.ajax({
        type: "GET",
        url: "/Entradas/EntradaCode",
        success: function (data) {
            $("#codigo").val(data.data);
        }
    });
}

//CARGA DE ENTRADA EL CODIGO A LA VISTA
$(document).ready(function () {
    cargarCodigo();
});

//INICIALIZADOR DE DATEPICKER
$('#myDatepicker2').datetimepicker({
    format: 'DD/MM/YYYY',
    defaultDate: new Date()
});

//INICIALIZADOR DE LENGUAJE SELECT 2
$('.js-example-basic-single').select2({
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

//CARGAR DATOS DE PROVEEDOR PARA SELECT2
$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: '/Entradas/getProveedor',
        dataType: 'JSON',
        success: function (data) {

            var agregar = "";      

            for (var i = 0; i < Object.keys(data.data).length; i++) {
                agregar += '<option value="' + data.data[i].Id + '">' + data.data[i].Proveedor + '</option>';
            }

            $("#proveedor").append(agregar);
        },
        error: function (data) {
            alert("Error");
        }
    });
});

//CARGAR DATOS DE PROVEEDOR PARA SELECT2
$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: '/Entradas/getTipoEntrada',
        dataType: 'JSON',
        success: function (data) {

            var agregar = "";

            for (var i = 0; i < Object.keys(data.data).length; i++) {
                agregar += '<option value="' + data.data[i].Id + '">' + data.data[i].Entrada + '</option>';
            }

            $("#entrada").append(agregar);
        },
        error: function (data) {
            alert("Error");
        }
    });
});

//CARGAR DATOS DE PROVEEDOR PARA SELECT2
$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: '/Entradas/getProductos',
        dataType: 'JSON',
        success: function (data) {

            var agregar = "";

            for (var i = 0; i < Object.keys(data.data).length; i++) {
                agregar += '<option value="' + data.data[i].Id + '">' + data.data[i].Presentacion + '</option>';
            }

            $("#producto").append(agregar);
        },
        error: function (data) {
            alert("Error");
        }
    });
});

//CARGAR DATOS DE PROVEEDOR PARA SELECT2
$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: '/Entradas/getArea',
        dataType: 'JSON',
        success: function (data) {

            var agregar = "";

            for (var i = 0; i < Object.keys(data.data).length; i++) {
                agregar += '<option value="' + data.data[i].Id + '">' + data.data[i].Bodega + '</option>';
            }

            $("#area").append(agregar);
        },
        error: function (data) {
            alert("Error");
        }
    });
});

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

//FUNCION PARA ALMACENAR AL PROVEEDOR
function saveSeller() {
    //VARIABLES DE LA TABLA PROVEEDOR
    var NombreComercial, Telefono, RUC, EstadoProveedor, Local, RetenedorIR, NombreProveedor, ApellidoProveedor, CedulaProveedor;
    //ASIGNANDO VALORES GENERALES
    Telefono = $("#telefono").val();
    RUC = $("#ruc").val();
    EstadoProveedor = true;
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
            NombreComercial, Telefono, RUC, EstadoProveedor, Local, RetenedorIR, NombreProveedor, ApellidoProveedor, CedulaProveedor
        },
        success: function (data) {
            if (data.data) {
                //AGREGAR EL REGISTRO AL SELECT 2
                var agregar = '<option value="' + data.Id + '">' + data.Proveedor + '</option>';

                $("#proveedor").append(agregar);

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

//MANDAR EL SWEET ALERT PARA CREAR/EDITAR
function Alert(message, algo, status) {
    swal({
        title: message,
        text: algo,
        icon: status
    });//FIN DEL SWEET ALERT
}//FIN FUNCION

//FUNCION PARA HACER EL CRUD A PRODUCTO POR MEDIO DEL MODAL (RECIBE UN FORM = FORMULARIO)
function SubmitForm(form) {
    $.validator.unobtrusive.parse(form);
    //if ($(form).valid()) {
    $.ajax({
        type: "POST", //TIPO DE ACCION
        url: form.action, //ACCION O METODO A REALIZAR
        data: $(form).serialize(), //SERIALIZACION DE LOS DATOS A ENVIAR
        success: function (data) {
            if (data.success) {//SI SE REALIZO CORRECTAMENTE
                //AGREGAR EL REGISTRO AL SELECT 2
                var agregar = '<option value="' + data.Id + '">' + data.Producto + '</option>';
                $("#producto").append(agregar);

                //CERRAR MODAL
                $("#small-modal").modal("hide"); //CERRAR MODAL

                //MOSTRANDO EL SWEET ALERT
                Alert(data.message, "", "success");

            }//FIN IF
            else
                //MANDAR EL ERROR DEL CONTROLADOR
                Alert("Error", data.message, "error");
        },//FIN SUCCESS
        error: function () {
            //AQUI MANDAR EL MENSAJE DE ERROR
            Alert("Error al almacenarlo", "Intentelo de nuevo", "error");
        }//FIN ERROR
    });//FIN AJAX
    //}//FIN DEL IF FORM VALID
    return false; //EVITA SALIRSE DEL METODO ACTUAL
}//FIN FUNCTION

function TableAdd() {
    //ALMACENAR LAS VARIABLES
    var producto = $("#producto").find("option:selected").val(), precio = $("#precio").val(), cantidad = $("#cantidad").val();
    var dec = Math.pow(10, 2);
    var precioTotal = parseInt((precio * cantidad) * dec, 10) / dec;
    var agregar = "";

    //VALIDAR QUE LOS CAMPOS NO ESTEN VACIOS
    if (producto === null || precio === "" || cantidad === "") {
        Alert("Error", "Llene todos los campos para agregar el producto", "error");
    } else {
        //GUARDAR LAS FILAS EXISTENTES DE LA TABLA
        var filas = $("#table_body").find("tr");
        var registrado = false, i = 0; 

        //RECORRER LOS VALORES DE LA TABLA
        while (i < filas.length && registrado === false) {
            var celdas = $(filas[i]).find("td"); //devolverá las celdas de una fila

            //AGARRAR EL VALUE ALMACENADO EN LA FILA - PRODUCTO
            var comp = $(celdas[0]).attr("value");

            //COMPARAMOS QUE EL PRODUCTO A INGRESAR NO SEA EL MISMO AL QUE YA ESTA AGREGADO
            if (comp === producto) {
                registrado = true;
            } else {
                registrado = false;
            }

            i++;
        }//FIN WHILE

        //SI NO SE EN
        if (!registrado) {
            //GENERAR FILA DEL PRODUCTO A LA TABLA
            agregar += '<tr class="even pointer">';
            agregar += '<td class="" value ="' + producto + '">' + $("#producto").find("option:selected").text() + '</td>';
            agregar += '<td class="" >' + "C$ " + precio + '</td>';
            agregar += '<td class="" >' + cantidad + '</td>';
            agregar += '<td class="" >' + "C$ " + precioTotal + '</td>';
            agregar += '<td class=" last"><a class="btn btn-primary" id="boton" onclick="editProduct(this);"><i class="fa fa-edit"></i></a>';
            agregar += '<a class="btn btn-danger" onclick = "deleteProduct(this);"> <i class="fa fa-trash"></i></a></td>';
            agregar += '</tr>';

            //CALCULAR EL TOTAL
            var total = parseFloat(CalcularTotal());

            //AGREGAR PRODUCTO A LA TABLA
            $("#table_body").append(agregar);
            //AGREGAR EL TOTAL TFOOT
            $("#total").html("C$ " + (total + precioTotal));

            //LIMPIAR LOS INPUTS Y SELECT
            $("#producto").val("-1");
            $('#producto').trigger('change'); // Notify any JS components that the value changed
            $("#precio").val("");
            $("#cantidad").val("");
        } else {
            Alert("Error", "El producto seleccionado ya se encuentra en la tabla", "error");
        }      
    }//FIN ELSE DE VALIDACION CAMPOS VACIOS
}//FIN FUNCTION

//FUNCION PARA EDITAR UN PRODUCTO DE LA TABLA
function editProduct(indice) {
    //EVENTO ONCLICK DEL BOTON EDITAR
    $("#productTable").on("click", "#boton", function () {
        //OBTENER LOS VALORES A UTILIZAR
        var prod = $(this).parents("tr").find("td").eq(0).attr("value");
        var precio = $(this).parents("tr").find("td").eq(1).html();
        var cantidad = $(this).parents("tr").find("td").eq(2).html();

        $("#producto").val(prod);
        $('#producto').trigger('change'); // Notify any JS components that the value changed

        //RECORTAR VALOR DE LA FILA PARA AGREGARLO AL INPUT
        var str = precio;
        var res = str.split("C$ ");
        $("#precio").val(res[1]);

        $("#cantidad").val(cantidad);        

        //RECALCULAR TOTAL TFOOT
        var totalF = $("#total").html();
        var prodPrecio = totalF.split("C$ ");
        var resta = parseFloat(prodPrecio[1] - (res[1] * cantidad));

        $("#total").html(resta);

        indice.closest("tr").remove();

        //deleteProduct(indice);     
    });
}//FIN FUNCTION

//FUNCION PARA ELIMINAR UNA FILA SELECCIONADA DE LA TABLA
function deleteProduct(row) {
    //SE BUSCA LA POSICION DE LA FILA SELECCIONADA PARA ELIMINARLA
    var indice = row.parentNode.parentNode.rowIndex;
    document.getElementById('productTable').deleteRow(indice);

    //RECALCULAMOS EL TOTAL
    var resta = parseFloat(CalcularTotal());
    $("#total").html("C$ " + resta);
    
}//FIN FUNCTION

//FUNCION PARA CALCULAR EL TOTAL GENERAL DE LA TABLA
function CalcularTotal() {
    var total = 0;

    //RECORRER LA TABLA PARA SUMAR TODOS LOS TOTALES DE PRODUCTOS
    $("#table_body tr").each(function () {
        var str = $(this).find("td").eq(3).html();
        var res = str.split("C$ ");
        total += parseFloat(res[1]);
    });

    return total;
}//FIN FUNCTION

//FUNCION PARA ALMACENAR EL INVENTARIO
function saveInventario() {
    var data = false, codigo = $("#codigo").val(), fecha = $("#myDatepicker2").val(), entrada = $("#entrada").find("option:selected").val(),
        proveedor = $("#proveedor").find("option:selected").val(), area = $("#area").find("option:selected").val();

    if (codigo !== "" && fecha !== "" && entrada !== "" && proveedor !== "" && area !== "") {

        var detalleEntrada = new Array();

        $("#table_body tr").each(function () {

            var row = $(this);
            var item = {};

            if (row.length > 0) {
                data = true;
            } else {
                data = false;
            }

            item["Id"] = 0;
            item["ProductoId"] = row.find("td").eq(0).attr("value");
            var precio = row.find("td").eq(1).html();
            var getPrice = precio.split("C$ ");
            item["PrecioEntrada"] = getPrice[1];
            item["CantidadEntrada"] = row.find("td").eq(2).html();
            item["Entrada"] = null;
            item["Producto"] = null;

            detalleEntrada.push(item);
        });

        //alert(JSON.stringify(detalleEntrada));

        if (data) {

            var datos = "Codigo=" + codigo + "&FechaEntrada=" + fecha + "&TipoEntradaId=" + entrada + "&BodegaId=" + area + "&ProveedorId=" + proveedor + "&detalleEntrada=" + JSON.stringify(detalleEntrada);

            $.ajax({
                type: "POST",
                url: "/Entradas/AddEntrada",
                data: datos,
                dataType: "JSON",                
                success: function (data) {
                    if (data.success) {
                        Alert("Almacenado correctamente", "", "success");
                        limpiarPantalla();
                    } else {
                        Alert("Error", "Hubieron errores al guardar", "error");
                    }
                },
                error: function (data) {
                    Alert("Error", "Revise", "error");
                }
            });
        } else {
            Alert("Error", "No se encontraron registros de productos", "error");
        }
    } else {
        Alert("Error", "Campos vacios. Revise e intente de nuevo", "error");
    }//FIN VALIDACION
}//FIN FUNCTION

//FUNCION PARA LIMPIAR LA PANTALLA
function limpiarPantalla() {
    //LIMPIAR INPUTS
    $("#codigo").val(cargarCodigo());
    $("#total").html("C$ 0.00");
    //LIMPIAR TABLA
    $("#table_body tr").remove();
    //LIMPIAR LOS SELECT2
    $("#entrada").select2("val", "-1");
    $("#area").select2("val", "-1");
    $("#producto").select2("val", "-1");
    $("#proveedor").select2("val", "-1");
}