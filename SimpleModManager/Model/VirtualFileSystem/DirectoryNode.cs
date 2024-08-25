using System.Text;

namespace SimpleModManager.Model.VirtualFileSystem;

public class DirectoryNode : Node
{
    public DirectoryNode(string name, DirectoryNode? parent = null) : base(name, parent)
    {
        Children = new List<Node>();
    }

    public DirectoryNode(string name, DirectoryNode? parent, params Node[] children) : base(name, parent)
    {
        Children = new List<Node>(children);
    }

    public List<Node> Children { get; }

    public static DirectoryNode CreateFromPath(string path)
    {
        var directoryInfo = new DirectoryInfo(path);
        var root = new DirectoryNode(directoryInfo.Name);
        foreach (var file in directoryInfo.GetFiles()) root.AddChild(new FileNode(file.Name));
        foreach (var directory in directoryInfo.GetDirectories()) root.AddChild(CreateFromPath(directory.FullName));
        return root;
    }

    public void AddChild(Node child)
    {
        child.Parent = this;
        Children.Add(child);
        SortChildren();
    }

    private void SortChildren()
    {
        Children.Sort((x, y) =>
        {
            return x switch
            {
                FileNode when y is DirectoryNode => -1,
                DirectoryNode when y is FileNode => 1,
                _ => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase)
            };
        });
    }

    public override string Display(int depth)
    {
        var sb = new StringBuilder();
        sb.Append(new string(' ', depth * 2) + Name + Path.DirectorySeparatorChar);
        foreach (var child in Children) sb.Append("\n" + child.Display(depth + 1));
        return sb.ToString();
    }
}