using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [Tooltip("Music source (loops background tracks)")]
    public AudioSource musicSource;

    [Tooltip("SFX source (one-shot sound effects)")]
    public AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip level1Music;
    // Add more level music clips here as needed

    [Header("SFX Clips")]
    public AudioClip buttonForwardClick;
    public AudioClip buttonBackClick;
    public AudioClip upgradeClick;
    public AudioClip creditPickup;

    public AudioClip pedActivate;
    public AudioClip pedDeactivate;
    public AudioClip boostActivate;
    public AudioClip dashActivate;
    public AudioClip invincibilityActivate;
    public AudioClip missileFire;
    public AudioClip missileExplosion;
    // Add more SFX here

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            PlayMusic(mainMenuMusic);
        }
        else if (scene.name == "Level1")
        {
            PlayMusic(level1Music);
        }
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return; // already playing

        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // --------------------------------------------------------------- Main Function to play SFX -----------------------------------------
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    // --------------------------------------------------------------- SFX Functions -----------------------------------------------------
    public void PlayButtonForwardClick()
    {
        PlaySFX(buttonForwardClick);
    }
    public void PlayButtonBackClick()
    {
        PlaySFX(buttonBackClick);
    }
    public void PlayUpgradeSound()
    {
        PlaySFX(upgradeClick);
    }
    public void PlayCreditPickupSound()
    {
        PlaySFX(creditPickup);
    }
    public void PlayPEDActivateSound()
    {
        PlaySFX(pedActivate);
    }
    public void PlayPEDDeactivateSound()
    {
        PlaySFX(pedDeactivate);
    }
    public void PlayBoostSound()
    {
        PlaySFX(boostActivate);
    }
    public void PlayDashSound()
    {
        PlaySFX(dashActivate);
    }
    public void PlayInvincibility()
    {
        PlaySFX(invincibilityActivate);
    }
    public void PlayMissileSound()
    {
        PlaySFX(missileFire);
    }
    public void PlayMissileExplosion()
    {
        PlaySFX(missileExplosion);
    }


}

