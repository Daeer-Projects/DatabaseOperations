# DatabaseOperations

The purpose of this library is to provide the consumer with tools to perform database operations.  The library has been designed for use in deployment/utilities to automate the tasks usually done manually.

**NOTE**:

Microsoft SQL Server Databases only.

## Introduction

I have used tools in Microsoft SQL Server Management Studio or Azure Data Studio to back up databases.  The programs provided me with the tools needed to conduct the tasks required.

The problem came when I had to create a tool used in C# to do the same process for me.  Now the process is easy using a small SQL script.  However, when I went looking for a library on NuGet for me to use, there were none.

So, I made this library that I am now putting out there for other people to use.

### Description

This repo is a library that will grow with more features to help automate database operations.  I welcome any feedback and features to add.

## Usages

There are more details on the project [wiki](https://github.com/daeer73/DatabaseOperations/wiki).  Here are some basics.

### Backup Operator

The `BackupOperator` is the only operator in the project at the moment.  Other operators will be added in due course.

#### Construction

The constructor requires no parameters.

**Signature:**

```csharp
public BackupOperator()
```

```csharp
var backupOperator = new BackupOperator();
```

#### Backup Database methods

There are two methods to enable the user to backup a database.  Both methods have an `async` version.

##### Original Versions - will be made obsolete

```csharp
public OperationResult BackupDatabase(ConnectionOptions options)

public async Task<OperationResult> BackupDatabaseAsync(
            ConnectionOptions options,
            CancellationToken token = default)
```

##### New Versions - the preferred method

```csharp
public OperationResult BackupDatabase(
            string connectionString,
            Action<OperatorOptions>? options = null)

public async Task<OperationResult> BackupDatabaseAsync(
            string connectionString,
            Action<OperatorOptions>? options = null,
            CancellationToken token = default)
```

The `OperatorOptions` class has defaults set, so can be left out of the call.  This would mean the backup would be saved to the default location used by SQL Server and the execution timeout will be set to one hour.

To perform the backup operation synchronously:

```csharp
const string connectionString = @"server=MyComputer\SQLDEV;database=TestDatabase;Integrated Security=SSPI;Connect Timeout=5;";
const string backupPath = @"E:\Database\Backups\";

OperationResult result = backupOperator.BackupDatabase(connectionString, options => {
    options.BackupPath = @"E:\Database\Backups\";
    options.Timeout = 600;
});
```

To perform the backup asynchronously (for many backup operations):

```csharp
// Define the connection strings.
const string connectionStringOne = @"server=MyComputer\SQLDEV;database=DatabaseOne;Integrated Security=SSPI;Connect Timeout=5;";
const string connectionStringTwo = @"server=MyComputer\SQLDEV;database=DatabaseTwo;Integrated Security=SSPI;Connect Timeout=5;";
const string connectionStringThree = @"server=MyComputer\SQLDEV;database=DatabaseTwo;Integrated Security=SSPI;Connect Timeout=5;";

// Define the backup path.
const string backupPath = @"E:\Database\Backups\";
var cancellationSource = new CancellationTokenSource();
var token = cancellationSource.Token;

// Start the backup operations in an asynchronous manor.
Task<OperationResult> resultOne = backupOperator.BackupDatabaseAsync(connectionStringOne, options => {
    options.BackupPath = backupPath;
}, token);
Task<OperationResult> resultTwo = backupOperator.BackupDatabaseAsync(connectionStringTwo, options => {
    options.BackupPath = backupPath;
}, token);
Task<OperationResult> resultThree = backupOperator.BackupDatabaseAsync(connectionStringThree, options => {
    options.BackupPath = backupPath;
}, token);

await Task.WhenAll(resultOne, resultTwo, resultThree).ConfigureAwait(false);

// Get the results.
OperationResult taskOne = await resultOne.ConfigureAwait(false);
OperationResult taskTwo = await resultTwo.ConfigureAwait(false);
OperationResult taskThree = await resultThree.ConfigureAwait(false);
```

### Cancellation of the operation

The `CancellationTokenSource` object can be created with a timeout in milliseconds or a `TimeSpan`.

The token source can be cancelled from any event, or when the timeout is reached.

If no token is supplied to the call, it will create one, but the calling application will not be able to raise the cancellation of the task.
