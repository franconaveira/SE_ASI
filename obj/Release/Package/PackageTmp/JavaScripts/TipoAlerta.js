$(document).ready(function () {
    // Activate tooltip
    $('[data-toggle="tooltip"]').tooltip();
    var e = document.getElementById("navBarButtonTA");
    e.classList.add("active");
    if (window.location.href.indexOf("#") > -1) {
        window.location.href = "https://localhost:44375/TipoAlerta";
    }
});

//EDIT - Funcion que llena el modal con la informacion de la fila seleccionada
$(function () {
    $(".edit").click(function () {
        var a = $(this).parents("tr").find(".cellTAId").text();
        $("#editTAId").val(a);
        var b = $(this).parents("tr").find(".cellTAName").text();
        $("#editTAName").val(b);
        var c = $(this).parents("tr").find(".cellTADesc").text();
        $("#editTADesc").val(c);
        $("#selectCurrentGD").empty();
        $("#selectAvailableGD").empty();

        $.ajax({
            type: "POST",
            url: "/TipoAlerta/getGruposDistribucion",
            data: '{idTipoAlerta: "' + a + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (d) {
                var e = document.getElementById("selectCurrentGD");
                var arr = d;
                for (var i = 0; i < arr.length; i++) {
                    var option = document.createElement("option");
                    option.text = arr[i].nombre;
                    option.value = arr[i].id;
                    e.appendChild(option);
                }
            }
        });

        $.ajax({
            type: "POST",
            url: "/TipoAlerta/getGruposDistribucionDisponibles",
            data: '{idTipoAlerta: "' + a + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (d) {
                var e = document.getElementById("selectAvailableGD");

                var arr = d;
                for (var i = 0; i < arr.length; i++) {
                    let newOption = new Option(arr[i].nombre, arr[i].id);
                    e.add(newOption, undefined);
                }
            }
        });
    });
});

$(function () {
    $("#AddMailButton").click(function () {
        var selectDisponibles = document.getElementById("selectAvailableGD");
        var selectActuales = document.getElementById('selectCurrentGD');
        var idGD = selectDisponibles.value;
        var index = selectDisponibles.selectedIndex;
        if (idGD.length != 0) {

            $.ajax({
                type: "POST",
                url: "/TipoAlerta/getGDById",
                data: '{idGrupoDistribucion: "' + idGD + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (d) {
                    var arr = d;
                    let newOption = new Option(arr[0].nombre, arr[0].id);
                    selectActuales.add(newOption, undefined);
                    selectDisponibles.remove(index);
                }
            });
        }//end if idGD.length
    });
});


$(function () {
    $("#RemoveMailButton").click(function () {
        var selectDisponibles = document.getElementById('selectAvailableGD');
        var selectActuales = document.getElementById('selectCurrentGD');
        var idGD = selectActuales.value;
        var index = selectActuales.selectedIndex;
        if (idGD.length != 0) {

            $.ajax({
                type: "POST",
                url: "/TipoAlerta/getGDById",
                data: '{idGrupoDistribucion: "' + idGD + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (d) {
                    var arr = d;
                    let newOption = new Option(arr[0].nombre, arr[0].id);
                    selectDisponibles.add(newOption, undefined);
                    selectActuales.remove(index);
                }
            });
        }//end if idGD.length
    });
});


$(function () {
    $("#updateGD").click(function () {
        var editTAId = $("#editTAId").val();

        var options = document.getElementById('selectCurrentGD').options;
        const arrayOpciones = [];
        //[IDTipoAlerta , idGD1 , IdGD2 , ..... ]
        let j = 0;
        arrayOpciones[j] = editTAId;

        for (let i = 0; i < options.length; i++) {
            j++;
            arrayOpciones[j] = options[i].value;
        }

        $.ajax({
            url: "/TipoAlerta/updateGruposTA",
            data: JSON.stringify(arrayOpciones),
            type: 'POST',
            contentType: "application/json; charset=utf-8"
        });
        $('#editTAModal').modal('hide');

    });

});