using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Timers
{
    public static class MasterTime
    {
        private static TimeSpan m_Offset;
        private static DateTime m_MasterClock;

        public static DateTime GetClock()
        {
            return m_MasterClock;
        }

        public static bool SetClock(DateTime setTime)
        {
            m_MasterClock = setTime;
            return true;
        }

        public static bool SetOffset(TimeSpan setOffset)
        {
            m_Offset = setOffset;
            return true;
        }

        public static bool SetOffset(String setOffset)
        {
            Parsetime pt = new Parsetime();
            m_Offset = pt.ParseTS(setOffset);
            return true;
        }

        public static bool SetOffset(TimeSpan currentOffset, String setOffset)
        {
            Parsetime pt = new Parsetime();
            m_Offset = currentOffset + pt.ParseTS(setOffset);
            return true;
        }

        public static TimeSpan GetOffset()
        {
            return m_Offset;
        }
    }
}
