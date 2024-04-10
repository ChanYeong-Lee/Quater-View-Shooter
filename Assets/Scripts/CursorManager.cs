using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorState
{
    None,
    Attack
}

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    public RectTransform cursorIndicator;
    public bool focus;

    private Dictionary<CursorState, GameObject> cursorDic;
    private GameObject currentCursor;

    private void Awake()
    {
        Instance = this;
    
        cursorDic = new Dictionary<CursorState, GameObject>()
        {
            { CursorState.None, cursorIndicator.Find("None").gameObject },
            { CursorState.Attack, cursorIndicator.Find("Attack").gameObject }
        };

        foreach (GameObject cursor in cursorDic.Values)
        {
            cursor.SetActive(false);
        }

        ChangeCursor(CursorState.None);
    }

    private void Update()
    {
        if (focus)
        {
            cursorIndicator.position = Input.mousePosition;
        }
    }

    public void ChangeCursor(CursorState cursor)
    {
        if (currentCursor != null)
        {
            currentCursor.SetActive(false);
        }

        cursorDic[cursor].SetActive(true);
        currentCursor = cursorDic[cursor];
    }

    private void OnApplicationFocus(bool focus)
    {
        this.focus = focus;

        //Cursor.visible = !focus;
        Cursor.lockState = focus ? CursorLockMode.Confined : CursorLockMode.None;
    }
}
