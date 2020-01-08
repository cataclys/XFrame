using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFrame.UI;

public class UIViewStart : UIView
{
    public Button BtnTest;
    // Start is called before the first frame update
    void Start()
    {
        int count = 0;
        BtnTest.onClick.AddListener(() =>
        {
            count++;
            //MessageBox.Show($"消息{count}");
            //MessageBox.ShowOk("中间消息", "dddddd");
        });
    }
}
