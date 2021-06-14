function confirmEmail() {
    var email = document.getElementById('email')?.value;
    console.log(email);
    $(document).ready(function () {
        var reservationID = $('#login').attr('data-reservation-id');
        var jsonData = JSON.stringify({
            uid: reservationID,
            email: email
        });
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: '/login/confirm',
            data: jsonData,
            success: function (response) {
                console.log(response);
                var email = $('#email').value
                console.log(email);
                window.location.href = "/booking";
            }
        });
    });
}