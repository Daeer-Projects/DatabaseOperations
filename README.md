# DatabaseOperations

The purpose of this library is to provide the consumer with tools to perform database operations.  The library has been designed for use in deployment/utilities to automate the tasks usually done manually.

[![Build Status](https://dev.azure.com/DaeerProjects/DatabaseOperations/_apis/build/status/Daeer-Projects.DatabaseOperations?branchName=main)](https://dev.azure.com/DaeerProjects/DatabaseOperations/_build/latest?definitionId=11&branchName=main)

**NOTE**:  
Microsoft SQL Server Databases only.

## Introduction

I have used tools in Microsoft SQL Server Management Studio or Azure Data Studio to back up databases.  The programs provided me with the tools needed to conduct the tasks required.

The problem came when I had to create a tool used in C# to do the same process for me.  Now the process is easy using a small SQL script.  However, when I went looking for a library on NuGet for me to use, there were none.

So, I made this library that I am now putting out there for other people to use.

### Description

This repo is a library that will grow with more features to help automate database operations.  I welcome any feedback and features to add.

## How to get setup

The project is a Visual Studio solution (`.sln`).

1. Fork, clone or download the repo.
2. Open the solution in your favourite IDE (VS, Rider, VSCode, etc).
3. Build the solution.
4. Run the unit tests.

## How to use the library

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
BackupOperator backupOperator = new();
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

## Credits

Although I am the creator of this repo, I did have help creating the original version for the company I work at.

* Adam Jackson.
* Ashley Izat.
* Leah Wood.

## License

This repo and library are using the MIT license.

## Contributing

See the included [Contributing File](https://github.com/Daeer-Projects/DatabaseOperations/blob/main/CONTRIBUTING.md) for more information.

## Testing

The project does have a unit test project that covers as much as possible of the code base.

There is also a test app that can be found here: [Test App Repo](https://github.com/Daeer-Projects/DatabaseOperations-TestApp)

This will show more usages and examples of how to consume the library.
