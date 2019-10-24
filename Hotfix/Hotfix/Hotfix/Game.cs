using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameTest : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.ShowView<UIViewLoading>();
    }
}

