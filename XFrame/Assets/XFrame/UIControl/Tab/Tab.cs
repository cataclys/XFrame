using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    private Toggle TabButton;
    public GameObject Content;
    public Text TabName;
    private void Awake()
    {
        TabButton = GetComponent<Toggle>();
    }
    // Start is called before the first frame update
    void Start()
    {
        TabButton.OnValueChangedAsObservable()
            .Subscribe(b =>{ Content.SetActive(b); })
            .AddTo(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
