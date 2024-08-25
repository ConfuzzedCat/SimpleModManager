namespace SimpleModManager.Model.VirtualFileSystem;

public abstract class Node
{
    public Node(string name, DirectoryNode? parent = null)
    {
        Name = name;
        Parent = parent;
    }

    public string Name { get; set; }

    public DirectoryNode? Parent { get; set; }

    public abstract string Display(int depth = 0);

    public abstract bool IsDir();
    public abstract bool IsFile();

    public string GetPath()
    {
        if (Parent is null)
        {
            return IsDir() ? Name + Path.DirectorySeparatorChar : Name;
        }

        return Parent.GetPath() + Path.DirectorySeparatorChar + Name;
    }
}