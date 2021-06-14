using Microsoft.Extensions.Configuration;
using SundownBoulevard.Booking.API.Models;
using SundownBoulevard.Booking.API.Services;
using SundownBoulevard.Booking.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SundownBoulevard.Booking.API.Repositories
{
    public class TableScheduleRepository
    {
        private readonly TimeSpan _allocatedTime;
        private readonly RestaurantContext _restaurantContext;
        private readonly IsValidReservationService _isValidReservationService;

        public TableScheduleRepository(RestaurantContext restaurantContext, IsValidReservationService isValidReservationService, IConfiguration configuration)
        {
            _allocatedTime = new TimeSpan(Convert.ToInt32(configuration["BookingConfiguration:AllocatedHoursForBooking"]), 0, 0);
            _restaurantContext = restaurantContext;
            _isValidReservationService = isValidReservationService;
        }

        /// <summary>
        /// Gets a list of repesentations of tables and when they've been reserved.
        /// </summary>
        /// <param name="reservationID"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<TableSchedule> GetTableSchedules(Guid reservationID, int year, int month, int day)
        {
            var reservations = _restaurantContext.Reservations.ToList().Where(r => !r.UID.Equals(reservationID) && DatesMatch(r, year, month, day) && _isValidReservationService.IsValid(r)).ToList();
            var bookedReservations = _restaurantContext.Bookings.ToList().Select(b => b.Reservation).ToList().Where(r => DatesMatch(r, year, month, day));
            // See which tables have been reserved or booked
            var tableReservations = _restaurantContext.TableReservations.ToList().Where(tr => IsReservedOrBooked(tr, reservations, bookedReservations));

            return GetTableSchedules(reservations, tableReservations);
        }

        /// <summary>
        /// Gets a list of representations of tables and their reservations, that are available outside of a specified time interval.
        /// </summary>
        /// <param name="tableSchedules"></param>
        /// <param name="timeSlotStartTime"></param>
        /// <returns></returns>
        public IEnumerable<TableSchedule> GetTableSchedulesWithAvailabilityOutsideCurrentInterval(List<TableSchedule> tableSchedules, TimeSpan timeSlotStartTime)
        {
            return tableSchedules.Where(ts => ts.ReservationStartTimes.All(rst => ReservationStartsAfterOrAtCurrentInterval(rst, timeSlotStartTime) || ReservationEndsBeforeOrAtCurrentInterval(rst, timeSlotStartTime)));
        }

        private static bool IsReservedOrBooked(TableReservation tr, IEnumerable<Reservation> reservations, IEnumerable<Reservation> bookedReservations)
        {
            return reservations.Any(rs => rs.ID == tr.ReservationID) || bookedReservations.Any(bs => bs.ID == tr.ReservationID);
        }

        private static bool DatesMatch(Reservation r, int year, int month, int day)
        {
            return r.Date.Year == year && r.Date.Month == month && r.Date.Day == day;
        }

        private List<TableSchedule> GetTableSchedules(IEnumerable<Reservation> reservations, IEnumerable<TableReservation> tableReservations)
        {
            var tables = _restaurantContext.Tables;
            var tableSchedules = new List<TableSchedule>();
            foreach (var table in tables)
            {
                var specificReservations = reservations.Where(r => tableReservations.Any(tr => tr.TableID == table.ID && tr.ReservationID == r.ID));
                tableSchedules.Add(new TableSchedule
                {
                    Table = table,
                    ReservationStartTimes = specificReservations.Select(sr => sr.Date.TimeOfDay)
                });
            }

            return tableSchedules;
        }
      
        private bool ReservationEndsBeforeOrAtCurrentInterval(TimeSpan reservedTime, TimeSpan timeSlotStart)
        {
            return AddAllocatedTime(reservedTime) <= timeSlotStart;
        }

        private bool ReservationStartsAfterOrAtCurrentInterval(TimeSpan reservedTime, TimeSpan timeSlotStart)
        {
            return reservedTime >= AddAllocatedTime(timeSlotStart);
        }

        private TimeSpan AddAllocatedTime(TimeSpan timeSlotStart)
        {
            return timeSlotStart.Add(_allocatedTime);
        }
    }
}
