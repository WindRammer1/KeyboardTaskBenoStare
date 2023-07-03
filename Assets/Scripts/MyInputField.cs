using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class MyInputField : InputFieldOriginal
{
    private int caretPositionOnDeselect = 0;
    private int selectStartPos = 0;
    private int selectEndPos = 0;
    public string keyboardTag = "keyboard";
    private List<RaycastResult> rayResults = new List<RaycastResult>();

    private bool IsTextSelected(int startPos, int endPos)
    {
        return startPos != endPos;
    }
    private int GetFirst(int startPos, int endPos)
    {
        int first = (startPos < endPos) ? startPos : endPos;
        return first;
    }
    private int GetLast(int startPos, int endPos)
    {
        int last = (startPos > endPos) ? startPos : endPos;
        return last;
    }

    void DeleteSelected(int startPos, int endPos)
    {
        int first = GetFirst(startPos, endPos);
        int length = Math.Abs(startPos - endPos);
        text = text.Remove(first, length);
        caretPosition = first;
    }

    void SetCaretPosition()
    {
        caretPosition = caretPositionOnDeselect;
    }

    bool IsPointerOverObjectWithTag(string tag)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        EventSystem.current.RaycastAll(eventData, rayResults);

        foreach (RaycastResult result in rayResults)
        {
            if (result.gameObject.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    void StoreAndReactivate()
    {
        UpdateLabel();
        ActivateInputField();
        SetCaretVisible();
        caretPositionOnDeselect = caretPosition;
        selectStartPos = selectionAnchorPosition;
        selectEndPos = selectionFocusPosition;
    }

    protected override void OnFocus()
    {
        SetCaretPosition();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (IsPointerOverObjectWithTag(keyboardTag))
        {
            caretPositionOnDeselect = caretPosition;
            selectStartPos = selectionAnchorPosition;
            selectEndPos = selectionFocusPosition;
        }
        else
        {
            caretPositionOnDeselect = text.Length;
            selectStartPos = text.Length;
            selectEndPos = text.Length;
        }
        base.OnDeselect(null);
    }

    ///////////////////////////////////// virtual button functionality /////////////////////////////////////
    public void VirtualInputKey(string key)
    {
        SetCaretPosition();
        Event keyboardEvent = Event.KeyboardEvent(key);
        keyboardEvent.character = char.ToUpper(keyboardEvent.character);

        if (IsTextSelected(selectStartPos, selectEndPos))
        {
            DeleteSelected(selectStartPos, selectEndPos);
        }

        ProcessEvent(keyboardEvent);
        StoreAndReactivate();
    }

    public void VirtualBackspace()
    {
        SetCaretPosition();

        if (!IsTextSelected(selectStartPos, selectEndPos))
        {
            Backspace();
            caretPosition = caretPositionOnDeselect - 1;
        }
        else
        {
            DeleteSelected(selectStartPos, selectEndPos);
        }
        StoreAndReactivate();
    }

    public void VirtualLeft()
    {
        SetCaretPosition();

        if (IsTextSelected(selectStartPos, selectEndPos))
        {
            caretPosition = GetFirst(selectStartPos, selectEndPos);
        }
        else
        {
            caretPosition = caretPositionOnDeselect - 1;
        }
        StoreAndReactivate();
    }

    public void VirtualRight()
    {
        SetCaretPosition();
        if (IsTextSelected(selectStartPos, selectEndPos))
        {
            caretPosition = GetLast(selectStartPos, selectEndPos);
        }
        else
        {
            caretPosition += 1;
        }
        StoreAndReactivate();
    }
}