using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFrame.UI;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.Show<UIViewStart>();
        UIManager.Instance.Show<UIViewStart>("页面1");
        UIManager.Instance.Show<UIViewStart>("页面2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
