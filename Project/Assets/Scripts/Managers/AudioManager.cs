using System.Collections;
using System.Collections.Generic;
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
    public AudioMixerGroup group;
    private IEnumerator soundRemover;

    [System.Serializable]
    public struct TemporalAudio
    {
        public GameObject go;
        public AudioSource Source;
        public float destructionTime;

        public TemporalAudio(GameObject position, AudioSource source, float duration)
        {
            this.go = position;
            this.Source = source;
            this.destructionTime = Time.timeSinceLevelLoad + duration;
        }
    }

    public List<TemporalAudio> audios = new List<TemporalAudio>();

    private void Awake()
    {
        Instance = this;
        //this.transform.parent = null;
        //DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        //if (soundRemover == null) return;
        soundRemover = checkAudios();
        StartCoroutine(soundRemover);
    }
   
    public void SetMixerVariable(string varName, float value)
    {
        Mixer.SetFloat(varName, value);
    }

    public void GetMixerVariable(string varName, out float value)
    {
        Mixer.GetFloat(varName, out value);
    }

    public void PlaySoundAt(Vector3 pos, AudioClip clip)
    {
        if (clip == null) return;
        
        var item = new GameObject("[TempAudio]");
        item.transform.position = pos;
        var casio = item.AddComponent<AudioSource>();
        casio.clip = clip;
        casio.outputAudioMixerGroup = group;
        casio.spatialBlend = 1f;
        casio.Play();
        TemporalAudio temp = new TemporalAudio(item, casio, clip.length);
        audios.Add(temp);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        
        var item = new GameObject("[TempAudio]");
        var casio = item.AddComponent<AudioSource>();
        casio.clip = clip;
        casio.outputAudioMixerGroup = group;
        casio.spatialBlend = 0f;
        casio.Play();
        TemporalAudio temp = new TemporalAudio(item, casio, clip.length);
        audios.Add(temp);
    }

    IEnumerator checkAudios()
    {
        while (enabled)
        {
            List<TemporalAudio> audiosToRemove = new List<TemporalAudio>();
            foreach (var item in audios)
            {
                if (item.destructionTime <= Time.timeSinceLevelLoad)
                {
                    Destroy(item.go);
                    audiosToRemove.Add(item);
                    //audios.Remove(item);
                }
            }

            foreach (var VARIABLE in audiosToRemove)
            {
                audios.Remove(VARIABLE);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}