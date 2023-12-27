﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class HireVehicle : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 7);
        public string DepartureAddress { get; set; }
        public string DestinationAddress { get; set; }
        public double Amount { get; set; }
        public string VehicleId { get; set; }
        public string DriverUserId { get; set; }
        public DateTime DeapartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string DepartureTerminalId { get; set; }
    }
}