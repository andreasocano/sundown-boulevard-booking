using SundownBoulevard.Booking.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SundownBoulevard.Booking.API.Repositories
{
    public class TableRepository
    {
        private readonly TableScheduleRepository _tableScheduleRepository;

        public TableRepository(TableScheduleRepository tableScheduleRepository)
        {
            _tableScheduleRepository = tableScheduleRepository;
        }

        /// <summary>
        /// Gets available tables for a date and time, based what other time intervals they've been reserved.
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public IEnumerable<Table> GetAvailable(Guid uid, DateTime date)
        {
            // Get tables that haven't been reserved for specified time.
            var tableSchedules = _tableScheduleRepository.GetTableSchedules(uid, date.Year, date.Month, date.Day);
            var timeSlotStartTime = date.TimeOfDay;
            var tablesWithAvailableSchedule = _tableScheduleRepository.GetTableSchedulesWithAvailabilityOutsideCurrentInterval(tableSchedules, timeSlotStartTime);
            return tablesWithAvailableSchedule.Select(ts => ts.Table);
        }
    }
}
