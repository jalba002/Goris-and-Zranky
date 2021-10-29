using UnityEngine;

public class PlayerPrefsLoader : MonoBehaviour
{
    private PlayerPrefsLoader _playerPreferences;

    public PlayerPrefsLoader Instance
    {
        get => _playerPreferences;
        set
        {
            if (_playerPreferences != null)
            {
                Destroy(this);
                return;
            }

            _playerPreferences = value;
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadAudioPreferences();
    }

    public void SaveAudioPreferences()
    {
        Debug.Log("Saving prefs");
        StoreFloat("MasterVol");
        StoreFloat("AmbientVol");
        StoreFloat("MusicVol");
        StoreFloat("SoundsVol");
        PlayerPrefs.Save();
    }

    private void StoreFloat(string variableName)
    {
        AudioManager.Instance.GetMixerVariable(variableName, out float variable);
        PlayerPrefs.SetFloat(variableName, variable);
    }

    private float RecoverFloat(string variableName)
    {
        return PlayerPrefs.GetFloat(variableName);
    }

    public void LoadAudioPreferences()
    {
        Debug.Log("Loading prefs");
        AudioManager.Instance.SetMixerVariable("MasterVol", RecoverFloat("MasterVol"));
        AudioManager.Instance.SetMixerVariable("AmbientVol", RecoverFloat("AmbientVol"));
        AudioManager.Instance.SetMixerVariable("MusicVol", RecoverFloat("MusicVol"));
        AudioManager.Instance.SetMixerVariable("SoundsVol", RecoverFloat("SoundsVol"));
    }

    public void SaveClothingPreferences()
    {
        Debug.Log("Saving clothing preferences");
        
    }

    public void LoadClothingPreferences()
    {
        Debug.Log("Loading clothing preferences");
        
    }
}