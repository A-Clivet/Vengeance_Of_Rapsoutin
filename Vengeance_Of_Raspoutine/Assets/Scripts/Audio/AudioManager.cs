using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float SFXVolume = 1;

    private Bus _masterBus;
    private Bus _musicBus;
    private Bus _sfxBus;

    private List<EventInstance> _eventInstances;
    private List<StudioEventEmitter> _eventEmitters;

    private EventInstance _musicEventInstance;

    public string _sceneString = "MainMenu";
    [SerializeField] Scene _mainMenu;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        _eventInstances = new List<EventInstance>();
        _eventEmitters = new List<StudioEventEmitter>();

        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        
        if (_sceneString == SceneManager.GetActiveScene().name)
        {
            _masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            PlayOneShot(FMODEvents.instance.MainMenuMusic, Camera.main.transform.position);
        }
        else
        {
            _masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            PlayOneShot(FMODEvents.instance.MainGameMusic, Camera.main.transform.position);
        }
    }

    private void Update()
    {
        _masterBus.setVolume(masterVolume);
        _musicBus.setVolume(musicVolume);
        _sfxBus.setVolume(SFXVolume);
    }

    private void InitializeMusic(EventReference p_musicEventReference)
    {
        _musicEventInstance = CreateInstance(p_musicEventReference);
        _musicEventInstance.start();
    }

    public void SetMusic(int p_Music)
    {
        //set p_Music to 0 for the main menu music and 1 for the battle music
        _musicEventInstance.setParameterByName("Crossfade", p_Music);
    }
    public void SetAdaptativeMusic(int p_Music)
    {
        //set p_Music to 1 for the short loop, 2 for the tense loop (only in domination)
        //set p_Music to 0 for the long loop 
        _musicEventInstance.setParameterByName("AdaptativeMusic", p_Music);
    }

    public void PlayOneShot(EventReference p_sound, Vector3 p_worldPos) //always plays the full sound
    {
        //make an Instance of this function when you want it to play, give it an Instance of the event you want to play
        //give it a vector 3
        RuntimeManager.PlayOneShot(p_sound, p_worldPos);
    }

    public EventInstance CreateInstance(EventReference p_eventReference) //only plays the sound when called and stops when not
    {
        //make an Instance of this function when you want it to play
        //give it the event you want to play
        EventInstance eventInstance = RuntimeManager.CreateInstance(p_eventReference);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference p_eventReference, GameObject p_emitterGameObject)//always plays the full sound if the player is close enough
    {
        //make an Instance of this function when you want it to play
        //give it an Instance of the event you want to play
        //give it a game object for directional audio
        StudioEventEmitter emitter = p_emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = p_eventReference;
        _eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in _eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        // stop all of the event emitters, because if we don't they may hang around in other scenes
        foreach (StudioEventEmitter emitter in _eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
