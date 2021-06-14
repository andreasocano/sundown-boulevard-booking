function updateDays(year, month) {
    var picker = document.getElementById("date-picker");
    picker.setAttribute('data-month', month);
    picker.setAttribute('data-day', 1);
    $(document).ready(function () {
        var jsonData = JSON.stringify({
            year: year,
            month: month
        });
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: '/date/days',
            data: jsonData,
            success: function (response) {
                var daySelector = $('#date-days');
                daySelector.empty();
                $(response.Items).each(function (index, item) {
                    $(daySelector).append(`<option value="${item.Value}" ${(item.IsSelected ? 'selected' : '')} ${(item.IsDisabled ? 'disabled' : '')}>${item.Text}</option>`);
                });
            }
        });
    })
}
function setDay(day) {
    $(document).ready(function () {
        var picker = $("#date-picker");
        picker.attr('data-day', day);
        getTimeSlots();
    });
}

function getTimeSlots() {
    document.getElementById('error-message').classList.add('hide');
    var picker = document.getElementById("date-picker");
    var year = picker.getAttribute('data-year');
    var month = picker.getAttribute('data-month');
    var day = picker.getAttribute('data-day');
    $(document).ready(function () {
        var jsonData = JSON.stringify({
            year: year,
            month: month,
            day: day
        });
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: '/date/timeslots',
            data: jsonData,
            success: function (response) {
                $('#time-slots').children('button').each(function () {
                    if (response.indexOf(Number(this.id)) > -1) {
                        this.classList.remove('hide');
                    } else {
                        this.classList.add('hide');
                    }
                });
            }
        });
    })
}

function saveDate(timeSlotStart) {
    $(document).ready(function () {
        var picker = $('#date-picker');
        var year = Number(picker.attr('data-year'));
        var month = Number(picker.attr('data-month'));
        var day = Number(picker.attr('data-day'));
        var reservationID = picker.attr('data-reservation-id');
        var jsonData = JSON.stringify({
            uid: reservationID,
            year: year,
            month: month,
            day: day,
            timeSlotStart: timeSlotStart
        });
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: '/date',
            data: jsonData,
            success: function (response) {
                window.location.href = "/menu";
            },
            error: function (response) {
                $('#error-message').removeClass('hide');
            }
        });
    });
}