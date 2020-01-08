using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tabs : MonoBehaviour
{
    public List<string> TabsName;
    public Transform TabNameParent;
    public Transform TabContentParent;
    public GameObject TabNameItem;
    public GameObject TabContentItem;

    IEnumerator Start()
    {
        foreach (var item in TabsName)
        {
            yield return new WaitForSeconds(0.2f);
            CreateTab(item);
            yield return new WaitForEndOfFrame();
        }
    }

    private void CreateTab(string item)
    {
        Tab tab = GameObject.Instantiate(TabNameItem, TabNameParent).GetComponent<Tab>();
        tab.TabName.text = item;
        GameObject content = GameObject.Instantiate(TabContentItem, TabContentParent);
        tab.Content = content;
        tab.gameObject.SetActive(true);
        content.SetActive(true);
    }
}
