using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerminalInterpreter : MonoBehaviour
{
    [SerializeField] private PcManager _pcManager;
    
    private string dirName = "";
    
    [SerializeField] List<string> _folderStructure;
    public List<string> _dektopFiles;

    private FileSystemNode currentDirectory;

    private void Awake()
    {
        var pcManagerScript = _pcManager.GetComponent<PcManager>();
        var pcData = pcManagerScript.currentPcData;
        
        currentDirectory = pcData.fileSystemRoot;
    }

    public List<string> Interpret (string input)
    {
        var pcManagerScript = _pcManager.GetComponent<PcManager>();
        var pcData = pcManagerScript.currentPcData;

        List<string> output = new List<string>();
        string[] inputArray = input.Split(' ');
        string command = inputArray[0];
        string[] arguments = new string[inputArray.Length - 1];
        Array.Copy(inputArray, 1, arguments, 0, inputArray.Length - 1);
        switch (command)
        {
            case "help":
                output.Add("help - displays this message");
                output.Add("echo - displays a message");
                output.Add("cd - change directory");
                output.Add("ls - list directory contents");
                output.Add("rd - remove directory");
                output.Add("hostname - display host name");
                output.Add("getmac - display MAC address");
                output.Add("ipconfig - display IP network settings");
                output.Add("netstat - display TCP/IP connections and status");
                output.Add("exit - exits the game");
                break;
            case "ls":
                ListDirectory(currentDirectory, output);
                break;
            case "cd":
                if (arguments.Length == 0) {
                    output.Add("cd: missing operand");
                } else if (arguments.Length > 1) {
                    output.Add("cd: too many arguments");
                } else {
                    ChangeDirectory(ref currentDirectory, arguments[0], output);
                }
                break;
            case "rd":
                DeleteDirectory(currentDirectory, arguments[0], output);
                break;
            case "hostname":
                output.Add(pcData.hostame);
                break;
            case "getmac":
                output.Add(pcData.Mac);
                break;
            case "changemac":
                pcData.Mac = arguments[0];
                output.Add("MAC address changed");
                break;
            case "getfiles":
                output.Add("Files on desktop:");
                foreach (var file in pcData.files)
                {
                    output.Add(file.name);
                }
                break;
            case "deletefile":
                if (arguments.Length == 0)
                {
                    output.Add("Error: No arguments");
                }
                else
                {
                    foreach (var file in _dektopFiles)
                    {
                        if (arguments[0] == file)
                        {
                            _dektopFiles.Remove(file);
                            output.Add("File deleted");
                            break;
                        }
                    }
                }
                break;
            case "ipconfig":
                output.Add("Ethernet adapter Ethernet:");
                output.Add("   Connection-specific DNS Suffix  . :");
                output.Add("   IPv4 Address. . . . . . . . . . . : " + pcData.IP);
                output.Add("   Subnet Mask . . . . . . . . . . . : 255.255.255.0");
                output.Add("   Default Gateway . . . . . . . . . : 192.168.169.1");
                break;
            case "netstat":
                // Pc Data lfkwejflkwej
                output.Add("Active Connections");
                output.Add("  Proto  Local Address          Foreign Address        State");
                output.Add($"  TCP    {pcData.IP}           DESKTOP-HAC:0       LISTENING");
                break;
            case "clear":
                // Add code to clear the terminal
                output.Add("clear");
                break;
            case "echo":
                output.Add(string.Join(" ", arguments));
                break;
            case "exit":
                Application.Quit();
                break;
            default:
                output.Add("Unknown command: " + command);
                break;
        }

        return output; 
    }
    
    public void ChangeDirectory(ref FileSystemNode currentDirectory, string targetDirectoryName, List<string> output) {
        if (targetDirectoryName == "..") {
            if (currentDirectory.parent != null) {
                currentDirectory = currentDirectory.parent;
            }
            return;
        }

        foreach (FileSystemNode child in currentDirectory.children) {
            if (child.isDirectory && child.name == targetDirectoryName) {
                currentDirectory = child;
                return;
            }
        }

        output.Add("cd: no such file or directory");
    }


    public void ListDirectory(FileSystemNode currentDirectory, List<string> output) {
        foreach (FileSystemNode child in currentDirectory.children) {
            if (child.isDirectory) {
                output.Add(child.name + "/");
            } else {
                output.Add(child.name);
            }
        }
    }

    public void DeleteDirectory(FileSystemNode currentDirectory, string targetDirectoryName, List<string> output) {
        if (targetDirectoryName == "..") {
            output.Add("Cannot delete parent directory");
            return;
        }

        FileSystemNode targetDirectory = null;
        foreach (FileSystemNode child in currentDirectory.children) {
            if (child.isDirectory && child.name == targetDirectoryName) {
                targetDirectory = child;
                break;
            }
        }

        if (targetDirectory == null) {
            output.Add("Directory not found");
            return;
        }

        if (targetDirectory.children.Count > 0) {
            output.Add("Directory not empty");
            return;
        }

        currentDirectory.children.Remove(targetDirectory);
    }

}

