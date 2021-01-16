﻿using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading;
using YY.EventLogExportAssistant;
using YY.EventLogExportAssistant.ClickHouse;

namespace YY.EventLogExportToClickHouse
{
    class Program
    {
        #region Private Static Member Variables

        private static long _totalRows;
        private static long _lastPortionRows;
        private static DateTime _lastEventPeriod;
        private static DateTime _beginPortionExport;
        private static DateTime _endPortionExport;

        #endregion

        #region Static Methods

        static void Main()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            string connectionString = Configuration.GetConnectionString("EventLogDatabase");

            IConfigurationSection eventLogSection = Configuration.GetSection("EventLog");
            string eventLogPath = eventLogSection.GetValue("SourcePath", string.Empty);
            int watchPeriodSeconds = eventLogSection.GetValue("WatchPeriod", 60);
            int watchPeriodSecondsMs = watchPeriodSeconds * 1000;
            bool useWatchMode = eventLogSection.GetValue("UseWatchMode", false);
            int portion = eventLogSection.GetValue("Portion", 1000);

            IConfigurationSection informationSystemSection = Configuration.GetSection("InformationSystem");
            string informationSystemName = informationSystemSection.GetValue("Name", string.Empty);
            string informationSystemDescription = informationSystemSection.GetValue("Description", string.Empty);
            string timeZoneName = informationSystemSection.GetValue("TimeZone", string.Empty);

            if (string.IsNullOrEmpty(eventLogPath))
            {
                Console.WriteLine("Не указан каталог с файлами данных журнала регистрации.");
                Console.WriteLine("Для выхода нажмите любую клавишу...");
                Console.Read();
                return;
            }

            Console.WriteLine();
            Console.WriteLine();

            using (EventLogExportMaster exporter = new EventLogExportMaster())
            {
                exporter.SetEventLogPath(eventLogPath);

                EventLogOnClickHouse target = new EventLogOnClickHouse(
                    connectionString, 
                    portion, 
                    new ClickHouseExtendedActions());
                target.SetInformationSystem(new InformationSystemsBase()
                {
                    Name = informationSystemName,
                    Description = informationSystemDescription,
                    TimeZoneName = timeZoneName
                });
                exporter.SetTarget(target);

                exporter.BeforeExportData += BeforeExportData;
                exporter.AfterExportData += AfterExportData;
                exporter.OnErrorExportData += OnErrorExportData;

                _beginPortionExport = DateTime.Now;
                if (useWatchMode)
                {
                    while (true)
                    {
                        if (Console.KeyAvailable)
                            if (Console.ReadKey().KeyChar == 'q')
                                break;

                        while (exporter.NewDataAvailable())
                        {
                            exporter.SendData();
                        }
                        Thread.Sleep(watchPeriodSecondsMs);
                    }
                }
                else
                    while (exporter.NewDataAvailable())
                        exporter.SendData();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Для выхода нажмите любую клавишу...");
            Console.Read();
        }

        #endregion

        #region Events

        private static void BeforeExportData(BeforeExportDataEventArgs e)
        {
            _lastPortionRows = e.Rows.Count;
            _totalRows += e.Rows.Count;
            _lastEventPeriod = e.Rows.LastOrDefault()?.Period ?? DateTime.MinValue;

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("[{0}] Last read: {1}             ", DateTime.Now, e.Rows.Count);
        }
        private static void AfterExportData(AfterExportDataEventArgs e)
        {
            _endPortionExport = DateTime.Now;
            var duration = _endPortionExport - _beginPortionExport;

            Console.WriteLine("[{0}] Total read: {1}            ", DateTime.Now, _totalRows);
            Console.WriteLine("[{0}] {1} / {2} (sec.)           ", DateTime.Now, _lastPortionRows, duration.TotalSeconds);
            Console.WriteLine("[{0}] Last period: {1}            ", DateTime.Now, _lastEventPeriod);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Нажмите 'q' для завершения отслеживания изменений...");

            _beginPortionExport = DateTime.Now;
        }
        private static void OnErrorExportData(OnErrorExportDataEventArgs e)
        {
            Console.WriteLine(
                "Ошибка при экспорте данных." +
                "Критическая: {0}\n" +
                "\n" +
                "Содержимое события:\n" +
                "{1}" +
                "\n" +
                "Информация об ошибке:\n" +
                "\n" +
                "{2}",
                e.Critical, e.SourceData, e.Exception);
        }

        #endregion
    }
}
