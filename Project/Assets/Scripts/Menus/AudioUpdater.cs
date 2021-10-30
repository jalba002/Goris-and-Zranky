using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioUpdater : MonoBehaviour
{
    public AudioMixer m_Mixer;
    public string variableName;
    
    private Slider _slider;
    public TextMeshProUGUI valueDisplay;
    
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(UpdateVisualValue);
    }

    public void SetLevel(float sliderValue)
    {
        m_Mixer.SetFloat(variableName, CalculateLogarithmicValue(sliderValue));
    }

    float CalculateLogarithmicValue(float value)
    {
        return Mathf.Log10(value) * 20f;
    }

    private void UpdateVisualValue(float value)
    {
        if (valueDisplay == null) return;
        valueDisplay.text = ((int)(value*100)).ToString(CultureInfo.CurrentUICulture);
    }

    private void OnEnable()
    {
        m_Mixer.GetFloat(variableName, out float value); 
        UpdateSlidersAndValues(value);
    }

    private void UpdateSlidersAndValues(float value)
    {
        float transformedValue = Mathf.Pow(10, value/20f);
        UpdateVisualValue(transformedValue);
        _slider.value = transformedValue;
    }
}
