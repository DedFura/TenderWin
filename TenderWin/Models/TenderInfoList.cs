using Newtonsoft.Json;
using System.Collections.Generic;

namespace TenderWin.Models
{
    public class TenderInfoList
    {
        [JsonProperty("invdata")]
        public List<TenderInfo> TanderInfo { get; set; }
    }
}
