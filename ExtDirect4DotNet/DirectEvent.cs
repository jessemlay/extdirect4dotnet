using System;
using System.Linq;
using Newtonsoft.Json;

namespace ExtDirect4DotNet {
    [JsonObject]
    public class DirectEvent {
        public object data;

        public string name;

        /// <summary>
        /// Creates a new Instance of Direct Event
        /// </summary>
        /// <param name="eventname">The Name of the Event</param>
        /// <param name="data">Data to propagate with this event</param>
        public DirectEvent(string eventname, object dataToPropagate) {
            name = eventname;
            data = dataToPropagate;
        }
    }
}