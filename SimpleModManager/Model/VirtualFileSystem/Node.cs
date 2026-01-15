
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

namespace SimpleModManager.Model.VirtualFileSystem;

public struct Node
{
    [JsonConstructor]
    public Node()
    {
        
    }
    public Node(string name, string absolutePath, string relativePath)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Argument can't be null or empty", nameof(name));
        }

        Id = Guid.NewGuid();
        Name = name;
        AbsolutePath = absolutePath;
        Children = new List<Node>();
        if (relativePath == "")
        {
            RelativePath = ".";
            return;
        }
        RelativePath = relativePath + Path.DirectorySeparatorChar + name;
    }


    public Node(string name, string absolutePath, string relativePath, params Node[] children) : this (name, absolutePath, relativePath)
    {
        Children = [..children];
    }

    public Guid Id { get; }
    public string Name { get; set; }
    public string AbsolutePath { get; set; }
    public string RelativePath { get; set; }
    private List<Node> Children { get; set; }

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
        return Directory.Exists(AbsolutePath);
    }

    public bool IsFile()
    {
        return File.Exists(AbsolutePath);
    }
    public List<Node> GetAllChildren()
    {
        var allChildren = new List<Node>();
        var stack = new Stack<Node>(Children);
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            allChildren.Add(current);
            foreach (var child in current.Children)
            {
                stack.Push(child);
            }
        }
        return allChildren;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    public bool DoesNodesConflict(Node other, out List<Node> conflicts)
    {
        conflicts = [];
        
        return IsFile() ? DoesNodesConflictFile(other) : DoesNodesConflictDir(other, out conflicts); 
    }
    
    private bool DoesNodesConflictFile(Node other)
    {
        return GetNodeRelativePath() == other.GetNodeRelativePath();
    }

    private string GetNodeRelativePath()
    {
        return $"{RelativePath}{Path.DirectorySeparatorChar}{Name}";
    }

    private bool DoesNodesConflictDir(Node other, out List<Node> conflicts)
    {
        conflicts = [];
        if (other.IsDir() && other.Name == Name)
        {
            
            bool flag = false;
            foreach (var child in Children)
            {
                flag = child.DoesNodesConflict(other, out var childConflicts);
                if (flag)
                {
                    conflicts.AddRange(childConflicts);
                }
            }
        }
        return GetNodeRelativePath() == other.GetNodeRelativePath();
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}