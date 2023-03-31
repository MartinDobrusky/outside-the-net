using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Passwords : MonoBehaviour
{
    [SerializeField] private GameObject firstSection;
    [SerializeField] private GameObject secondSection;
    [SerializeField] private GameObject passwordSection;
    
    [SerializeField] private Button nextSectionButton;
    
    public TMP_InputField passwordInput;
    public TMP_Text passwordStrengthText;

    private void Start()
    {
        // Add a listener to the password input field so that we can update the password strength
        // text every time the user types a character
        passwordInput.onValueChanged.AddListener(UpdatePasswordStrengthText);
    }
    
    public void NextSection()
    {
        firstSection.SetActive(false);
        secondSection.SetActive(true);
        passwordSection.SetActive(false);
    }

    private void UpdatePasswordStrengthText(string password)
    {
        // Calculate the password strength
        int passwordStrength = 0;

        // Add points for password length
        passwordStrength += Mathf.Clamp(password.Length, 0, 8);

        // Add points for presence of uppercase and lowercase letters
        if (HasLowerCaseLetter(password) && HasUpperCaseLetter(password))
        {
            passwordStrength += 2;
        }

        // Add points for presence of numbers
        if (HasNumber(password))
        {
            passwordStrength += 2;
        }

        // Add points for presence of symbols
        if (HasSymbol(password))
        {
            passwordStrength += 2;
        }

        // Set the password strength text based on the calculated strength
        if (passwordStrength < 4)
        {
            passwordStrengthText.text = "Very weak";
        }
        else if (passwordStrength < 8)
        {
            passwordStrengthText.text = "Weak";
        }
        else if (passwordStrength < 12)
        {
            passwordStrengthText.text = "Medium";
        }
        else if (passwordStrength < 16)
        {
            passwordStrengthText.text = "Strong";
            nextSectionButton.interactable = true;
        }
        else
        {
            passwordStrengthText.text = "Very strong";
        }
    }

    private bool HasLowerCaseLetter(string str)
    {
        foreach (char c in str)
        {
            if (char.IsLower(c))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasUpperCaseLetter(string str)
    {
        foreach (char c in str)
        {
            if (char.IsUpper(c))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasNumber(string str)
    {
        foreach (char c in str)
        {
            if (char.IsDigit(c))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasSymbol(string str)
    {
        foreach (char c in str)
        {
            if (!char.IsLetterOrDigit(c))
            {
                return true;
            }
        }

        return false;
    }
}
