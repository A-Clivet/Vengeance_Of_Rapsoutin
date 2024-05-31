using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

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

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
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
        InitializeMusic(FMODEvents.instance.music);
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

    public void PlayOneShot(EventReference p_sound, Vector3 p_worldPos)
    {
        RuntimeManager.PlayOneShot(p_sound, p_worldPos);
    }

    public EventInstance CreateInstance(EventReference p_eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(p_eventReference);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference p_eventReference, GameObject p_emitterGameObject)
    {
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
