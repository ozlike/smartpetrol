using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartpetrol.Configuration
{
    public static class GlobalValues
    {
        private static int? _reservationHours;
        public static int ReservationHours {
            get
            {
                return (int)_reservationHours;
            }
            set
            {
                if (_reservationHours != null) throw new Exception();
                _reservationHours = value;
            }
        }
    }
}
