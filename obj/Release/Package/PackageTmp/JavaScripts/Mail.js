$(document).ready(function () {
    // Activate tooltip
    $('[data-toggle="tooltip"]').tooltip();
    var e = document.getElementById("navBarButtonMail");
    e.classList.add("active");
    var flagNameCreate = false;
    var flagMailCreate = false;

    const addMailModal = document.getElementById("addMailModal");


//EDIT - Funcion que llena el modal con la informacion de la fila seleccionada
$(function () {
    $(".edit").click(function () {
        var a = $(this).parents("tr").find(".cellId").text();
        $("#editId").val(a);
        var b = $(this).parents("tr").find(".cellName").text();
        $("#editName").val(b);
        var c = $(this).parents("tr").find(".cellMail").text();
        $("#editMail").val(c);
        var flagNameEdit = true;
        var flagMailEdit = true;
        validarDatosEditarMail();
    });
});

$(function () {
    $("#createMail").click(function () {
        var newMail = $("#addMail").val();
        var newName = $("#addName").val();

        $.ajax({
            type: "POST",
            url: "/Mail/addMail",
            data: '{mail: "' + newMail + '" , name: "' + newName + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                // alert("ok");
            }
        });
        //alert(a);
    });
});

// campo de NOMBRE para NUEVO CORREO
$("#addName").change(function () {
    var no = $("#addName").val();

    if (no.length > 5) // si es texto y mayor a 1
    {
        $("#addName").css('border-color', 'green');
        flagNameCreate = true;
    }
    else {
        $("#addName").css('border-color', 'red');
        flagNameCreate = false;
    }
    validarDatosCreateMail();
});


// campo de CORREO para NUEVO CORREO
    $("#addMail").change(function ()
    {
    var no = $("#addMail").val();
    if (no.length > 8 && no.match(/^[^\s@]+@[^\s@]+\.[^\s@]+$/)  ) 
    {
    $("#addMail").css('border-color', 'green');
    flagMailCreate = true;
    }
            else {
    $("#addMail").css('border-color', 'red');
    flagMailCreate = false;
    }
    validarDatosCreateMail();
});

    // campo de NOMBRE para EDITAR CORREO
    $("#editName").change(function () {
        var noe = $("#editName").val();

        if (noe.length > 5) // si es texto y mayor a 1
        {
            $("#editName").css('border-color', 'green');
            flagNameEdit = true;
        }
        else {
            $("#editName").css('border-color', 'red');
            flagNameEdit = false;
        }
        validarDatosEditarMail();
    });


    // campo de CORREO para EDITAR CORREO
    $("#editMail").change(function () {
        var mailE = $("#editMail").val();
        if (mailE.length > 8 && mailE.match(/^[^\s@]+@[^\s@]+\.[^\s@]+$/))
        {
            $("#editMail").css('border-color', 'green');
            flagMailEdit = true;
        }
        else {
            $("#editMail").css('border-color', 'red');
            flagMailEdit = false;
        }
        validarDatosEditarMail();
    });

    function validarDatosEditarMail() // activa o desactiva el boton de formulario editar mail
    {
        if (flagNameEdit == true && flagMailEdit == true) {
            document.getElementById("updateMail").disabled = false;
        } else {
            document.getElementById("updateMail").disabled = true;
        }
    }

function validarDatosCreateMail() // activa o desactiva el boton de formulario
{
    if (flagNameCreate == true && flagMailCreate == true)
    {
        document.getElementById("createMail").disabled = false;
    } else
    {
        document.getElementById("createMail").disabled = true;
    }
}


$(function () {
    $("#confirmDeletion").click(function () {
        var idMail = $("#deleteId").val();
        $.ajax({
            type: "POST",
            url: "/Mail/removeMail",
            data: '{idCorreo: "' + idMail + '" }',
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
    $("#updateMail").click(function () {
        var editMailID = $("#editId").val();
        var editMailAddress = $("#editMail").val();
        var editMailName = $("#editName").val();
        $.ajax({
            type: "POST",
            url: "/Mail/updateMail",
            data: '{idCorreo: "' + editMailID + '" , correo: "' + editMailAddress + '" , nombre: "' + editMailName + '" }',
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
    $(".delete").click(function () {
        var a = "";
        var b = "";
        var a = $(this).parents("tr").find(".cellId").text();
        $("#deleteId").val(a);
        var b = $(this).parents("tr").find(".cellMail").text();
        $("#deleteMail").val(b);
    });
});


});// ready