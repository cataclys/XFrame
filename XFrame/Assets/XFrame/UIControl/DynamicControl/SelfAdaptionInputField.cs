using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class SelfAdaptionInputField : DynamicControl, ILayoutElement
{
    private Text textComponent
    {
        get
        {
            return this.GetComponent<InputField>().textComponent;
        }
    }
    private string text
    {
        get
        {
            return this.GetComponent<InputField>().text;
        }
    }
    private TextGenerator _generatorForLayout;
    private RectTransform rectTransform
    {
        get
        {
            if (m_Rect == null)
                m_Rect = GetComponent<RectTransform>();
            return m_Rect;
        }
    }
    private Vector2 originalSize;
    private InputField _inputField;
    public Text _lable;
    private RectTransform m_Rect;
    private float _offsetHeight;
    private float _offsetTextComponentLeftRingt;
    private int priority = 1;

    public virtual float minWidth
    {
        get { return -1; }
    }
    public virtual float flexibleWidth { get { return -1; } }
    public virtual float minHeight
    {
        get { return -1; }
    }
    public override void SetEnabled(bool b)
    {
        base.SetEnabled(b);
        inputField.interactable = b;
    }
    public virtual float preferredHeight
    {
        get
        {
            if (fixedWidth)
            {
                return generatorForLayout.GetPreferredHeight(text, GetTextGenerationSettings(new Vector2(this.textComponent.GetPixelAdjustedRect().size.x, 0.0f))) / textComponent.pixelsPerUnit + offsetHeight;
            }
            else
            {
                return generatorForLayout.GetPreferredHeight(text, GetTextGenerationSettings(new Vector2(this.textComponent.GetPixelAdjustedRect().size.x, 0.0f))) / textComponent.pixelsPerUnit + offsetHeight;
            }

        }
    }
    public virtual float flexibleHeight { get { return -1; } }
    public int fontSize = 16;
    public bool fixedWidth = true;
    public bool keepInitWidthSize = true;
    public virtual int layoutPriority { get { return priority; } }
    public TextGenerator generatorForLayout
    {
        get
        {
            return _generatorForLayout ?? (_generatorForLayout = new TextGenerator());
        }
    }
    public InputField inputField
    {
        get
        {
            return _inputField ?? (_inputField = this.GetComponent<InputField>());
        }
    }
    public Text lable
    {
        get
        {
            return _lable ?? (_lable = transform.Find("Lable").GetComponent<Text>());
        }
    }
    public float offsetHeight
    {
        get
        {
            if (_offsetHeight == 0)
                _offsetHeight = generatorForLayout.GetPreferredHeight(text, GetTextGenerationSettings(Vector2.zero)) / textComponent.pixelsPerUnit;
            return _offsetHeight;
        }
    }
    public float preferredWidth
    {
        get
        {
            if (fixedWidth)
            {
                return this.originalSize.x;
            }
            else
            {
                if (keepInitWidthSize)
                {
                    return Mathf.Max(this.originalSize.x, generatorForLayout.GetPreferredWidth(text, GetTextGenerationSettings(Vector2.zero)) / textComponent.pixelsPerUnit + offsetTextComponentLeftRingt);
                }
                else
                {
                    return generatorForLayout.GetPreferredWidth(text, GetTextGenerationSettings(Vector2.zero)) / textComponent.pixelsPerUnit + offsetTextComponentLeftRingt;
                }
            }
        }
    }
    public float offsetTextComponentLeftRingt
    {
        get
        {
            if (_offsetTextComponentLeftRingt == 0)
                _offsetTextComponentLeftRingt = Mathf.Abs(rectTransform.rect.xMin - textComponent.rectTransform.rect.xMin) + Mathf.Abs(rectTransform.rect.xMax - textComponent.rectTransform.rect.xMax);
            return _offsetTextComponentLeftRingt;
        }
    }

    protected void Awake()
    {
        textComponent.fontSize = fontSize;
        inputField.placeholder.GetComponent<Text>().fontSize = fontSize;
        this.originalSize = this.GetComponent<RectTransform>().sizeDelta;
        inputField.lineType = fixedWidth ? InputField.LineType.MultiLineNewline : InputField.LineType.SingleLine;
        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)1, LayoutUtility.GetPreferredHeight(m_Rect));
    }

    public TextGenerationSettings GetTextGenerationSettings(Vector2 extents)
    {
        var settings = textComponent.GetGenerationSettings(extents);
        settings.generateOutOfBounds = true;
        return settings;
    }
    public void OnValueChanged(string v)
    {
        if (!fixedWidth)
        {
            rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)0, LayoutUtility.GetPreferredWidth(m_Rect));
        }
        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)1, LayoutUtility.GetPreferredHeight(m_Rect));
    }

    void OnEnable()
    {
        this.inputField.onValueChanged.AddListener(OnValueChanged);
    }

    void OnDisable()
    {
        this.inputField.onValueChanged.RemoveListener(OnValueChanged);
    }
    public void Update()
    {

    }
    public virtual void CalculateLayoutInputHorizontal()
    {
    }

    public virtual void CalculateLayoutInputVertical()
    {
    }

}
