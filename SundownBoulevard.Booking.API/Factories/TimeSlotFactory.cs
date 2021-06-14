using SundownBoulevard.Booking.API.Models;
using SundownBoulevard.Booking.API.Repositories;
using SundownBoulevard.Booking.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SundownBoulevard.Booking.API.Factories
{
    public class TimeSlotFactory
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly TableScheduleRepository _tableScheduleRepository;
        private readonly BookingConfiguration _bookingConfiguration;

        public TimeSlotFactory(ReservationRepository reservationRepository, TableScheduleRepository tableScheduleRepository, BookingConfiguration bookingConfiguration)
        {
            _reservationRepository = reservationRepository;
            _tableScheduleRepository = tableScheduleRepository;
            _bookingConfiguration = bookingConfiguration;
        }

        /// <summary>
        /// Creates a list of available time slots.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<TimeSlot> CreateAvailable(GetTimeSlotsRequest request)
        {
            var reservationID = request.UID;
            var reservation = _reservationRepository.Get(reservationID);
            if (reservation == null) return Enumerable.Empty<TimeSlot>();
            int seats = reservation.Seats;
            int year = request.Year;
            int month = request.Month;
            int day = request.Day;
            // Get reservations and bookings for this date.
            List<TableSchedule> tableSchedules = _tableScheduleRepository.GetTableSchedules(reservationID, year, month, day);
            List<TimeSpan> allTimeSlotStartTimes = GetAllTimeSlotStartTimes();
            allTimeSlotStartTimes = FilterForCurrentDay(year, month, day, allTimeSlotStartTimes);
            List<TimeSpan> resultingTimeSlotStarts = GetAvailableTimeSlotStartTimes(seats, tableSchedules, allTimeSlotStartTimes);

            return resultingTimeSlotStarts.Select(Create);
        }

        /// <summary>
        /// Gets a list of all possible start times where a booking can be made.
        /// </summary>
        /// <returns></returns>
        public List<TimeSpan> GetAllTimeSlotStartTimes()
        {
            var allTimeSlotStarts = new List<TimeSpan>();
            for (int i = _bookingConfiguration.EarliestBookingHour; i < _bookingConfiguration.LatestBookingHour; i++)
            {
                for (int j = 0; j < 60; j += _bookingConfiguration.TimeIncrements)
                {
                    allTimeSlotStarts.Add(new TimeSpan(i, j, 0));
                }
            }
            allTimeSlotStarts.Add(new TimeSpan(_bookingConfiguration.LatestBookingHour, 0, 0));

            return allTimeSlotStarts;
        }

        private static List<TimeSpan> FilterForCurrentDay(int year, int month, int day, List<TimeSpan> allTimeSlotStartTimes)
        {
            var now = DateTime.Now;
            if (year == now.Year && month == now.Month && day == now.Day)
                allTimeSlotStartTimes = allTimeSlotStartTimes.Where(atsst => atsst > now.TimeOfDay).ToList();
            return allTimeSlotStartTimes.OrderBy(a => a).ToList();//.ThenBy(a => a.Minutes).ToList();
        }

        private List<TimeSpan> GetAvailableTimeSlotStartTimes(int seats, List<TableSchedule> tableSchedules, List<TimeSpan> allTimeSlotStartTimes)
        {
            var resultingTimeSlotStarts = new List<TimeSpan>();
            foreach (var timeSlotStartTime in allTimeSlotStartTimes)
            {
                // Check if there's enough seats for that time
                var compatibleTableSchedules = _tableScheduleRepository.GetTableSchedulesWithAvailabilityOutsideCurrentInterval(tableSchedules, timeSlotStartTime);
                var availableSeats = compatibleTableSchedules.Select(cts => cts.Table).Sum(at => at.Seats);
                if (availableSeats < seats) continue;
                resultingTimeSlotStarts.Add(timeSlotStartTime);
            }

            return resultingTimeSlotStarts;
        }

        private static TimeSlot Create(TimeSpan rtss)
        {
            return new TimeSlot { StartTime = rtss };
        }
    }
}
