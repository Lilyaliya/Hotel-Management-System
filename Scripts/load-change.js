$(document).ready(function () {
    
    RegisterChange();
});

function RegisterChange() {
    $.ajax({
        async: true,
        type: 'GET',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf-8',
        //data: { ID: ID },
        url: '/Home/GetChange',
        success: function (data) {
            if (data.success === true) {
                var result = confirm(data.message);
                if (result === false) {
                    return false;
                }
                ApplyChange();
            }

        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }

    });



};

function ApplyChange() {
    $.ajax({
        async: true,
        type: 'GET',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf-8',
        //data: { ID: ID },
        url: '/Change/RegisterChange',
        success: function (data) {
            if (data.success === true) {
                alert(data.message);
            }

        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }

    });
}

//function LoadRoomDetails() {

//    $.ajax({
//        async: true,
//        data: 'GET',
//        dataType: 'HTML',
//        contentType: false,
//        processData: false,
//        url: '/Home/GetAllRooms',
//        success: function (data) {
//            $("#grid").html(data);
//        },
//        error: function () {
//            alert('Возникли проблемы с вашим запросом.');
//        }
//    });
//}