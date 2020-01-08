//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UniRx;

//public class SitePlanTab : MonoBehaviour
//{
//    public Text txtName;
//    public Toggle btnTab;

//    private List<SitePlan> Data;

//    private void Awake()
//    {
//        btnTab.group = transform.parent.GetComponent<ToggleGroup>();
//    }
//    private void Start()
//    {
//        txtName.text = "建筑总平面图";
//        btnTab.OnValueChangedAsObservable()
//            .Subscribe(b =>
//            {
//                if (b)
//                {
//                    MessageSystem.Broadcast<List<SitePlan>>(Msg.RefreshSitePlans, Data);
//                    GameManager.CurrentBuildingId = "-1";
//                }
//            }).AddTo(gameObject);
//    }
//    public void Init(List<SitePlan> sitePlanInfos)
//    {
//        Data = sitePlanInfos;
//        btnTab.isOn = true;
//    }


//}
