using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropBoxWCF.Classes
{
    public static class CLog
    {
        public static object locker = new object();
        public static string LogTime
        {
            get
            {
                DateTime d = new DateTime();
                string logTime = string.Empty;
                d = DateTime.Now;

                if (d.Hour < 10)
                    logTime += "0" + d.Hour.ToString();
                else
                    logTime += d.Hour.ToString();

                logTime += ":";

                if (d.Minute < 10)
                    logTime += "0" + d.Minute.ToString();
                else
                    logTime += d.Minute.ToString();

                logTime += ":";

                if (d.Second < 10)
                    logTime += "0" + d.Second.ToString();
                else
                    logTime += d.Second.ToString();

                return logTime;
            }
        }

        /// <summary>
        /// Функция ведёт информативный лог в консоли. Тип: Текст дебага \ инфо
        /// </summary>
        /// <param name="text">Текст, выводимый в консоли</param>
        /// <returns></returns>
        public static void Log(string text)
        {
            lock (locker)
            {
                Console.WriteLine("[" + LogTime + " INFO] " + text);
            }
        }
        /// <summary>
        /// Функция ведёт информативный лог в консоли. Тип: Предупреждение
        /// </summary>
        /// <param name="text">Текст, выводимый в консоли</param>
        /// <returns></returns>
        public static void Log(string text, int warning)
        {
            lock (locker)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[" + LogTime + " WARN] " + text);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        /// <summary>
        /// Функция ведёт информативный лог в консоли. Тип: Важно
        /// </summary>
        /// <param name="text">Текст, выводимый в консоли</param>
        /// <returns></returns>
        public static void Log(string text, double appworkinfo_positive)
        {
            lock (locker)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[" + LogTime + " WORK] " + text);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
