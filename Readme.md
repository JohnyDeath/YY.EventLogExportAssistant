# Помощник экспорта журнала регистрации

| Nuget-пакет | Актуальная версия | Описание |
| ----------- | ----------------- | -------- |
| YY.EventLogExportAssistant.Core | [![NuGet version](https://badge.fury.io/nu/YY.EventLogExportAssistant.Core.svg)](https://badge.fury.io/nu/YY.EventLogExportAssistant.Core) | Базовый пакет |
| YY.EventLogExportAssistant.SQLServer | [![NuGet version](https://badge.fury.io/nu/YY.EventLogExportAssistant.SQLServer.svg)](https://badge.fury.io/nu/YY.EventLogExportAssistant.SQLServer) | Пакет для экспорта в базу SQL Server |
| YY.EventLogExportAssistant.PostgreSQL | [![NuGet version](https://badge.fury.io/nu/YY.EventLogExportAssistant.PostgreSQL.svg)](https://badge.fury.io/nu/YY.EventLogExportAssistant.PostgreSQL) | Пакет для экспорта в базу PostgreSQL |
| YY.EventLogExportAssistant.MySQL | [![NuGet version](https://badge.fury.io/nu/YY.EventLogExportAssistant.MySQL.svg)](https://badge.fury.io/nu/YY.EventLogExportAssistant.MySQL) | Пакет для экспорта в базу MySQL |
| YY.EventLogExportAssistant.ElasticSearch | [![NuGet version](https://badge.fury.io/nu/YY.EventLogExportAssistant.ElasticSearch.svg)](https://badge.fury.io/nu/YY.EventLogExportAssistant.ElasticSearch) | Пакет для экспорта в индексы ElasticSearch |
| YY.EventLogExportAssistant.ClickHouse | [![NuGet version](https://badge.fury.io/nu/YY.EventLogExportAssistant.ClickHouse.svg)](https://badge.fury.io/nu/YY.EventLogExportAssistant.ClickHouse) | Пакет для экспорта в базу ClickHouse |

Решение для экспорта данных журнала регистрации платформы 1С:Предприятие 8.x в нестандартные хранилища данных.
С помощью библиотеки **[YY.EventLogReaderAssistant](https://github.com/YPermitin/YY.EventLogReaderAssistant)** реализовано чтение данных журнала регистрации как текстового формата (*.lgf, *.lgp), так и нового формата в виде SQLite-базы (*.lgd).

Последние новости об этой и других разработках, а также выходе других материалов, **[смотрите в Telegram-канале](https://t.me/DevQuietPlace)**.

### Состояние сборки

| Windows |  Linux |
|:-------:|:------:|
| [![Build status](https://ci.appveyor.com/api/projects/status/lm4hex3gooyvaes2?svg=true)](https://ci.appveyor.com/project/YPermitin/yy-eventlogexportassistant) | [![Build Status](https://travis-ci.org/YPermitin/YY.EventLogExportAssistant.svg?branch=master)](https://travis-ci.org/YPermitin/YY.EventLogExportAssistant) |

### Code Climat

[![Maintainability](https://api.codeclimate.com/v1/badges/53754cae357167e851bd/maintainability)](https://codeclimate.com/github/YPermitin/YY.EventLogExportAssistant/maintainability)

## Состав репозитория

* Библиотеки
  * YY.EventLogExportAssistant.Core - ядро библиотеки с основным функционалом чтения и передачи данных.
  * YY.EventLogExportAssistant.SQLServer - функционал для экспорта данных в базу SQL Server.
  * YY.EventLogExportAssistant.PostgreSQL - функционал для экспорта данных в базу PostgreSQL.
  * YY.EventLogExportAssistant.MySQL - функционал для экспорта данных в базу MySQL.
  * YY.EventLogExportAssistant.ElasticSearch - функционал для экспорта данных в индексы ElasticSearch.
  * YY.EventLogExportAssistant.ClickHouse - функционал для экспорта данных в базу ClickHouse.
* Примеры приложений
  * YY.EventLogExportToSQLServer - пример приложения для экспорта данных в базу SQL Server.
  * YY.EventLogExportToPostgreSQL - пример приложения для экспорта данных в базу PostgreSQL.
  * YY.EventLogExportToMySQL - пример приложения для экспорта данных в базу MySQL.
  * YY.EventLogExportToElasticSearch - пример приложения для экспорта данных в индексы ElasticSearch.
  * YY.EventLogExportToClickHouse - пример приложения для экспорта данных в базу ClickHouse.

## Требования и совместимость

Работа библиотеки тестировалась со следующими версиями компонентов:

* Платформа 1С:Предприятие версии от 8.3.6 и выше.
* SQL Server 2012 и более новые.
* PostgreSQL 9.6 и выше.
* MySQL 5.7 и выше.
* ElasticSearch 7.6 и выше.
* ClickHouse 20.9 и выше.

В большинстве случаев работоспособность подтверждается и на более старых версиях ПО, но меньше тестируется. Основная разработка ведется для Microsoft Windows, но некоторый функционал проверялся под *.nix.*

## Пример использования

Репозиторий содержит несколько примеров консольных приложений для экспорта данных:

* YY.EventLogExportToSQLServer
* YY.EventLogExportToPostgreSQL
* YY.EventLogExportToMySQL
* YY.EventLogExportToElasticSearch
* YY.EventLogExportToClickHouse

Для удобства приведем основной небольшой пример для выгрузки данных журнала регистрации в базу SQL Server.

### Конфигурация

Первое, с чего следует начать - это конфигурационный файл приложения "appsettings.json". Это JSON-файл со строкой подключения к базе данных, сведениями об информационной системе и параметрами обработки журнала регистрации. Располагается в корне каталога приложения.

```json
{
  "ConnectionStrings": {
    "EventLogDatabase": "Host=localhost;Port=5432;Database=EventLog;Username=YourUser;Password=YourPassword"
  },
  "InformationSystem": {
    "Name": "Бухгалтерия предприяния 3.0 (рабочая база)",
    "Description": "Журнал регистрации основной рабочей базы БУ 3.0"
  },
  "EventLog": {
    "SourcePath": "C:\\Program Files\\1cv8\\srvinfo\\reg_1541\\3f54d9a8-5457-41ad-9b43-10207c36f144\\1Cv8Log",
    "UseWatchMode": true,
    "WatchPeriod": 5,
    "Portion": 10000
  }
}
```

Секция **"ConnectionStrings"** содержит строку подключения **"EventLogDatabase"** к базе данных для экспорта. База будет создана автоматически при первом запуске приложения. Также можно создать ее вручную, главное, чтобы структура была соответствующей. Имя строки подключения **"EventLogDatabase"** - это значение по умолчанию. Контекст приложения будет использовать ее автоматически, если это не переопределено разработчиком явно.

Примеры строк подключения:

  * **SQLServer**: "Data Source=<Имя или адрес сервера>;Initial Catalog=<Имя базы данных>;Integrated Security=True"
  * **PostgreSQL**: "User ID=<Имя пользователя>;Password=<Пароль>;Host=<Имя или адрес сервера>;Port=5432;Database=<Имя базы данных>;"
  * **MySQL**: "Server=<Имя или адрес сервера>;Database=<Имя базы данных>;Uid=<Имя пользователя>;Pwd=<Пароль>;"
  * **ClickHouse**: "Host=<Имя или адрес сервера>;Port=8123;Username=<Имя пользователя>;password=<Пароль>;Database=<Имя базы данных>;"

Подробнее о настройке подключения можете прочитать в официальной документации к каждой из СУБД.

Секция **"InformationSystem"** содержит название информационной системы и ее описание. Информационная система позволяет разделять хранение журналов регистрации разных баз 1С в одной базе данных.

Секция **"EventLog"** содержит параметры обработки журнала регистрации:

* **SourcePath** - путь к каталогу с файлами журнала регистрации. Может быть указан только каталог или конкретный файл журнала (1Cv8.lgf или 1Cv8.lgd).
* **UseWatchMode** - при значении false приложение завершит свою работу после загрузки всех данных. При значении true будет отслеживать появления новых данных пока приложение не будет явно закрыто.
* **WatchPeriod** - период в секундах, с которым приложение будет проверять наличие изменений. Используется, если параметр "UseWatchMode" установлен в true.
* **Portion** - количество записей, передаваемых в одной порции в базу данных из журнала регистрации.

Настройки "UseWatchMode" и "WatchPeriod" не относятся к библиотеке. Эти параметры добавлены лишь для примеров консольных приложений и используются в них же.

Для экспорта данных в ElasticSearch настройки несколько отличаются.

```json
{
  "ElasticSearch": {
    "Node": "http://localhost:9200/",
    "IndexName": "YourIndexName",
    "MaximumRetries": 2,
    "MaxRetryTimeout": 60,
    "IndexSeparationPeriod": "Hour"
  },
  "InformationSystem": {
    "Name": "EventLogGenerator",
    "Description": "Экспериметнальное решение для генерации файлов журнала регистрации."
  },
  "EventLog": {
    "SourcePath": "\\\\SRV-1C-01-VM\\1Cv8Log",
    "UseWatchMode": true,
    "WatchPeriod": 5,
    "Portion": 10000
  }
}
```

В секции "ElasticSearch" файла конфигурации задаются дополнительные настройки, относящиеся только к ES:

* **Node** - адрес службы ElasticSearch.
* **IndexName** - имя индекса. Фактически это начало имени индекса. Финальное имя будет зависеть от параметров разделения данных по индексам и типа данных. Например, "indexname-logdata-20200412070000" - имя содержит основную часть, затем тип "logdata" и дату записей.
* **MaximumRetries** - максимальное количество повторных попыток отправки запроса.
* **MaxRetryTimeout** - максимальный таймаут при попытке отправки повторных запросов.
* **IndexSeparationPeriod** - принцип разделения записей по индексам (None, Hour, Day, Week, Month, Quarter, HalfYear, Year). Если выбрано значение "None", то в имени будет содержаться название "FULL".

При экспорте создаются три вида индексов:

* **[имя индекса]-logdata-[период]** - индекс с записями журнала регистрации.
* **[имя индекса]-logfiles-actual** - индекс с информацией о последних считанных данных в разрезе информационных систем.
* **[имя индекса]-logfiles-history** - индекс с информацией об истории обработанных файлов данных в разрезе информационных систем.

В остальном экспорт работает также, как и при использовании SQL Server / PostgreSQL.

### Пример использования

На следующем листинге показан пример использования библиотеки.

```csharp
#region Private Static Member Variables

private static long _totalRows = 0;
private static long _lastPortionRows = 0;
private static DateTime _beginPortionExport;
private static DateTime _endPortionExport;

#endregion

#region Static Methods

static void Main()
{
    // 1. Инициализация настроек из файла "appsettings.json"
    IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

    IConfigurationSection eventLogSection = Configuration.GetSection("EventLog");
    string eventLogPath = eventLogSection.GetValue("SourcePath", string.Empty);
    int watchPeriodSeconds = eventLogSection.GetValue("WatchPeriod", 60);
    int watchPeriodSecondsMs = watchPeriodSeconds * 1000;
    bool useWatchMode = eventLogSection.GetValue("UseWatchMode", false);
    int portion = eventLogSection.GetValue("Portion", 1000);

    IConfigurationSection inforamtionSystemSection = Configuration.GetSection("InformationSystem");
    string inforamtionSystemName = inforamtionSystemSection.GetValue("Name", string.Empty);
    string inforamtionSystemDescription = inforamtionSystemSection.GetValue("Description", string.Empty);

    if (string.IsNullOrEmpty(eventLogPath))
    {
        Console.WriteLine("Не указан каталог с файлами данных журнала регистрации.");
        Console.WriteLine("Для выхода нажмите любую клавишу...");
        Console.Read();
        return;
    }

    Console.WriteLine();
    Console.WriteLine();

    // 2. (опционально) Инициализация настроек подключения к базе данных для экспорта
    string connectionString = Configuration.GetConnectionString("EventLogDatabase");
    DbContextOptions<EventLogContext> options = new DbContextOptions<EventLogContext>();
    var optionsBuilder = new DbContextOptionsBuilder<EventLogContext>();
    optionsBuilder.UseSqlServer(connectionString);

    // 3. Создаем объект для экспорта данных
    using(EventLogExportMaster exporter = new EventLogExportMaster())
    {
        // 3.1. Устанавливаем каталог с файлами журнала регистрации
        exporter.SetEventLogPath(eventLogPath);

        // 3.2. Инициализируем назначение экспорта данных. Для каждого назначения - свой класс, наследуемый от класса
        // "EventLogOnTarget" и устанавливаем в нем информационную систему для выгрузки.
        // Для SQL Server - "EventLogOnSQLServer"
        // Для PostgreSQL - "EventLogOnPostgreSQL"
        // Для ClickHouse - "EventLogOnClickHouse"
        // Для ElasticSearch - "EventLogOnElasticSearch"
        // Для MySQL - "EventLogOnMySQL"
        // Можно создать собственный класс для выгрузки в произвольное хранилище.
        EventLogOnSQLServer target = new EventLogOnSQLServer(optionsBuilder.Options, portion);
        target.SetInformationSystem(new InformationSystemsBase()
        {
            Name = inforamtionSystemName,
            Description = inforamtionSystemDescription
        });

        // 4. Устанавливаем назначение экспорта
        exporter.SetTarget(target);

        // 5. Подписываемся на события экспорта данных
        // 5.1. Событие "Перед отправкой данных"
        exporter.BeforeExportData += BeforeExportData;
        // 5.2. Событие "После отправки данных"
        exporter.AfterExportData += AfterExportData;         

        // 6. Выгрузка данных
        _beginPortionExport = DateTime.Now;
        if (useWatchMode)
        {
            // При настройке "WatchMode" = true выгружаем все накопившиеся данные,
            // а после проверяем новые данные для выгрузки каждые N секунд из настройки "WatchPeriod"
            while (true)
            {
                if (Console.KeyAvailable)
                    if (Console.ReadKey().KeyChar == 'q')
                        break;

                while (exporter.NewDataAvailiable())
                {
                    exporter.SendData();
                    Thread.Sleep(watchPeriodSecondsMs);
                }                    
            }
        } else // При настройке "WatchMode" = false просто выгружаем все накопившиеся данные
            while (exporter.NewDataAvailiable())
                exporter.SendData();
    }

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("Для выхода нажмите любую клавишу...");
    Console.Read();
}

#endregion
```

Так выглядят примеры обработчиков событий "Перед экспортом данных" и "После экспорта данных".

```csharp
#region Events

private static void BeforeExportData(BeforeExportDataEventArgs e)
{
    _lastPortionRows = e.Rows.Count;
    _totalRows += e.Rows.Count;

    Console.SetCursorPosition(0, 0);
    Console.WriteLine("[{0}] Last read: {1}             ", DateTime.Now, e.Rows.Count);
}
private static void AfterExportData(AfterExportDataEventArgs e)
{
    _endPortionExport = DateTime.Now;
    var duration = _endPortionExport - _beginPortionExport;

    Console.WriteLine("[{0}] Total read: {1}            ", DateTime.Now, _totalRows);
    Console.WriteLine("[{0}] {1} / {2} (sec.)           ", DateTime.Now, _lastPortionRows, duration.TotalSeconds);
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("Нажмите 'q' для завершения отслеживания изменений...");
    
    _beginPortionExport = DateTime.Now;
}

#endregion
```

С их помощью можно проанализировать какие данные будут выгружены и отказаться от выгрузки с помощью поля "Cancel" в параметре события "BeforeExportDataEventArgs" в событии "Перед экспортом данных". В событии "После экспорта данных" можно проанализировать выгруженные данные.

## Cценарии использования

Библиотека может быть использования для создания приложений для экспорта стандартного журнала регистрации платформы 1С:Предприяние 8.ч в нестандартные хранилища. На текущий момент доступна выгрузка в базы данных PostgreSQL и SQL Server.

Основные цели выгрузки - создать более производительный и эффективный способ работы с журналом регистрации с минимальным риском нарушить штатную работу платформы 1С. Может использоваться, например для:

* Контроля состояния системы на постоянной основе (периодические рассылки, проверка ошибок в течении рабочего и др.)
* Долгосрочное хранение информации о действиях в информационной базе с удобным способом хранения, бэкапирования и развертывания.
* Создание возможности работать с журналом регистрации с помощью стандартных запросов платформы 1С через внешние источники данных или ADO.
* Возможность работы с данными журнала регистрации с помощью средств вне экосистемы платформы 1С.

И это не полный список, все зависит от конкретных задач.

## Производительность

Производительность работы библиотеки сбалансирована для работы с достаточно большим объемом данных в журнале регистрации. Также при экспорте данных не нарушается работа платформы 1С, которая продолжает работать как обычно и не подозревает о происходящих процессах выгрузки.

Скорость экспорта записей журнала регистрации зависит от мощности оборудования, загруженности системы и конфигурации инфраструктуры.

При тестировании экспорта данных между двумя серверами (сервер приложений 1С и сервер баз данных с базой для экспорта журнала регистрации) со средней нагрузкой и каналом связи 1 Гбит/сек были получены следующие результаты. Тестовая машина также имела 16 ядер и процессор Intel Core i9900k.

| № | СУБД | Порция данных | Среднее время выгрузки (сек.) | Среднее использование CPU приложением, % | Среднее использование RAM приложением, МБ |
| - | ---- | ------------- | ----------------------------- | ----------------------- | ----------------------- |
| 1 | SQL Server | 10000 | 0.27 | 0.7 | 60 |
| 2 | PostgreSQL | 10000 | 0.32 | 0.8 | 97 |
| 3 | MySQL | 10000 | 2.91 | 3 | 130 |
| 4 | ElasticSearch | 10000 | 0.67 | 0.9 | 48 |
| 5 | ClickHouse | 10000 | 0.08 | 1.2 | 70 |

В целом не важно какая СУБД используется для хранения данных журнала регистрации. Разница в производительности на уровне статистической погрешности. В обоих вариантах время выгрузки около 35 тыс. записей журнала регистрации в минуту. Не часто можно встретить информационную базу, которая генерирует такой объем записей, но и она не будет препятствием для использования этой библиотеки выгрузки.

Для MySQL текущая версии библиотеки имеет более низкую производительность из-за технических особенностей.

## TODO

Планы в части разработки:

* Добавить возможность экспорта данных в MongoDB
* Улучшить обработку ошибок по уровням возникновения (критические и нет)
* Улучшение производительности и добавление bencmark'ов
* Расширение unit-тестов библиотеки

## Лицензия

MIT - делайте все, что посчитаете нужным. Никакой гарантии и никаких ограничений по использованию.
