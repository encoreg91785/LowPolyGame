using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class JoysTick : ScrollRect
{
    private float mRadius;
    public Action<RectTransform> JoystickBeginHandle;
    public Action<Vector3,float> JoystickMoveHandle;
    public Action<RectTransform> JoystickEndHandle;

    /// <summary>
    /// 是否自動隱藏搖桿
    /// </summary>

    protected override void Start()
    {
        mRadius = GetComponent<RectTransform>().sizeDelta.x * 0.5f;
        content.sizeDelta = ((RectTransform)transform).sizeDelta * 1.6f;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (JoystickBeginHandle != null)
        {
            JoystickBeginHandle(content);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        
        //限制距離
        var contentPostion = content.anchoredPosition;
        if (contentPostion.magnitude > mRadius)
        {
            contentPostion = contentPostion.normalized * mRadius;
            SetContentAnchoredPosition(contentPostion);
        }

        //旋转
        if (content.anchoredPosition.y != 0)
        {
            content.eulerAngles = new Vector3(0, 0, Vector3.Angle(Vector3.right, content.anchoredPosition) * content.anchoredPosition.y / Mathf.Abs(content.anchoredPosition.y) - 90);
        }

        if (JoystickMoveHandle != null)
        {
            Vector3 v = new Vector3(content.anchoredPosition.x, 0, content.anchoredPosition.y);
            float f = contentPostion.magnitude / mRadius;
            JoystickMoveHandle(v,f);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        
        base.OnEndDrag(eventData);
        if (JoystickEndHandle != null)
        {
            JoystickEndHandle(content);
        }
        ResetContent();
    }

    void ResetContent()
    {
        content.anchoredPosition = Vector2.zero;
        content.eulerAngles = Vector3.zero;
    }
}