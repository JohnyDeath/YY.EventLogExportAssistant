using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using YY.EventLogExportAssistant.Database;
using YY.EventLogExportAssistant.Tests.Helpers;
using YY.EventLogExportAssistant.Tests.Helpers.Models;
using YY.EventLogReaderAssistant;

namespace YY.EventLogExportAssistant.MySQL.Tests
{
    [CollectionDefinition("YY.EventLogExportAssistant.MySQL", DisableParallelization = true)]
    public class EventLogExportMasterTests
    {
        #region Private Member Variables

        private readonly CommonTestSettings _settings;
        DbContextOptionsBuilder<EventLogContext> _optionsBuilder;

        #endregion

        #region Constructors

        public EventLogExportMasterTests()
        {
            string configFilePath = GetConfigFile();

            _settings = new CommonTestSettings(
                configFilePath,
                new EventLogMySQLActions());

            _optionsBuilder = new DbContextOptionsBuilder<EventLogContext>();
            _optionsBuilder.UseMySql(_settings.ConnectionString, ServerVersion.AutoDetect(_settings.ConnectionString));

            using (EventLogContext context = EventLogContext.Create(_optionsBuilder.Options, _settings.DBMSActions))
                context.Database.EnsureDeleted();
        }

        #endregion

        #region Public Methods

        [Fact]
        public void ExportFormatLGFToMySQLTest()
        {
            ExportToPostgreSQL(_settings.SettingsLGF);
        }
        [Fact]
        public void ExportFormatLGDToMySQLTest()
        {
            ExportToPostgreSQL(_settings.SettingsLGD);
        }

        #endregion

        #region Private Methods

        private void ExportToPostgreSQL(EventLogExportSettings eventLogSettings)
        {
            EventLogOnMySQL target = new EventLogOnMySQL(_optionsBuilder.Options, eventLogSettings.Portion);
            target.SetInformationSystem(new InformationSystemsBase()
            {
                Name = eventLogSettings.InforamtionSystemName,
                Description = eventLogSettings.InforamtionSystemDescription
            });

            ExportHelper.ExportToTargetStorage(eventLogSettings, target);

            long rowsInDB;
            using (EventLogContext context = EventLogContext.Create(_optionsBuilder.Options, _settings.DBMSActions))
            {
                var informationSystem = context.InformationSystems
                    .First(i => i.Name == eventLogSettings.InforamtionSystemName);
                var getCount = context.RowsData
                    .Where(r => r.InformationSystemId == informationSystem.Id)
                    .LongCountAsync();
                getCount.Wait();
                rowsInDB = getCount.Result;
            }

            long rowsInSourceFiles;
            using (EventLogReader reader = EventLogReader.CreateReader(eventLogSettings.EventLogPath))
                rowsInSourceFiles = reader.Count();

            Assert.NotEqual(0, rowsInSourceFiles);
            Assert.NotEqual(0, rowsInDB);
            Assert.Equal(rowsInSourceFiles, rowsInDB);
        }
        private string GetConfigFile()
        {
            // TODO
            // ��������� ������������ ����������������� ����� � ������� CI

            string configFilePath = "appsettings.json";
            if (!File.Exists(configFilePath))
            {
                configFilePath = "githubactions-appsettings.json";
                IConfiguration Configuration = new ConfigurationBuilder()
                    .AddJsonFile(configFilePath, optional: true, reloadOnChange: true)
                    .Build();
                string connectionString = Configuration.GetConnectionString("EventLogDatabase");
                try
                {
                    _optionsBuilder = new DbContextOptionsBuilder<EventLogContext>();
                    _optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    using (EventLogContext context = EventLogContext.Create(_optionsBuilder.Options, new EventLogMySQLActions()))
                        context.Database.EnsureDeleted();
                }
                catch
                {
                    configFilePath = "appveyor-appsettings.json";
                }
            }

            if (!File.Exists(configFilePath))
                throw new Exception("���� ������������ �� ���������.");

            return configFilePath;
        }

        #endregion
    }
}
