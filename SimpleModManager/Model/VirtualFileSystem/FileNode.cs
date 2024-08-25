namespace SimpleModManager.Model.VirtualFileSystem;

public class FileNode : Node
{
    public FileNode(string name, DirectoryNode? parent = null) : base(name, parent)
    {
    }

    public override string Display(int depth)
    {
        return new string(' ', depth * 2) + Name;
    }
    public override bool IsDir()
    {
        return false;
    }

    public override bool IsFile()
    {
        return true;
    }
}