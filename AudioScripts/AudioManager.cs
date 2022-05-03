using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    //public SoundControls[] sc;
    //public List<SoundControls> sc;
    private AudioClip tempAC;

    public static AudioManager instance;

    public float sfxVolume;
    public float musicVolume;

    public Slider sfxSlider;
    public Slider musicSlider;

    [HideInInspector]
    public SoundControls scBuffer = null;

    [SerializeField] AudioSource audSource;

    [SerializeField] AudioSource musicAudSource;
    
    [SerializeField] AudioSource uiAudSource;

    [SerializeField] AudioCollection deckAudioCollection;

    [SerializeField] AudioCollection silAudioCollection;

    [SerializeField] AudioCollection vfxAudioCollection;

    [SerializeField] AudioCollection musicAudioCollection;


    void Awake()
    {
        // Makes sure there is only one audio manager in scene, if there are multiple it deletes
        // If null, creates one
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        scBuffer.name = "Buffer Card";
        //sc.Add(scBuffer);
    }

    public void ChangeMusicVolume(float param)
    {
        musicVolume = param;
    }

    public void ChangeSFXVolume(float param)
    {
        sfxVolume = param;
    }

    private void Update()
    {
        musicAudSource.volume = musicVolume;
    }

    private void Start()
    {
        musicVolume = SceneManagment.musicVolume;
        sfxVolume = SceneManagment.sfxVolume;


        if (musicSlider != null) musicSlider.value = musicVolume;
        if (sfxSlider != null) sfxSlider.value = sfxVolume;


        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManagment.currentSceneLoaded = SceneManagment.SceneLoaded.menu;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManagment.currentSceneLoaded = SceneManagment.SceneLoaded.cutscene;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManagment.currentSceneLoaded = SceneManagment.SceneLoaded.basement;
        }

        switch (SceneManagment.currentSceneLoaded)
        {
            case SceneManagment.SceneLoaded.menu:
                musicVolume = SceneManagment.musicVolume;
                sfxVolume = SceneManagment.sfxVolume;
                musicSounds(0);
                break;
            case SceneManagment.SceneLoaded.cutscene:
                //musicSounds(1);
                break;
            case SceneManagment.SceneLoaded.basement:
                musicVolume = SceneManagment.musicVolume;
                sfxVolume = SceneManagment.sfxVolume;
                musicSounds(2);
                break;
            case SceneManagment.SceneLoaded.bedroom:
                musicVolume = SceneManagment.musicVolume;
                sfxVolume = SceneManagment.sfxVolume;
                musicSounds(3);
                break;
            case SceneManagment.SceneLoaded.attic:
                musicVolume = SceneManagment.musicVolume;
                sfxVolume = SceneManagment.sfxVolume;
                musicSounds(4);
                break;
            default:
                break;
        }
    }

    public void instanceAudioManager()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    //public void PlayTorsoSound()
    //{
    //    //int randTemp = Random.Range(0, audCollection.sounds.Length);
    //    //Sounds soundItem = audCollection.sounds[0];
    //    //audSource.volume = soundItem.volume;
    //    //audSource.pitch = Random.Range(soundItem.pitchMinRange, soundItem.pitchMaxRange);
    //    //audSource.PlayOneShot(soundItem.clip);
    //}

    //public void PlayCardSound(Card card)
    //{
    //    //int randTemp = Random.Range(0, audCollection.sounds.Length);
    //    //Sounds soundItem = audCollection.sounds[1];
    //    //audSource.volume = soundItem.volume;
    //    //audSource.pitch = Random.Range(soundItem.pitchMinRange, soundItem.pitchMaxRange);
    //    //audSource.PlayOneShot(soundItem.clip);

    //}

    /// <summary>
    /// Plays audio when given the right string name
    /// Call for this anywhere with
    /// FindObjectOfType<AudioManager>().playRandPitch("CARD AUDIO NAME HERE");
    /// </summary>
    /// <param name="name"></param>
    /// 

    public IEnumerator SFXDelay(AudioClip clip, float sfxDelay)
    {
        yield return new WaitForSeconds(sfxDelay);
        audSource.PlayOneShot(clip);
    }


    // AudioManager.instance.DeckSounds(int);
    public void DeckSounds(int sfxIDX)
    {
        //Debug.Log("Deck Sounds IDX: "+sfxIDX);
        //int randTemp = Random.Range(0, audCollection.sounds.Length-1);
        Sounds soundItem = deckAudioCollection.sounds[sfxIDX];
        audSource.volume = soundItem.volume;
        audSource.volume = audSource.volume * (sfxVolume/1);
        //audSource.pitch = Random.Range(soundItem.pitchMinRange, soundItem.pitchMaxRange);
        audSource.PlayOneShot(soundItem.clip);
    }

    public void musicSounds(int sfxIDX)
    {
        Sounds soundItem = musicAudioCollection.sounds[sfxIDX];
        musicAudSource.volume = soundItem.volume;
        musicAudSource.volume = musicAudSource.volume * (musicVolume / 1);
        musicAudSource.loop = musicAudioCollection.sounds[sfxIDX].loop;
        //audSource.pitch = Random.Range(soundItem.pitchMinRange, soundItem.pitchMaxRange);
        musicAudSource.clip = soundItem.clip;
        musicAudSource.Play();
    }

    public void VFXSounds(SoundControls sc)
    {
        if (sc.clip != null)
        {
            audSource.volume = sc.volume;
            audSource.volume = audSource.volume * (sfxVolume / 1);
            //audSource.pitch = Random.Range(sc.pitchMinRange, sc.pitchMaxRange);
            audSource.PlayOneShot(sc.clip);
            //StartCoroutine(SFXDelay(sc.clip, sc.sfxDelay));
        }

        //Sounds soundItem = vfxAudioCollection.sounds[sfxIDX];
        //audSource.volume = soundItem.volume;
        //audSource.PlayOneShot(soundItem.clip);
        //if (sc.clip != null)
        //{
        //    //audSource.volume = sc.volume;
        //    //audSource.pitch = Random.Range(sc.pitchMinRange, sc.pitchMaxRange);
        //    //StartCoroutine(SFXDelay(sc.clip, sc.sfxDelay));
        //    //audSource.PlayOneShot(sc.clip);
        //}
        //else
        //{
        //    //Debug.Log("Sound Controls are Null in VFXSounds Audio Manager");
        //}
    }

    public void TorsoSounds(SoundControls[] sc)
    {
        if (sc != null)
        {
            int randTemp = Random.Range(0, sc.Length-1);
            audSource.volume = sc[randTemp].volume;
            audSource.volume = audSource.volume * (sfxVolume / 1);
            //audSource.pitch = Random.Range(sc[randTemp].pitchMinRange, sc[randTemp].pitchMaxRange);
            StartCoroutine(SFXDelay(sc[randTemp].clip, sc[randTemp].sfxDelay));
            //audSource.PlayOneShot(sc[randTemp].clip);
        }
        else
        {
            //Debug.Log("Sound Controls are Null in VFXSounds Audio Manager");
        }
    }

    public void CardSounds(Card card)
    {
        int randTemp = Random.Range(0, card.attackSounds.Length-1);
        SoundControls cardSC = card.attackSounds[randTemp];
        audSource.volume = cardSC.volume;
        audSource.volume = audSource.volume * (sfxVolume / 1);
        //audSource.pitch = Random.Range(cardSC.pitchMinRange, cardSC.pitchMaxRange);
        StartCoroutine(SFXDelay(cardSC.clip, cardSC.sfxDelay));
        //audSource.PlayOneShot(cardSC.clip);
        //Debug.Log("UI Sounds Name " + cardSC.name);
        //Debug.Log("UI Sounds Clip" + cardSC.clip);
    }

    //This is for 2D UI elements
    public void UISounds(SoundControls sc)
    {
        if (sc != null)
        {
            audSource.volume = sc.volume;
            audSource.volume = audSource.volume * (sfxVolume / 1);
            audSource.pitch = Random.Range(sc.pitchMinRange, sc.pitchMaxRange);
            StartCoroutine(SFXDelay(sc.clip, sc.sfxDelay));
            SceneManagment.musicVolume = musicVolume;
            SceneManagment.sfxVolume = sfxVolume;
            //audSource.PlayOneShot(sc.clip);
            //Debug.Log("UI Sounds Name " + sc.name);
            //Debug.Log("UI Sounds Clip" + sc.clip);
        }
        else
        {
            //Debug.Log("Sound Controls are Null in UISounds Audio Manager");
        }

        //int randTemp = Random.Range(0, audCollection.sounds.Length-1);


    }

    public void SilhouetteSounds(int sfxIDX)
    {
        //Debug.Log("Silhouette Sounds IDX: " + sfxIDX);
        //int randTemp = Random.Range(0, audCollection.sounds.Length-1);
        Sounds soundItem = silAudioCollection.sounds[sfxIDX];
        uiAudSource.volume = soundItem.volume;
        uiAudSource.volume = uiAudSource.volume * (sfxVolume / 1);
        uiAudSource.pitch = Random.Range(soundItem.pitchMinRange, soundItem.pitchMaxRange);
        uiAudSource.PlayOneShot(soundItem.clip);
        //StartCoroutine(SFXDelay(soundItem.clip, soundItem.sfxDelay));
    }

    //public void playRandPitch(String udyhfgklj)
    //{
    //    //int randTemp = Random.Range(0, audCollection.sounds.Length);
    //    //Sounds soundItem = audCollection.sounds[2];
    //    //audSource.volume = soundItem.volume;
    //    //audSource.pitch = Random.Range(soundItem.pitchMinRange, soundItem.pitchMaxRange);
    //    //audSource.PlayOneShot(soundItem.clip);
    //}
    //public void PlayUISound(SoundControls uiSC)
    //{
    //    if (uiSC != null)
    //    {
    //        // Destroys audio source from previous audio clip
    //        Destroy(sc[sc.Count - 1].source);
    //        // Removes previous sound from SC list
    //        sc.RemoveAt(sc.Count - 1);
    //        foreach (SoundControls s in sc)
    //        {
    //            // If the Audio Source is "destroyed"
    //            if (s.source.enabled == false)
    //            {
    //                s.source = gameObject.AddComponent<AudioSource>();
    //                // Assigns values for Audio Source
    //                s.source.clip = s.clip;
    //                s.source.volume = s.volume;
    //                s.source.pitch = 1;
    //            }

    //        }
    //        sc.Add(uiSC);

    //        foreach (SoundControls s in sc)
    //        {
    //            // Sometimes s.source is null, but also sometimes s.source isn't null, but s.source.enabled still = false?
    //            // I plan to fix this later, but it prevents the bug of audio sources getting deleted before they can play.
    //            if (s.source == null)
    //            {
    //                s.source = gameObject.AddComponent<AudioSource>();
    //            }
    //            if (s.source.enabled == false)
    //            {
    //                s.source = gameObject.AddComponent<AudioSource>();

    //            }
    //            // Assigns values for Audio Source
    //            s.source.clip = s.clip;
    //            s.source.volume = s.volume;
    //            s.source.pitch = 1;
    //        }
    //        playRandPitch(uiSC.name);
    //        //sc[sc.Count - 1].source.name = card.cardName;
    //    }
    //}
}
