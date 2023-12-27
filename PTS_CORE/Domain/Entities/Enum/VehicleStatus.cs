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
        Working = 1,
        InWorkShop = 2,
        SpecialAssignment = 3,
        OnSale = 4,
        DisAbled = 5,
        SiezedByAuthority = 6,
        Accidented = 7,
        BrokeDown = 8,
        Patrol = 9,
        TerminalUse = 10,
        TripDenied = 11,
        PickUp = 12,
        Rescue = 13,
    }
}
