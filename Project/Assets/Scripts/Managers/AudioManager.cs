using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, IUpdateOnSceneLoad
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

    private void Awake()
    {
        Instance = this;
    }

    public void SetMixerVariable(string varName, float value)
    {
        Mixer.SetFloat(varName, value);
    }

    public void GetMixerVariable(string varName, out float value)
    {
        Mixer.GetFloat(varName, out value);
    }


    public void UpdateOnSceneLoad()
    {
        // Stop all audios when scene loads.
    }
}