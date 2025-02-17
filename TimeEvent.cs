using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timeline
{
    public class TimeEvent
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string DefaultValue { get; set; }

        public bool IsPreference { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
