using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ConfigReader.Load();
        ConfigReader.Save();
        //UIManager.Instance.ShowView<UIViewLoading>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
