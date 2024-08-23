namespace SimpleModManager.Model;

public sealed class SmmConfig
{
    public string StagingDir { get; set; }
    public string ArchiveDir { get; set; }

    public SmmConfig(string stagingDir, string archiveDir)
    {
        StagingDir = stagingDir;
        ArchiveDir = archiveDir;
    }
}