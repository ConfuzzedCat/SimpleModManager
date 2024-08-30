
using System.Text;

namespace SimpleModManager.Model.VirtualFileSystem;

public sealed class Node
{
    public Node(string name, string realPath)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Argument can't be null or empty", nameof(name));
        }

        Id = Guid.NewGuid();
        Name = name;
        RealPath = realPath;
        Children = new List<Node>();
    }

    public Node(string name, string realPath, params Node[] children) : this (name, realPath)
    {
        Children = [..children];
    }

    public Guid Id { get; }
    public string Name { get; set; }
    public string RealPath { get; set; }
    public List<Node> Children { get; }

    public void AddChild(Node child)
    {
        Children.Add(child);
    }

    public string Display(int depth = 0)
    {
        return IsFile() ? DisplayFile(depth) : DisplayDir(depth);
    }

    private string DisplayDir(int depth = 0)
    {
        var sb = new StringBuilder();
        sb.Append(new string(' ', depth * 2) + Name + Path.DirectorySeparatorChar);
        foreach (var child in Children) sb.Append("\n" + child.Display(depth + 1));
        return sb.ToString();
    }
    private string DisplayFile(int depth = 0)
    {
        return new string(' ', depth * 2) + Name;
    }

    public bool IsDir()
    {
        return Directory.Exists(RealPath);
    }

    public bool IsFile()
    {
        return File.Exists(RealPath);
    }
}