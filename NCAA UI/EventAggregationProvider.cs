using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCAA_UI
{
    public static class EventAggregationProvider
    {
        public static EventAggregator TrackerEventAggregator { get; set; } = new EventAggregator();
    }
}
