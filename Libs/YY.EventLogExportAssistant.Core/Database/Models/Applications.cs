﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YY.EventLogExportAssistant.Database.Models
{
    public class Applications : ReferenceObject
    {
        #region Private Static Members

        private static readonly Dictionary<string, string> _mapPresentation = new Dictionary<string, string>()
        {
            { "1CV8", "Толстый клиент" },
            { "1CV8C", "Тонкий клиент" },
            { "WebClient", "Веб-клиент" },
            { "Designer", "Конфигуратор" },
            { "COMConnection", "Внешнее соединение (COM, обычное)" },
            { "WSConnection", "Сессия web-сервиса" },
            { "BackgroundJob", "Фоновое задание" },
            { "SystemBackgroundJob", "Системное фоновое задание" },
            { "SrvrConsole", "Консоль кластера" },
            { "COMConsole", "Внешнее соединение (COM, административное)" },
            { "JobScheduler", "Планировщик заданий" },
            { "Debugger", "Отладчик" },
            { "RAS", "Сервер администрирования" }
        };

        #endregion

        #region Public Static Methods

        public static string GetPresentationByName(string name)
        {
            return _mapPresentation.TryGetValue(name, out string output) ? output : name;
        }

        #endregion

        #region Public Members

        [MaxLength(500)]
        public string Presentation
        {
            get => GetPresentationByName(Name);
            // ReSharper disable once ValueParameterNotUsed
            set { }
        }

        #endregion
    }
}
