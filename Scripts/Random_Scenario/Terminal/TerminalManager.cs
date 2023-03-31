using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalManager : MonoBehaviour
{
    [SerializeField] private GameObject directoryLine;
    [SerializeField] private GameObject responseLine;
    
    [SerializeField] private TMP_InputField terminalInput;
    [SerializeField] private GameObject userInputLine;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject msgList;
    
    TerminalInterpreter interpreter;
    
    private void Start()
    {
        interpreter = GetComponent<TerminalInterpreter>();
    }

    private void OnGUI()
    {
        if (terminalInput.isFocused && terminalInput.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            string userInput = terminalInput.text;
            
            ClearInputField();
            
            AddDirectoryLine(userInput);

            var lines = AddInterpreterLines(interpreter.Interpret(userInput));
            
            ScrollToBottom(lines);
            
            userInputLine.transform.SetAsLastSibling();
            terminalInput.ActivateInputField();
            terminalInput.Select();
        }
    }
    
    private void ClearInputField()
    {
        terminalInput.text = "";
    }
    
    private void AddDirectoryLine(string userInput)
    {
        Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
        msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 35.0f);
        
        GameObject newLine = Instantiate(directoryLine, msgList.transform);
        newLine.transform.SetSiblingIndex(msgList.transform.childCount - 1);
        newLine.GetComponentsInChildren<TMP_Text>()[1].text = userInput;
    }

    int AddInterpreterLines(List<string> interpretation)
    {
        for (int i = 0; i < interpretation.Count; i++)
        {
            GameObject responseMsg = Instantiate(responseLine, msgList.transform);
            responseMsg.transform.SetAsLastSibling();
            Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
            msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 35.0f);
            responseMsg.GetComponentInChildren<TMP_Text>().text = interpretation[i];
        }
        
        return interpretation.Count;
    }
    
    private void ScrollToBottom(int lines)
    {
        if (lines > 4)
        {
            scrollRect.velocity = new Vector2(0, 450);
        }
        else
        {
            scrollRect.verticalNormalizedPosition = 0.0f;
        }
    }
    
    public void ClearTerminal()
    {
        int childCount = msgList.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = msgList.transform.GetChild(i);
            if (i != 0 && i != 1 && i != childCount - 1)
            {
                Destroy(child.gameObject);
            }
        }
    }

}
