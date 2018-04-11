using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager {

    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource ambientSound;
    [SerializeField] private AudioSource ambientSound2;
    [SerializeField] private string ambientSoundName;
    [SerializeField] private string ambientSoundName2;

    private AudioSource _activeMusic;
    private AudioSource _inactiveMusic;

    public float crossFadeRate = 1.5f;
    private bool _corssFading;
    private float _musicVolume;

    public ManagerStatus status { get; private set; }

    public float musicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            _musicVolume = value;
            if (ambientSound != null && !_corssFading)
            {
                ambientSound.volume = _musicVolume;
                ambientSound2.volume = _musicVolume;
            }
        }
    }
    public bool musicMute
    {
        get
        {
            if (ambientSound != null)
            {
                return ambientSound.mute;
            }
            return false;
        }
        set
        {
            if (ambientSound != null)
            {
                ambientSound.mute = value;
                ambientSound2.mute = value;
            }
        }
    }



    public void PlayLevelMusic()
    {
        //PlayMusic(Resources.Load("Music/" + ambientSoundName) as AudioClip);
        PlayMusic(Resources.Load("Music/" + ambientSoundName2) as AudioClip);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (_corssFading)
        {
            return;
        }
        StartCoroutine(CrossFadeMusic(clip));
//        ambientSound.clip = clip;
//        ambientSound.Play();
    }

    private IEnumerator CrossFadeMusic(AudioClip clip)
    {
        _corssFading = true;

        _inactiveMusic.clip = clip;
        _inactiveMusic.volume = 0;
        _inactiveMusic.Play();

        float scaledRate = crossFadeRate * _musicVolume;
        while (_activeMusic.volume > 0)
        {
            _activeMusic.volume -= scaledRate * Time.deltaTime;
            _inactiveMusic.volume += scaledRate * Time.deltaTime;
            yield return null;
        }

        AudioSource temp = _activeMusic;

        _activeMusic = _inactiveMusic;
        _activeMusic.volume = _musicVolume;

        _inactiveMusic = temp;
        _inactiveMusic.Stop();

        _corssFading = false; 
    }

    public void StopMusic()
    {
        _activeMusic.Stop();
        _inactiveMusic.Stop();
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }

    public void Startup()
    {
        Debug.Log("Audio manager starting...");
        musicVolume = 1;
        soundVolume = 1;

        ambientSound.ignoreListenerVolume = true;
        ambientSound.ignoreListenerPause = true;
        ambientSound2.ignoreListenerVolume = true;
        ambientSound2.ignoreListenerPause = true;

        _activeMusic = ambientSound;
        _inactiveMusic = ambientSound2;

        //_activeMusic.Play();
        //_inactiveMusic.Stop();
        
        status = ManagerStatus.Started;
    }

    private IEnumerator startCrossFade()
    {
        yield return new WaitForSeconds(3);
        PlayLevelMusic();
    }

    public float soundVolume
    {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }

    public bool soundMute
    {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
