using MyEPA.Extensions;
using MyEPA.Models.BaseModels;
using NLog;

namespace MyEPA.Services
{
    public class NLogService
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();
    }
    public static class ExtNLog
    {
        public static void DebugT<T>(this Logger logger, T model)
        {
            BaseLoggerModel baseLogger = new BaseLoggerModel(model);
            logger.Debug(baseLogger.ToJson());
        }
    }
}