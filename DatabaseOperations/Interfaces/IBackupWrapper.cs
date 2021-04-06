using Microsoft.SqlServer.Management.Smo;

namespace DatabaseOperations.Interfaces
{
    public interface IBackupWrapper
	{
        BackupActionType ActionType { get; set; }
        string BackupSetDescription { get; set; }
        string Database { get; set; }
        string BackupSetName { get; set; }
        bool Initialize { get; set; }
        bool Checksum { get; set; }
        bool ContinueAfterError { get; set; }
        bool Incremental { get; set; }
        BackupTruncateLogType LogTruncation { get; set; }
        bool FormatMedia { get; set; }
        void SqlBackup(IServerWrapper server);
        void AddDevice(BackupDeviceItem device);
        void RemoveDevice(BackupDeviceItem device);
        BackupDeviceList GetDeviceList();
	}
}
