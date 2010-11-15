using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ExtDirect4DotNet
{
    [JsonObject]
    public class DirectEvent
    {
        public string name;
        public object data;

        /// <summary>
        /// Creates a new Instance of Direct Event
        /// </summary>
        /// <param name="eventname">The Name of the Event</param>
        /// <param name="data">Data to propagate with this event</param>
        public DirectEvent(string eventname, object dataToPropagate)
        {
            this.name = eventname;
            this.data = dataToPropagate;
        }
    }
}
