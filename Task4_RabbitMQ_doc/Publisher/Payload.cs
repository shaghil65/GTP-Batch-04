using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    public class Payload
    {
        public Guid id { get; set; }
        public string filename { get; set; }

    }
    public class VideoPayload : Payload
    {
        public string type { get; set; }

    }
    public class ImagePayload : Payload
    {
        public string codec { get; set; }
    }
    public class TimeData : Payload
    {
        public string timestamp { get; set; }
    }
}
