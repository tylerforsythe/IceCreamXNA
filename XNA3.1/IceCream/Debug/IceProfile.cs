using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace IceCream.Debug
{
    public class IceProfile
    {
        Stopwatch Timer;

        private bool PropertyIsProfiling;
        private double PropertyTotalTime;
        private double PropertyMaxTime;
        private int PropertyName;
        private double PropertyCalls;
        private double _latestTime;

        public double Calls
        {
            get { return PropertyCalls; }
        }

        public double AverageTime
        {
            get { return (PropertyCalls > 0 ? PropertyTotalTime / PropertyCalls : 0); }
        }

        public bool IsProfiling
        {
            get { return PropertyIsProfiling; }
        }

        public double MaxTime
        {
            get { return PropertyMaxTime; }
        }

        public double LatestTime
        {
            get { return _latestTime; }
        }

        public int Name
        {
            get { return PropertyName; }
        }

        public double TotalTime
        {
            get { return PropertyTotalTime; }
        }

        public IceProfile(int Name)
        {
            PropertyName = Name;

            PropertyIsProfiling = false;
            PropertyTotalTime = 0f;
            PropertyMaxTime = 0f;
            PropertyCalls = 0;

            Timer = new Stopwatch();
        }

        public void StartProfiling()
        {
            Timer.Start();
            PropertyIsProfiling = true;
        }

        public void StopProfiling()
        {
            Timer.Stop();
            double time = Timer.Elapsed.TotalSeconds;
            _latestTime = time;
            PropertyTotalTime += time;
            if (PropertyMaxTime < time)
            {
                PropertyMaxTime = time;
            }
            PropertyCalls++;
            PropertyIsProfiling = false;
            Timer.Reset();
        }
    }

}
