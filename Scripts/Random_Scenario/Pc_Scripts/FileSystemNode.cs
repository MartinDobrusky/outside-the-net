using System.Collections.Generic;

public class FileSystemNode {
    public string name;
    public bool isDirectory;
    public List<FileSystemNode> children;
    public FileSystemNode parent;

    public FileSystemNode(string name, bool isDirectory) {
        this.name = name;
        this.isDirectory = isDirectory;
        children = new List<FileSystemNode>();
    }
}


