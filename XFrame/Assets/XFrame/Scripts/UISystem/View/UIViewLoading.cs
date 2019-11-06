using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFrame.UI;

public class UIViewLoading : UIView
{
    private int Number = 0;
    public override void Show()
    {
        base.Show();
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
