using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class DependantVariableEdit : MonoBehaviour
{
    [SerializeField] public InputField inputField;
    [SerializeField] public string unit;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void RemoveUnit()
    {
        string newString = Regex.Replace(inputField.text, "[^.0-9]", "");
        inputField.text = newString;
        
    }

    public void AddUnitBack()
    {
        string newString = "";

        newString = Regex.Replace(inputField.text, "[^.0-9]", "");
        if (newString.Equals("")) { newString = "0"; }
        double number = (double) System.Math.Round(double.Parse(newString),2);
        inputField.text = number.ToString("0." + new string('#', 339) + " " + unit);

    }
}
