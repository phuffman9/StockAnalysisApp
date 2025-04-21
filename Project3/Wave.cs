using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public class Wave
    {
        public decimal startPrice {  get; set; }
        public decimal endPrice { get; set; }

        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public bool up {  get; set; }
        public bool down { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }  
        public string displayDate { get; set; }
        public Wave()
        {
            startDate = DateTime.MinValue;
            endDate = DateTime.MinValue;
            startIndex = 0;
            endIndex = 0;
            up = false;
            down = false;
            displayDate = string.Empty;
           
        }
    }
}
