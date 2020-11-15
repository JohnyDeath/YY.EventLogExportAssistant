#!/bin/sh
API_KEY = $1

dotnet nuget push ./Libs/YY.EventLogExportAssistant.Core/bin/Release/YY.EventLogExportAssistant.Core.*.nupkg -k $1 -s https://api.nuget.org/v3/index.json --skip-duplicate
dotnet nuget push ./Libs/YY.EventLogExportAssistant.SQLServer/bin/Release/YY.EventLogExportAssistant.SQLServer.*.nupkg -k $1 -s https://api.nuget.org/v3/index.json --skip-duplicate
dotnet nuget push ./Libs/YY.EventLogExportAssistant.PostgreSQL/bin/Release/YY.EventLogExportAssistant.PostgreSQL.*.nupkg -k $1 -s https://api.nuget.org/v3/index.json --skip-duplicate
dotnet nuget push ./Libs/YY.EventLogExportAssistant.ElasticSearch/bin/Release/YY.EventLogExportAssistant.ElasticSearch.*.nupkg -k $1 -s https://api.nuget.org/v3/index.json --skip-duplicate
dotnet nuget push ./Libs/YY.EventLogExportAssistant.MySQL/bin/Release/YY.EventLogExportAssistant.MySQL.*.nupkg -k $1 -s https://api.nuget.org/v3/index.json --skip-duplicate
dotnet nuget push ./Libs/YY.EventLogExportAssistant.ClickHouse/bin/Release/YY.EventLogExportAssistant.ClickHouse.*.nupkg -k $1 -s https://api.nuget.org/v3/index.json --skip-duplicate