//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UniRx;

//public class BuildingTab : MonoBehaviour
//{
//    public Toggle Tab;
//    public Text Name;
//    private Building Data;

//    private void Awake()
//    {
//        Tab.group = transform.parent.GetComponent<ToggleGroup>();
//    }
//    private void Start()
//    {
//        Tab.OnValueChangedAsObservable().Subscribe(b =>
//        {
//            if (b)
//            {
//                MessageSystem.Broadcast<Building>(Msg.RefreshBuildingFloor, Data);
//                GameManager.CurrentBuildingId = Data.Id;
//            }
//        });
//    }

//    public void Init(Building building)
//    {
//        Data = building;
//        Name.text = Data.Name;
//    }

//}
