using Serilog;

namespace FarasaEtl.Infrastructure.SystemLogs
{
    public static class Logging
    {
        private const string _messageTemplate = "{message}";
        #region Verbose
        public static void Verbose<T>(T propertyValue, string messageTemplate = _messageTemplate)
        {
            Log.Verbose(messageTemplate, propertyValue);
        }
        public static void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Verbose(messageTemplate, propertyValue0, propertyValue1);
        }
        public static void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }
        public static void Verbose(string messageTemplate, params object[] propertyValues)
        {
            Log.Verbose(messageTemplate, propertyValues);
        }
        public static void Verbose(Exception exception, string messageTemplate)
        {
            Log.Verbose(exception, messageTemplate);
        }
        public static void Verbose<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            Log.Verbose(exception, messageTemplate, propertyValue);
        }
        public static void Verbose<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
        }
        public static void Verbose<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }
        public static void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log.Verbose(exception, messageTemplate, propertyValues);
        }
        #endregion

        #region Debug
        public static void Debug<T>(T propertyValue, string messageTemplate = _messageTemplate)
        {
            Log.Debug(messageTemplate, propertyValue);
        }
        public static void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Debug(messageTemplate, propertyValue0, propertyValue1);
        }
        public static void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }
        public static void Debug(string messageTemplate, params object[] propertyValues)
        {
            Log.Debug(messageTemplate, propertyValues);
        }
        public static void Debug(Exception exception, string messageTemplate)
        {
            Log.Debug(exception, messageTemplate);
        }
        public static void Debug<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            Log.Debug(exception, messageTemplate, propertyValue);
        }
        public static void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Debug(exception, messageTemplate, propertyValue0, propertyValue1);
        }
        public static void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }
        public static void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log.Debug(exception, messageTemplate, propertyValues);
        }
        #endregion

        #region Info
        public static void Information<T>(T propertyValue, string messageTemplate = _messageTemplate)
        {
            Log.Information(messageTemplate, propertyValue);
        }
        public static void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Information(messageTemplate, propertyValue0, propertyValue1);
        }
        public static void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }
        public static void Information(string messageTemplate, params object[] propertyValues)
        {
            Log.Information(messageTemplate, propertyValues);
        }
        public static void Information(Exception exception, string messageTemplate)
        {
            Log.Information(exception, messageTemplate);
        }
        public static void Information<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            Log.Information(exception, messageTemplate, propertyValue);
        }
        public static void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Information(exception, messageTemplate, propertyValue0, propertyValue1);
        }
        public static void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }
        public static void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log.Information(exception, messageTemplate, propertyValues);
        }
        #endregion

        #region Warn
        public static void Warning<T>(T propertyValue, string messageTemplate = _messageTemplate)
        {
            Log.Warning(messageTemplate, propertyValue);
        }
        public static void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Warning(messageTemplate, propertyValue0, propertyValue1);
        }
        public static void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }
        public static void Warning(string messageTemplate, params object[] propertyValues)
        {
            Log.Warning(messageTemplate, propertyValues);
        }
        public static void Warning(Exception exception, string messageTemplate)
        {
            Log.Warning(exception, messageTemplate);
        }
        public static void Warning<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            Log.Warning(exception, messageTemplate, propertyValue);
        }
        public static void Warning<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Warning(exception, messageTemplate, propertyValue0, propertyValue1);
        }
        public static void Warning<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }
        public static void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log.Warning(exception, messageTemplate, propertyValues);
        }
        #endregion

        #region Error
        public static void Error<T>(T propertyValue, string messageTemplate = _messageTemplate)
        {
            Log.Error(messageTemplate, propertyValue);
        }
        public static void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Error(messageTemplate, propertyValue0, propertyValue1);
        }
        public static void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }
        public static void Error(string messageTemplate, params object[] propertyValues)
        {
            Log.Error(messageTemplate, propertyValues);
        }
        public static void Error(Exception exception, string messageTemplate)
        {
            Log.Error(exception, messageTemplate);
        }
        public static void Error<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            Log.Error(exception, messageTemplate, propertyValue);
        }
        public static void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Error(exception, messageTemplate, propertyValue0, propertyValue1);
        }
        public static void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }
        public static void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log.Error(exception, messageTemplate, propertyValues);
        }
        #endregion

        #region Fatal
        public static void Fatal<T>(T propertyValue, string messageTemplate = _messageTemplate)
        {
            Log.Fatal(messageTemplate, propertyValue);
        }
        public static void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Fatal(messageTemplate, propertyValue0, propertyValue1);
        }
        public static void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }
        public static void Fatal(string messageTemplate, params object[] propertyValues)
        {
            Log.Fatal(messageTemplate, propertyValues);
        }
        public static void Fatal(Exception exception, string messageTemplate)
        {
            Log.Fatal(exception, messageTemplate);
        }
        public static void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            Log.Fatal(exception, messageTemplate, propertyValue);
        }
        public static void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
        }
        public static void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }
        public static void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log.Fatal(exception, messageTemplate, propertyValues);
        }
        #endregion

    }
}
