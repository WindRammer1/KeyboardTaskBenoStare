using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualKeyboard : MonoBehaviour
{
    public MyInputField InputField;

    public void KeyPress(string c)
    {
        InputField.VirtualInputKey(c);
    }

    public void KeyLeft()
    {
        InputField.VirtualLeft();
    }

    public void KeyRight()
    {
        InputField.VirtualRight();
    }

    public void KeyBackspace()
    {
        InputField.VirtualBackspace();
    }
}