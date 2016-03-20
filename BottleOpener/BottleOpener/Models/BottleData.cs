using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottleOpener.Models
{
    public class BottleData
    {
        public DateTime Time { get; set; }
        public int AvgData { get; set; }
        public List<int> RawData { get; set; }

        public BottleData(List<int> _rawdata)
        {
            Time = DateTime.Now;
            RawData = new List<int>(_rawdata);
            AvgData = RawData.Sum() / RawData.Count;
        }
    }
}
