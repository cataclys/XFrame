//using System.Collections;
//using System.Collections.Generic;
//using UniRx;
//using UnityEngine;

//public class BuildingFloor : Floor
//{
//    public BuildingArea Data;
//    private void Start()
//    {
//        tgeSelect.OnValueChangedAsObservable().Subscribe(b =>
//        {
//            if (b)
//            {
//                WorkingArea.Instance.GetBuildingAreaData(Data);
//                GameManager.CurrentFloorId = Data.Id;
//            }
//        }).AddTo(gameObject);

//        inputName.OnEndEditAsObservable().Subscribe(x =>
//        {
//            inputName.gameObject.SetActive(false);
//            if (!string.IsNullOrEmpty(x))
//            {
//                Data.Name = x;
//                HttpManager.Instance.Put<BuildingArea>(WebAPI.UpdateBuildingAreas + Data.Id, Data, () =>
//                {
//                    MessageBox.Show("修改名称成功");
//                    Name.Value = x;
//                }, error => { MessageBox.Show(error); });
//            }
//        }).AddTo(gameObject);
//    }

//    public void SetData(BuildingArea buildingArea)
//    {
//        Data = buildingArea;
//        Name.Value = Data.Name;
//    }
//}
