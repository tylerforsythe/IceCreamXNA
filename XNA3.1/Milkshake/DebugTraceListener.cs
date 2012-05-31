using System;
using System.Collections.Generic;

using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;

namespace Milkshake
{
    public class DebugTraceListener:TraceListener
    {
        bool fileOpen = false;
        string fileName;
        [DllImport( "kernel32.dll" )]
        static extern bool AttachConsole( int dwProcessId );
        private const int ATTACH_PARENT_PROCESS = -1;

        public string FileName
        {
            get
            {
                if (fileName == null)
                {
                    foreach (DictionaryEntry de in this.Attributes)
                        if (de.Key.ToString().ToLower() == "filename")
                            fileName = de.Value.ToString();
                }
                return fileName;
            }
            set { fileName = value; }
        }
        private bool _consoleLogging = false;
        public bool ConsoleLogging
        {
            get
            {
                 foreach (DictionaryEntry de in this.Attributes)
                        if (de.Key.ToString().ToLower() == "consolelogging")
                            _consoleLogging =bool.Parse( de.Value.ToString());
                
                return _consoleLogging;
            }

        }
        protected override string[] GetSupportedAttributes()
        {
            return new string[] { "FileName" ,"ConsoleLogging"};
        }
        static DebugTraceListener()
        {
             AttachConsole( ATTACH_PARENT_PROCESS );
        }
        
        public DebugTraceListener()
        {

        }
        public override void Write(object o)
        {
            base.Write(o);
        }
        public override void Write(object o, string category)
        {
            base.Write(o, category);
        }
        public override void Write(string message)
        {
            //Console.Write(PrefixString(message));
           // base.Write(PrefixString(message));
            CheckFileStatus();
            sw.Write(PrefixString(message));
            if(ConsoleLogging)
                Console.Write(PrefixString(message));
           
        }
        public override void Write(string message, string category)
        {
            base.Write(message, category);
        }
        public override void WriteLine(object o)
        {
            base.WriteLine(o);
        }
        public override void WriteLine(object o, string category)
        {
            base.WriteLine(o, "[" + category + "]");
        }
        public override void WriteLine(string message)
        {
            CheckFileStatus();
            sw.WriteLine(PrefixString(message));
            if (ConsoleLogging)
                Console.WriteLine(PrefixString(message));
        }
        StreamWriter sw;
        private void CheckFileStatus()
        {
            if (!fileOpen)
            {
                sw=new StreamWriter(File.Create(FileName));
                sw.AutoFlush = true;
                fileOpen = true;
            }
        }
        public override void WriteLine(string message, string category)
        {
            base.WriteLine(message, category);
        }
        public string PrefixString(string message)
        {
            return DateTime.Now.ToString() + " - " + message;
        }
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            base.TraceData(eventCache, source, eventType, id, data);
        }
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            base.TraceData(eventCache, source, eventType, id, data);
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            base.TraceEvent(eventCache, source, eventType, id);
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            base.TraceEvent(eventCache, source, eventType, id, format, args);
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            base.TraceEvent(eventCache, source, eventType, id, message);
        }
        public override void Flush()
        {
            base.Flush();
        }
        protected override void Dispose(bool disposing)
        {
            sw.Close();
            base.Dispose(disposing);
        }
    }
}
