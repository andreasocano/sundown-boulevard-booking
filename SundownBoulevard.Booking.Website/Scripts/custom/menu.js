function getDish(amount) {
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: '/menu/dish',
            success: function (response) {
                $('#dish-name').text(response.Name);
                $('#dish-category').text(response.Category);
                $('#dish-thumbnail').attr('src', response.ThumnailURL);
                $('#dish').attr('data-dish-object', response.Object);
            }
        });
    })
}

function saveMenu() {
    $(document).ready(function () {
        var menu = $('#menu');
        var reservationID = menu.attr('data-reservation-id');
        var dishObject = $('#dish').attr('data-dish-object');
        var drinkObject = '';
        $('#drinks').children('input').each(function () {
            if (this.checked) {
                drinkObject = $(this).attr('data-drink-object');
                return;
            };
        });
        var jsonData = JSON.stringify({
            uid: reservationID,
            dish: dishObject,
            drink: drinkObject,
        });
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: '/menu',
            data: jsonData,
            success: function (response) {
                window.location.href = "/login";
            },
            error: function (response) {
                $('#error-message').removeClass('hide');
            }
        });
    })
}