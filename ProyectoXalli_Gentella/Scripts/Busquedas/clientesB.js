function CargarCliente() {
    $.ajax({
        type: "GET",
        url: "/Busquedas/busquedaClientes/",
        data: {
            Nombre: $("#nombre").val().trim(), Apellido: $("#apellido").val().trim()
        },
        success: function (data) {

            alert(JSON.stringify(Object.keys.length));

            if (data != null) {
                //ELIMINAR LA FILA POR DEFECTO
                $("#data").remove();
                var agregar = "";

                //LLENAR LA TABLA DE CLIENTES
                for (var i = 0; i < Object.keys.length; i++) {
                    var cliente = JSON.stringify(data[i]);

                    agregar += '<tr class="headings" id = "data">' +
                        '<td>' + cliente.Documento + '</td>' +
                        '<td>' + cliente.RUC != null ? data.RUC : " N/A " + '</td>' +
                        '<td>' + cliente.Nombres + '</td>' +
                        '<td>' + cliente.ClienteId + '</td>' +
                        '</tr>';

                    alert(cliente);
                }

                $("#clientBody").append(agregar);
            }
        }
    });
}
