using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFrame.UI;

public class UIViewLoading : UIView
{
    private int Number = 0;
    public override void Show(object data)
    {
        base.Show(data);
        Number++;
    }
    public override void Hide()
    {
        Number--;
        if (Number == 0)
        {
            base.Hide();
        }
    }
}
