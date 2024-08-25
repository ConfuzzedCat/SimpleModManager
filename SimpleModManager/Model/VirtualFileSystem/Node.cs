namespace SimpleModManager.Model.VirtualFileSystem;

public abstract class Node
{
    public string Name { get; set; }

    public DirectoryNode? Parent { get; set; }

    public string Display()
    {
        return Display(0);
    }

    public abstract string Display(int depth);

    public Node(string name, DirectoryNode? parent = null)
    {
        Name = name;
        Parent = parent;
    }
}