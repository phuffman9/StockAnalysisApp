using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public class PeakValley
    {
        public bool peak {get; set;}
        public bool valley { get; set; }
        public int index { get; set; }
        public int lMargin{ get; set; }
        public int rMargin { get; set; }
        public DateTime date { get; set; }

        public PeakValley()
        {
            peak = false;
            valley = false;
            index = -1;
            lMargin = -1;
            rMargin = -1;
        }

        public PeakValley(bool Peak, bool Valley, int Index, int LMargin, int RMargin)
        {
            peak = Peak; 
            valley = Valley;
            index = Index; 
            lMargin = LMargin; 
            rMargin = RMargin;    
        }
    }
}
//have a find peakvalley class that takes a list of candlesticks and the margin, which returns a list of PeakValley's
//find all peaks and valleys of margin 1, then if we change the margin, go through the list of peakvalley's and check if both 
//the lMargin and rMargin are > than the specified margin.
//have a dictionary for each size of margin where the key is an int representing the margin, and the value is a list<PeakValley>
