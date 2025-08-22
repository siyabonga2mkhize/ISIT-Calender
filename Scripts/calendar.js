document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        events: '/Calendar/GetEvents',
        dateClick: onDateClick,
        eventClick: onEventClick
    });
    calendar.render();

    $('#createEventForm').on('submit', onFormSubmit);
});
