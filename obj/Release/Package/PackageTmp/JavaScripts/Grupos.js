
$(document).ready(function () {
    // Activate tooltip
    //     $('[data-toggle="tooltip"]').tooltip();
    var e = document.getElementById("navBarButtonDG");
    e.classList.add("active");
          if (window.location.href.indexOf("#") > -1) {
              window.location.href = "https://localhost:44375/GruposMail";
          }
    var flagNombreCrear = false;
    var flagDescCrear = false;
    

//activa/desactiva el boton de guardar los cambios en el modal de CREAR NUEVO GRUPO DISTRIBUCION
    function validarDatosNuevoGrupo() 
    {
        if (flagNombreCrear == true && flagDescCrear == true) {
            document.getElementById("createGDConfirm").disabled = false;
        } else {
            document.getElementById("createGDConfirm").disabled = true;
        }
    }

    //activa/desactiva el boton de guardar los cambios en el modal de EDITAR GRUPO DISTRIBUCION
    function validarDatosEditarGrupo() 
    {
        if (flagNombreEditar == true && flagDescEditar == true) {
           
            document.getElementById("updateGD").disabled = false;
        } else {
            document.getElementById("updateGD").disabled = true;
        }
    }

    // campo de NOMBRE para NUEVO GRUPO DE DISTRIBCION     
    $("#addGDName").change(function () {

        var newNombre = $("#addGDName").val();
        if (newNombre.length > 5) // si es texto y mayor a 1
        {
            $("#addGDName").css('border-color', 'green');
            flagNombreCrear = true;
        }
        else {
            $("#addGDName").css('border-color', 'red');
            flagNombreCrear = false;
        }
        validarDatosNuevoGrupo();
    });

    // campo de DESCRIPCION para NUEVO GRUPO DE DISTRIBCION
    $("#addGDDesc").change(function () {
   
        var newDesc = $("#addGDDesc").val();
        if (newDesc.length > 5) // si es texto y mayor a 1
        {
            $("#addGDDesc").css('border-color', 'green');
            flagDescCrear = true;
        }
        else {
            $("#addGDDesc").css('border-color', 'red');
            flagDescCrear = false;
        }
        validarDatosNuevoGrupo();
    });
     

    // campo de NOMBRE para EDITAR GRUPO DE DISTRIBCION  
    $("#editGDName").change(function () {

        var editNombre = $("#editGDName").val();
        if (editNombre.length > 5) // si es texto y mayor a 1
        {
            $("#editGDName").css('border-color', 'green');
            flagNombreEditar = true;
        }
        else {
            $("#editGDName").css('border-color', 'red');
            flagNombreEditar = false;
        }
        validarDatosEditarGrupo();
    });

    // campo de NOMBRE para EDITAR GRUPO DE DISTRIBCION
    $("#editGDDesc").change(function () {

        var editDesc = $("#editGDDesc").val();
        if (editDesc.length > 5) // si es texto y mayor a 1
        {
            $("#editGDDesc").css('border-color', 'green');
            flagDescEditar = true;
        }
        else {
            $("#editGDDesc").css('border-color', 'red');
            flagDescEditar = false;
        }
        validarDatosEditarGrupo();
    });
    //Delete - Funcion que llena el modal con la informacion de la fila seleccionada
    $(function () {
        $(".delete").click(function () {
            var a = "";
            var b = "";
            var a = $(this).parents("tr").find(".cellGDId").text();
            $("#deleteGDId").val(a);
            var b = $(this).parents("tr").find(".cellGDName").text();
            $("#deleteGDName").val(b);
        });
    });

    $(function () {
        $("#confirmDeletion").click(function () {
            var idGrupo = $("#deleteGDId").val();
            $.ajax({
                type: "POST",
                url: "/GruposMail/deleteGrupoDistribucion",
                data: '{grupoId: "' + idGrupo + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    // alert("ok");
                }
            });
            //alert(a);
        });
    });

    //EDIT - Funcion que llena el modal con la informacion de la fila seleccionada
    $(function () {
        $(".edit").click(function () {
            var a = $(this).parents("tr").find(".cellGDId").text();
            $("#editGDId").val(a);
            var b = $(this).parents("tr").find(".cellGDName").text();
            $("#editGDName").val(b);
            var c = $(this).parents("tr").find(".cellGDDesc").text();
            $("#editGDDesc").val(c);
            $("#selectCurrentMail").empty();
            $("#selectAvailableMail").empty();
            var flagNombreEditar = true;
            var flagDescEditar = true;

            $.ajax({
                type: "POST",
                url: "/GruposMail/getCorreos",
                data: '{idGrupo: "' + a + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (d) {
                    var e = document.getElementById("selectCurrentMail");

                    var arr = d;
                    for (var i = 0; i < arr.length; i++) {
                        let newOption = new Option(arr[i].correo, arr[i].id);
                        e.add(newOption, undefined);
                    }
                }
            });

            $.ajax({
                type: "POST",
                url: "/GruposMail/getCorreosDisponibles",
                data: '{idGrupo: "' + a + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (d) {
                    var e = document.getElementById("selectAvailableMail");
                    var arr = d;
                    for (var i = 0; i < arr.length; i++) {
                        let newOption = new Option(arr[i].correo, arr[i].id);
                        e.add(newOption, undefined);
                    }
                }
            });
        });
    });

    $(function () {
        $("#createGDConfirm").click(function () {
            var newGDName = $("#addGDName").val();
            var newGDDesc = $("#addGDDesc").val();
            $.ajax({
                type: "POST",
                url: "/GruposMail/addGrupoDistribucion",
                data: '{nombreGrupo: "' + newGDName + '" , descripcionGrupo: "' + newGDDesc + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    // alert("ok");
                }
            });
            //alert(a);
        });
    });

    $(function () {
        $("#updateGD").click(function () {
            var editGDId = $("#editGDId").val();
            var editGDDesc = $("#editGDDesc").val();
            var editGDlName = $("#editGDName").val();

            $.ajax({
                type: "POST",
                url: "/GruposMail/updateGrupoDistribucion",
                data: '{groupId: "' + editGDId + '" , groupName: "' + editGDlName + '", groupDesc: "' + editGDDesc + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    // alert("ok");
                }
            });

            var options = document.getElementById('selectCurrentMail').options;
            const arrayOpciones = [];
            //[IDGrupo , idMail1 , IdMail2 , ..... ]
            let j = 0;
            arrayOpciones[j] = editGDId;

            for (let i = 0; i < options.length; i++) {
                j++;
                arrayOpciones[j] = options[i].value;
            }

            $.ajax({
                url: "/GruposMail/updateMiembrosGD",
                data: JSON.stringify(arrayOpciones),
                type: 'POST',
                contentType: "application/json; charset=utf-8"
            });
            $('#editDGModal').modal('hide');

        });


    });

    $(function () {
        $("#AddMailButton").click(function () {
            var selectDisponibles = document.getElementById("selectAvailableMail");
            var selectActuales = document.getElementById('selectCurrentMail');
            var idMail = selectDisponibles.value;
            var index = selectDisponibles.selectedIndex;

            if (idMail.length != 0)
            {
            $.ajax({
                type: "POST",
                url: "/GruposMail/getCorreoById",
                data: '{idCorreo: "' + idMail + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (d) {
                    var arr = d;
                    let newOption = new Option(arr[0].correo, arr[0].id);
                    selectActuales.add(newOption, undefined);
                    selectDisponibles.remove(index);
                }
            });
            }// end if mail.length !=  0
        });
    });


    $(function () {
        $("#RemoveMailButton").click(function () {
            var selectDisponibles = document.getElementById('selectAvailableMail');
            var selectActuales = document.getElementById('selectCurrentMail');
            var idMail = selectActuales.value;
            var index = selectActuales.selectedIndex;
            if (idMail.length != 0) {
            $.ajax({
                type: "POST",
                url: "/GruposMail/getCorreoById",
                data: '{idCorreo: "' + idMail + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (d) {
                    var arr = d;
                    let newOption = new Option(arr[0].correo, arr[0].id);
                    selectDisponibles.add(newOption, undefined);
                    selectActuales.remove(index);
                }
            });
            }// end if mail.length !=  0
        });
    });

    // }
});


