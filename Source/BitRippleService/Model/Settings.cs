using BitRippleService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BitRippleService.Model
{
    [DataContract]
    public class Settings : ISettingsService
    {
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public int Interval { get; set; }
    }
}
