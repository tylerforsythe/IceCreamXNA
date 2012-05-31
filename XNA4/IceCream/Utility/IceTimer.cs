using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IceCream.Utility
{
    /// <summary>
    /// To use, instantiate new IceTimer(). Then call Stopwatch() with ms intervals. It will auto-reset, so
    /// just call/monitor return value of Stopwatch().
    /// </summary>
    public class IceTimer
    {
        public IceTimer() {
            timer_start = TimeGetTime();
            Reset();
        }

        private long TimeGetTime() {
            return DateTime.Now.Ticks / 10000; //convert ticks to milliseconds. 10,000 ticks in 1 millisecond.
        }

        public long GetTimer() {
            return (long)(TimeGetTime());
        }

        public long GetStartTimeMillis() {
            return (long)(TimeGetTime() - timer_start);
        }


        public void Sleep(int ms) {
            long start = GetTimer();
            while (start + ms > GetTimer()) { }
        }

        public void Reset() {
            stopwatch_start = GetTimer();
        }

        public bool Stopwatch(int ms) {
            if (TimeGetTime() > stopwatch_start + ms) {
                Reset();
                return true;
            }
            else
                return false;
        }

        long timer_start;
        long stopwatch_start;
    }
}
