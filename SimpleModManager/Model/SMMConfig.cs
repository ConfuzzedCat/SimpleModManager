namespace SimpleModManager.Model;

public sealed class SmmConfig
{
    public SmmConfig(string stagingDir, string archiveDir, bool rememberOverwriteChoose)
    {
        ArchiveDir = archiveDir;
        StagingDir = stagingDir;
        RememberOverwriteChoose = rememberOverwriteChoose;
    }

    public string ArchiveDir { get; set; } // TODO: Sanitize path using wildcard
    public string StagingDir { get; set; } // TODO: Sanitize path using wildcard

    public bool RememberOverwriteChoose { get; set; }
}