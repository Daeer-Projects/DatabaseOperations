# DatabaseOperations

The purpose of this library is to provide the consumer with tools to perform database operations.  The library has been designed for use in deployment/utilities to automate the tasks usually done manually.

**NOTE**:

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

To perform the backup operation synchronously:

```csharp
const string connectionString = @"server=MyComputer\SQLDEV;database=TestDatabase;Integrated Security=SSPI;Connect Timeout=5;";
const string backupPath = @"E:\Database\Backups\";

var result = backupOperator.BackupDatabase(new ConnectionOptions(connectionString, backupPath));
```

To perform the backup asynchronously (for many backup operations):

```csharp
const string connectionStringOne = @"server=MyComputer\SQLDEV;database=DatabaseOne;Integrated Security=SSPI;Connect Timeout=5;";
const string connectionStringTwo = @"server=MyComputer\SQLDEV;database=DatabaseTwo;Integrated Security=SSPI;Connect Timeout=5;";
const string connectionStringThree = @"server=MyComputer\SQLDEV;database=DatabaseTwo;Integrated Security=SSPI;Connect Timeout=5;";
const string backupPath = @"E:\Database\Backups\";
var cancellationSource = new CancellationTokenSource();
var token = cancellationSource.Token;

var resultOne = backupOperator.BackupDatabaseAsync(new ConnectionOptions(connectionStringOne, backupPath), token);
var resultTwo = backupOperator.BackupDatabaseAsync(new ConnectionOptions(connectionStringTwo, backupPath), token);
var resultThree = backupOperator.BackupDatabaseAsync(new ConnectionOptions(connectionStringThree, backupPath), token);

await Task.WhenAll(resultOne, resultTwo, resultThree).ConfigureAwait(false);

var taskOne = await resultOne.ConfigureAwait(false);
var taskTwo = await resultTwo.ConfigureAwait(false);
var taskThree = await resultThree.ConfigureAwait(false);
```

### Cancellation of the operation

The `CancellationTokenSource` object can be created with a timeout in milliseconds, or a `TimeSpan`.

The token source can be cancelled from any event, or when the timeout is reached.

If no token is supplied to the call, it will create one, but the calling application will not be able to raise the cancellation of the task.

