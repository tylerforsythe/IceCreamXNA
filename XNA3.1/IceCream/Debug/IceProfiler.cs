using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace IceCream.Debug
{
    public static class IceProfiler
    {
        public static bool ProfilingEnabled = true;

        static Dictionary<int, IceProfile> PropertyProfiles;

        static IceProfiler()
        {
            PropertyProfiles = new Dictionary<int, IceProfile>();
        }

        public static void SaveProfile()
        {
            #if(PROFILE && WINDOWS)
            TextWriter tw = new StreamWriter("IceProfile.log", false);

            tw.WriteLine();
            tw.WriteLine();
            tw.WriteLine();
            tw.WriteLine("New Profile :" + DateTime.Now.ToString());
            tw.WriteLine(string.Format("{0,-32}", "(Name),") +
                            string.Format("{0,-40}", "(Avg Time),") +
                            string.Format("{0,-20}", "(Calls),") +
                            string.Format("{0,-20}", "(Max Time),") +
                            string.Format("{0,-20}", "(Total Time),"));
            foreach (IceProfile p in IceProfiler.Profiles.Values)
            {
                tw.WriteLine(string.Format("{0,-32}", ((IceProfilerNames)p.Name).ToString() + ",") +
                            string.Format("{0,-40}", p.AverageTime + " (" + Math.Round((1.0f / p.AverageTime)) + " Hz)," ) +
                            string.Format("{0,-20}", p.Calls + ",") +
                            string.Format("{0,-20}", p.MaxTime + ",") +
                            string.Format("{0,-20}", p.TotalTime));
            }
            tw.Close();
            #endif
        }

        internal static IceProfile GetProfileByName(int id)
        {
            if (PropertyProfiles.ContainsKey(id))
                return PropertyProfiles[id];
            else
                return null;
        }

        public static void StartProfiling(IceProfilerNames id)
        {
            #if(PROFILE)
            if (ProfilingEnabled)
            {

                IceProfile Item = GetProfileByName((int)id);

                if (Item == null)
                {
                    Item = new IceProfile((int)id);
                    PropertyProfiles.Add((int)id, Item);
                }

                if (Item.IsProfiling)
                    throw new NotSupportedException(id + " is currently profiling, try stopping it first");

                Item.StartProfiling();
            }
            #endif
        }

        public static void StopProfiling(IceProfilerNames id)
        {
            #if(PROFILE)
            if (ProfilingEnabled)
            {
                IceProfile Item = GetProfileByName((int)id);

                if (Item == null)
                    throw new NotSupportedException("Profile doesn't exist");

                if (!Item.IsProfiling)
                    throw new NotSupportedException(id + " isn't currently profiling, try starting it first");

                Item.StopProfiling();
            }
            #endif
        }

        public static Dictionary<int, IceProfile> Profiles
        {
            get { return (PropertyProfiles); }
        }
    }    
}
