using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LearningData : MonoBehaviour
{
    public TMP_Text[] checkboxTexts;
    public Color checkmarkColor = Color.green;
    public Color crossColor = Color.red;

    private readonly bool[] _isChecked = new bool[4];

    private void Start()
    {
        // Load the saved checkbox value from player prefs
        _isChecked[0] = PlayerPrefs.GetInt("Social", 0) == 1;
        _isChecked[1] = PlayerPrefs.GetInt("Malware", 0) == 1;
        _isChecked[2] = PlayerPrefs.GetInt("Passwords", 0) == 1;
        _isChecked[3] = PlayerPrefs.GetInt("Data", 0) == 1;

        // Update the checkbox text based on the loaded value
        UpdateCheckboxText();
    }

    private void UpdateCheckboxText()
    {
        for (var i = 0; i < checkboxTexts.Length; i++)
        {
            if (_isChecked[i])
            {
                checkboxTexts[i].text = ">";
                checkboxTexts[i].color = checkmarkColor;
            }
            else
            {
                checkboxTexts[i].text = "X";
                checkboxTexts[i].color = crossColor;
            }
        }
    }
}

