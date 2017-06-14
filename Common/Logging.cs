using log4net;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Common
{
    public class Logging
    {
        public static string GetExecutingMethodName(System.Reflection.MemberInfo MI)
        {
            try
            {
                return string.Concat(MI.ReflectedType.Name, ".", MI.Name, "()");
            }
            catch (Exception ex)
            {
                return string.Concat("GetExecutingMethodName : ", ex.Message);
            }

        }

        /// <summary>
        /// Logging method that check for IsDebugEnabled flag and prefixes the message with the calling method name
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="message">The message to log</param>
        /// <param name="parameters">The parameter to apply to the message format</param>
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void DebugFormat(ILog logger, string message, params object[] parameters)
        {
            if (logger.IsDebugEnabled)
            {
                var callingMethod = new StackFrame(1).GetMethod().Name + ". ";
                logger.DebugFormat(callingMethod + message, parameters);
            }
        }

        /// <summary>
        /// Logging method that check for IsInfoEnabled flag and prefixes the message with the calling method name
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="message">The message to log</param>
        /// <param name="parameters">The parameter to apply to the message format</param>
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void InfoFormat(ILog logger, string message, params object[] parameters)
        {
            if (logger.IsInfoEnabled)
            {
                var callingMethod = new StackFrame(1).GetMethod().Name + ". ";
                logger.InfoFormat(callingMethod + message, parameters);
            }
        }

        /// <summary>
        /// Logging method that check for IsErrorEnabled flag and prefixes the message with the calling method name
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="message">The message to log</param>
        /// <param name="parameters">The parameter to apply to the message format</param>
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void ErrorFormat(ILog logger, string message, params object[] parameters)
        {
            if (logger.IsErrorEnabled)
            {
                var callingMethod = string.Format("Error in {0}, ", new StackFrame(1).GetMethod().Name);
                logger.ErrorFormat(callingMethod + message, parameters);
            }
        }

        /// <summary>
        /// Logging method that check for IsFatalEnabled flag and prefixes the message with the calling method name
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="message">The message to log</param>
        /// <param name="parameters">The parameter to apply to the message format</param>
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void FatalFormat(ILog logger, string message, params object[] parameters)
        {
            if (logger.IsFatalEnabled)
            {
                var callingMethod = new StackFrame(1).GetMethod().Name + ". ";
                logger.FatalFormat(callingMethod + message, parameters);
            }
        }

        /// <summary>
        /// Logging method that check for IsWarnEnabled flag and prefixes the message with the calling method name
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="message">The message to log</param>
        /// <param name="parameters">The parameter to apply to the message format</param>
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void WarnFormat(ILog logger, string message, params object[] parameters)
        {
            if (logger.IsWarnEnabled)
            {
                var callingMethod = new StackFrame(1).GetMethod().Name + ". ";
                logger.WarnFormat(callingMethod + message, parameters);
            }
        }

        /// <summary>
        /// Logging method that check for IsDebugEnabled flag and prefixes the message with the calling method name.
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="message">The message to log</param>
        /// <param name="parameters">The parameter to apply to the message format</param>
        /// <returns>The tick count at the time of loggin</returns>
        public static int DebugFormatDurationStart(ILog logger, string message, params object[] parameters)
        {
            if (logger.IsDebugEnabled)
            {
                var msg = string.Format("{0}. {1}", new StackFrame(1).GetMethod().Name, message);
                logger.DebugFormat(msg, parameters);
            }
            return Environment.TickCount;
        }

        /// <summary>
        /// Logging method that check for IsDebugEnabled flag and prefixes the message with the calling method name and duration in milliseconds.
        /// </summary>
        /// <param name="tickCount">A tick count that was received from DebugFormatDurationStart</param>
        /// <param name="logger">The logger to use</param>
        /// <param name="message">The message to log</param>
        /// <param name="parameters">The parameter to apply to the message format</param>
        public static void DebugFormatDurationEnd(int tickCount, ILog logger, string message = null, params object[] parameters)
        {
            if (logger.IsDebugEnabled) {
                var callingMethod = new StackFrame(1).GetMethod().Name + ". ";
                var duration = string.Format("duration={0} ms. ", Environment.TickCount - tickCount);
                var msg = message != null ? duration + message : duration;
                logger.DebugFormat(callingMethod + msg, parameters);
            }
        }
    }
}