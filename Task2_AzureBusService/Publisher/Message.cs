using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
        public class Message
        {
            [Key]
            public Guid Id { get; set; }

            public MessageType Category { get; set; }

            public string Payload { get; set; }

            public DateTime CreatedTime { get; set; }

            public Guid SenderId { get; set; }
        }

        public enum MessageType
        {
            ProcessVideo,
            ProcessImage,
            ProcessTimeSeriesData
        }
}