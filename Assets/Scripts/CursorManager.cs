using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorState
{
    None,
    Normal,
    CheckEnemy,
    Attack,
    Dash,
    Move
}

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    public RectTransform cursorIndicator;
    
    private Dictionary<CursorState, GameObject> cursorDic;
    private GameObject currentCursor;

    private void Awake()
    {
        Instance = this;

        cursorDic = new Dictionary<CursorState, GameObject>();

        foreach (CursorState cursor in Enum.GetValues(typeof(CursorState)))
        {
            cursorDic.Add(cursor, cursorIndicator.Find(cursor.ToString()).gameObject);
        }

        foreach (GameObject cursor in cursorDic.Values)
        {
            cursor.SetActive(false);
        }

        ChangeCursor(CursorState.Normal);
    }

    private void Update()
    {
        cursorIndicator.position = Input.mousePosition;
    }

    public void ChangeCursor(CursorState cursor)
    {
        if (currentCursor != null)
        {
            currentCursor.SetActive(false);
        }

        Cursor.visible = cursor == CursorState.None;

        cursorDic[cursor].SetActive(true);
        currentCursor = cursorDic[cursor];
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = focus ? CursorLockMode.Confined : CursorLockMode.None;
    }
}
