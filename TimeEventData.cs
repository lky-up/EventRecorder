using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timeline
{
    public class TimeEventData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TimeEventId { get; set; } // 外键，指向 TimeEvent
        public DateTime TimeCreated { get; set; }

        public string? ExtraData { get; set; }
    }
}
