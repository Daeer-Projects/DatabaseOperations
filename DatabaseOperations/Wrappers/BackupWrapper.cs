using DatabaseOperations.Interfaces;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseOperations.Wrappers
{
    public class BackupWrapper : IBackupWrapper
	{
        private readonly Backup _sqlBackup = new Backup();

        public BackupActionType ActionType
        {
            get => _sqlBackup.Action;
            set => _sqlBackup.Action = value;
        }

        public string BackupSetDescription
        {
            get => _sqlBackup.BackupSetDescription;
            set => _sqlBackup.BackupSetDescription = value;
        }

        public string Database
        {
            get => _sqlBackup.Database;
            set => _sqlBackup.Database = value;
        }

        public string BackupSetName
        {
            get => _sqlBackup.BackupSetName;
            set => _sqlBackup.BackupSetName = value;
        }

        public bool Initialize
        {
            get => _sqlBackup.Initialize;
            set => _sqlBackup.Initialize = value;
        }

        public bool Checksum
        {
            get => _sqlBackup.Checksum;
            set => _sqlBackup.Checksum = value;
        }

        public bool ContinueAfterError
        {
            get => _sqlBackup.ContinueAfterError;
            set => _sqlBackup.ContinueAfterError = value;
        }

        public bool Incremental
        {
            get => _sqlBackup.Incremental;
            set => _sqlBackup.Incremental = value;
        }

        public BackupTruncateLogType LogTruncation
        {
            get => _sqlBackup.LogTruncation;
            set => _sqlBackup.LogTruncation = value;
        }

        public bool FormatMedia
        {
            get => _sqlBackup.FormatMedia;
            set => _sqlBackup.FormatMedia = value;
        }

        public void SqlBackup(IServerWrapper server)
        {
            _sqlBackup.SqlBackup(server.GetServer());
        }

        public void AddDevice(BackupDeviceItem device)
        {
            _sqlBackup.Devices.Add(device);
        }

        public void RemoveDevice(BackupDeviceItem device)
        {
            _sqlBackup.Devices.Remove(device);
        }

        public BackupDeviceList GetDeviceList()
        {
            return _sqlBackup.Devices;
        }
	}
}
