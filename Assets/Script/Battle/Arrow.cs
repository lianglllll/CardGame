using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector2 StartPoint;
    private Vector2 EndPoint;
    private RectTransform arrow;

    private float ArrowLength;
    private float ArrowAngle;
    private Vector2 ArrowPosition;

    private void Start()
    {
        arrow = transform.GetComponent<RectTransform>();
    }


    private void Update()
    {
        //计算相关信息
        EndPoint = Input.mousePosition - new Vector3(960.0f, 540.0f, 0.0f);
       
        ArrowPosition = new Vector2((EndPoint.x + StartPoint.x) / 2, (EndPoint.y + StartPoint.y) / 2);
        ArrowLength = Mathf.Sqrt((EndPoint.x - StartPoint.x) * (EndPoint.x - StartPoint.x) + (EndPoint.y - StartPoint.y) * (EndPoint.y - StartPoint.y)-50);//让其长度断一点，否则鼠标点击ui的时候不会触发
        ArrowAngle = Mathf.Atan2(EndPoint.y - StartPoint.y, EndPoint.x - StartPoint.x);

        arrow.localPosition = ArrowPosition;
        arrow.sizeDelta = new Vector2(ArrowLength, arrow.sizeDelta.y);
        arrow.localEulerAngles = new Vector3(0.0f, 0.0f, ArrowAngle * 180 / Mathf.PI);
    }


    public void SetStartPoint(Vector2 _startPoint)//传进来的是世界坐标
    {
        StartPoint = _startPoint- new Vector2(960.0f, 540.0f);
    }
}
