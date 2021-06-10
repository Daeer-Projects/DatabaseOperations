# DatabaseOperations

The purpose of this library is to provide the consumer with tools to perform database operations.  The library has been designed for use in deployment/utilities to automate the tasks usually done manually.

**NOTE**
Microsoft SQL Server Databases only.

## Introduction

I have used tools in Microsoft SQL Server Management Studio or Azure Data Studio to back up databases.  The programs provided me with the tools needed to conduct the tasks required.

The problem came when I had to create a tool used in C# to do the same process for me. Now the process is easy using a small SQL script. However, when I went looking for a library on NuGet for me to use, there were none.

So, I made this library that I am now putting out there for other people to use.

## Usages

There are more details on the project [wiki](https://github.com/daeer73/DatabaseOperations/wiki).  Here are some basics.

### Backup Operator

To create a new instance of the `BackupOperator`:

```csharp
var backupOperator = new BackupOperator();
```

To perform the backup operation:

```csharp
const string connectionString = @"server=MyComputer\SQLDEV;database=TestDatabase;Integrated Security=SSPI;Connect Timeout=5;";
const string backupPath = @"E:\Database\Backups\";

var result = backupOperator.BackupDatabase(new ConnectionOptions(connectionString, backupPath));
```

## Milestones

The first milestone is going to be the initial release as a NuGet package.
