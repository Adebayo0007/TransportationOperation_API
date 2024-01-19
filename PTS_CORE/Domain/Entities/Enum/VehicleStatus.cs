using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities.Enum
{
    public enum VehicleStatus
    {
        Unknown = 0,
        Idle = 1,
        Working = 2,
        InWorkShop = 3,
        SpecialAssignment = 4,
        OnSale = 5,
        DisAbled = 6,
        SiezedByAuthority = 7,
        Accidented = 8,
        BrokeDown = 9,
        Patrol = 10,
        TerminalUse = 11,
        TripDenied = 12,
        PickUp = 13,
        Rescue = 14,
    }
}
