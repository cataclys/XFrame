using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRoll : MonoBehaviour
{
    void Update()
    {
        float delX = Input.mousePosition.x - transform.position.x;
        float delY = Input.mousePosition.y - transform.position.y;

        float scaleX = delX / GetComponent<RectTransform>().rect.width / transform.localScale.x;
        float scaleY = delY / GetComponent<RectTransform>().rect.height / transform.localScale.y;


        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (transform.localScale.x >= 10f)
            {
                return;
            }

            transform.localScale += Vector3.one * 0.1f;
            //float nowx = GetComponent<RectTransform>().pivot.x;
            //float nowy = GetComponent<RectTransform>().pivot.y;
            //float targetposx = Mathf.Clamp(nowx+ scaleX,0,1);
            //float targetposy = Mathf.Clamp(nowy + scaleY, 0, 1);
            //GetComponent<RectTransform>().pivot = new Vector2(targetposx, targetposy);//+= new Vector2(scaleX, scaleY);

            GetComponent<RectTransform>().pivot += new Vector2(scaleX, scaleY);
            transform.position += new Vector3(delX, delY, 0);
            // Debug.Log(transform.position + "--------" + transform.parent.position);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (transform.localScale.x <= 0.15f)
            {
                return;
            }
            transform.localScale += Vector3.one * -0.1f;
            //float nowx = GetComponent<RectTransform>().pivot.x;
            //float nowy = GetComponent<RectTransform>().pivot.y;
            //float targetposx = Mathf.Clamp(nowx + scaleX, 0, 1);
            //float targetposy = Mathf.Clamp(nowy + scaleY, 0, 1);
            //GetComponent<RectTransform>().pivot = new Vector2(targetposx, targetposy);
            GetComponent<RectTransform>().pivot += new Vector2(scaleX, scaleY);
            transform.position += new Vector3(delX, delY, 0);
        }

        #region
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    rect.pivot += Vector2.one * 0.1f;
        //    Debug.Log(transform.position);
        //}
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    rect.pivot += Vector2.one * -0.1f;
        //}
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log(transform.position);
        //    Debug.Log(rect.sizeDelta.x * rect.localScale.x);
        //}
        //if (Input.GetAxis("Mouse ScrollWheel") != 0)
        //{
        //    float scalevalue = Input.GetAxis("Mouse ScrollWheel");
        //    Vector2 mousepos = GetMouseScreenPos();
        //    Vector2 povitoffset = GetPivotPos(mousepos);

        //    rect.localScale += Vector3.one * scalevalue;

        //}
        //else
        //{
        //    //if (rect.pivot.x!=0.5||rect.pivot.y!=0.5)
        //    //{
        //    //   rect.pivot = new Vector2(0.5f, 0.5f);
        //    //}
        //}
        #endregion
    }
    #region
    //private Vector2 GetMouseScreenPos()
    //{
    //    Vector2 uisize = canvas.GetComponent<RectTransform>().sizeDelta;//得到画布的尺寸
    //    Vector2 screenpos = Input.mousePosition;
    //    Vector2 screenpos2;
    //    screenpos2.x = screenpos.x;// - (Screen.width / 2);//转换为以屏幕中心为原点的屏幕坐标
    //    screenpos2.y = screenpos.y;//- (Screen.height / 2);
    //    Vector2 uipos3;//UI坐标
    //    uipos3.x = screenpos2.x * (uisize.x / Screen.width);//转换后的屏幕坐标*画布与屏幕宽高比
    //    uipos3.y = screenpos2.y * (uisize.y / Screen.height);
    //    return uipos3;

    //}
    //private Vector2 GetPivotPos(Vector2 mousepos)
    //{
    //    Vector2 offset = Vector2.zero ;
    //    float offx = (mousepos.x - transform.position.x) / (rect.sizeDelta.x * rect.localScale.x);
    //    float offy = (mousepos.y - transform.position.y) / (rect.sizeDelta.y * rect.localScale.y);
    //    offset = new Vector2(0.5f+offx,0.5f+offy);
    //    rect.pivot = offset;
    //    return offset;
    //}
    #endregion
}
