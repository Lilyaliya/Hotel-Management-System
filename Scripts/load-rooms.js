$(document).ready(function () {
    LoadRoomDetails();
});

function LoadRoomDetails() {

    $.ajax({
        async: true,
        data: 'GET',
        dataType: 'HTML',
        contentType: false,
        processData: false,
        url: '/Home/GetAllRooms',
        success: function (data) {
            $("#grid").html(data);
        },
        error: function () {
            alert('Возникли проблемы с вашим запросом.');
        }
    });
}