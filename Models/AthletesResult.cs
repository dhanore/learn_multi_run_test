using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yoyo_web_app.Models
{
    public class AthletesResult
    {
        public int Id { get; set; }
        public string AthleteName { get; set; }
        public string SpeedLevel { get; set; }
        public string ShuttleNo { get; set; }
        public bool? IsWarned { get; set; }
        public string Secret { get; set; }

    }

    public class AthletesDto
    {
        public int Id { get; set; }
        public string AthleteName { get; set; }
        public int AccumulatedShuttleDistance { get; set; }
        public bool? IsWarned { get; set; }

    }
}
