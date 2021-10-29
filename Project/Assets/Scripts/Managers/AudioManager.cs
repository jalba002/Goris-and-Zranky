using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _audioManager;

    public static AudioManager Instance
    {
        get => _audioManager;

        set
        {
            if (_audioManager != null)
            {
                Destroy(value);
                return;
            }

            _audioManager = value;
        }
    }

    
    public AudioMixer Mixer;

    public void SetMixerVariable(string varName, float value)
    {
        Mixer.SetFloat(varName, value);
    }

    public void GetMixerVariable(string varName, out float value)
    {
        Mixer.GetFloat(varName, out value);
    }

    private void Awake()
    {
        Instance = this;
    }
    
    
}