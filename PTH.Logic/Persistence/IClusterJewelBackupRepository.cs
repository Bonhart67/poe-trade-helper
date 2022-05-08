namespace PTH.Logic.Persistence;

public interface IClusterJewelBackupRepository
{
    Task BackupCurrentClusterDetails();
    Task FeedAllData();
}