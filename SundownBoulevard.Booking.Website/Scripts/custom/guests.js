function saveNumberOfGuests(amount) {
    $(document).ready(function () {
        var reservationID = $('#guests').attr('data-reservation-id');
        var jsonData = JSON.stringify({
            uid: reservationID,
            amount: amount
        });
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: '/guests/seats',
            data: jsonData,
            success: function (response) {
                window.location.href = "/date";
            }
        });
    })
}