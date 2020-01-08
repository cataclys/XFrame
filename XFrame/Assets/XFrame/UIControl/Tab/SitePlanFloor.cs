//using System.Collections;
//using System.Collections.Generic;
//using UniRx;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class SitePlanFloor : Floor
//{
//    public SitePlan Data;
//    private void Start()
//    {
//        tgeSelect.OnValueChangedAsObservable().Subscribe(b =>
//        {
//            if (b)
//            {
//                WorkingArea.Instance.GetSitePlanData(Data);
//                GameManager.CurrentFloorId = Data.Id;
//            }
//        }).AddTo(gameObject);

//        inputName.OnEndEditAsObservable().Subscribe(x =>
//        {
//            inputName.gameObject.SetActive(false);
//            if (!string.IsNullOrEmpty(x))
//            {
//                Data.Name = x;
//                HttpManager.Instance.Put<SitePlan>(WebAPI.EditorSitePlans + Data.Id, Data, () =>
//                {
//                    MessageBox.Show("修改名称成功"); 
//                    Name.Value = x;
//                }, error => { MessageBox.Show(error); });
//            }
//        }).AddTo(gameObject);
//    }

//    public void SetData(SitePlan sitePlanInfo)
//    {
//        Data = sitePlanInfo;
//        Name.Value = Data.Name;
//    }
//}
