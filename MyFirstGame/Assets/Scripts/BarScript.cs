using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    [SerializeField]
    private float fillAmount;
    [SerializeField]
    private Image content;
    [SerializeField]
    private float lerpSpeed;
    [SerializeField]
    private Color fullColor;
    [SerializeField]
    private Color lowColor;
    public float maxHeath;
    public float value
    {
        set
        {
            fillAmount = Map(value, 0, maxHeath, 0, 1);
        }
    }
    void Start()
    {
        content.color = fullColor;
    }

    // Update is called once per frame
    void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        if (content.fillAmount != fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);            
        }
        content.color = Color.Lerp(lowColor, fullColor, fillAmount);
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //(80-0)*(1-0)/(120-0)+0 = 80/120
    }

    public void Refresh()
    {
        this.value = maxHeath;
        content.fillAmount = 1;
    }
}
