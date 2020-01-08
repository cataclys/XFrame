using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

public class Floor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Toggle tgeSelect;
    public Text txtName;
    public InputField inputName;
    public Button btnEditor;

    public StringReactiveProperty Name;

    private void Awake()
    {
        tgeSelect.group = transform.parent.GetComponent<ToggleGroup>();
        Name.SubscribeToText(txtName).AddTo(gameObject);
        Name.Subscribe(x => { inputName.text = x; }).AddTo(gameObject);
        btnEditor.OnClickAsObservable().Subscribe(x => { inputName.gameObject.SetActive(true); inputName.ActivateInputField(); }).AddTo(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        btnEditor.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btnEditor.gameObject.SetActive(false);
    }
}
