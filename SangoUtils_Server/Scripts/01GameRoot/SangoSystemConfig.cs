﻿using SangoScripts_Server.Logger;

namespace SangoUtils_Server.Config
{
    public static class SangoSystemConfig
    {
        public static readonly LoggerConfig_Sango LoggerConfig_Sango = new()
        {
            EnableSangoLog = true,
            LogPrefix = "#",
            EnableTimestamp = true,
            LogSeparate = ">>",
            EnableThreadID = true,
            EnableTraceInfo = true,
            EnableSaveLog = true,
            EnableCoverLog = true,
            SaveLogPath = string.Format("{0}Logs\\", AppDomain.CurrentDomain.BaseDirectory),
            SaveLogName = "SangoLog.txt",
            LoggerType = LoggerType.OnConsole
        };

        public static readonly RegistRegularConfig RegistRegularConfig = new()
        {
            GuestIDPrefix = "_SangoGuest_",
            DefaultTransform = new(new(0, 0, 0), new(0, 0, 0, 0), new(1, 1, 1))
        };
    }
}
