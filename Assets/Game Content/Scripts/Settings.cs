using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private FadeManager SettingsPannelFM;
    [Header("Settings")]
    [SerializeField] private BoolValueData sfxEnabled;
    [SerializeField] private BoolValueData musicEnabled;
    [Header("Icons")]
    [SerializeField] private GameObject MusicEnabledIcon;
    [SerializeField] private GameObject MusicDisabledIcon;
    [SerializeField] private GameObject SfxEnabledIcon;
    [SerializeField] private GameObject SfxDisabledIcon;
    private void Start()
    {
        LoadSettings();
    }
    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("SFX"))
        {
            if (PlayerPrefs.GetString("SFX") == "enabled")
            {
                TurnOnSFX();
            }
            else
            {
                TurnOfSFX();
            }
        }
        else
        {
            TurnOnSFX();
        }
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetString("Music") == "enabled")
            {
                TurnOnMusic();
            }
            else
            {
                TurnOfMusic();
            }
        }
        else
        {
            TurnOnMusic();
        }
    }

    public void TurnOnorOfSFX()
    {
        if (sfxEnabled.value)
        {
            TurnOfSFX();
        }
        else
        {
            TurnOnSFX();
        }
    }
    public void TurnOnOrOfMusic()
    {
        if (musicEnabled.value)
        {
            TurnOfMusic();
            AudioManager.Instance.StopMusic();
        }
        else
        {
            TurnOnMusic();
        }
    }
    void TurnOnSFX()
    {
        sfxEnabled.value = true;
        SfxDisabledIcon.SetActive(false);
        SfxEnabledIcon.SetActive(true);
        PlayerPrefs.SetString("SFX", "enabled");
    }
    void TurnOfSFX()
    {
        sfxEnabled.value = false;
        SfxDisabledIcon.SetActive(true);
        SfxEnabledIcon.SetActive(false);
        PlayerPrefs.SetString("SFX", "disabled");
    }
    void TurnOnMusic()
    {
        musicEnabled.value = true;
        MusicDisabledIcon.SetActive(false);
        MusicEnabledIcon.SetActive(true);
        AudioManager.Instance.PlayMusic("GhostTheme");
        PlayerPrefs.SetString("Music", "enabled");
    }
    void TurnOfMusic()
    {
        musicEnabled.value = false;
        MusicDisabledIcon.SetActive(true);
        MusicEnabledIcon.SetActive(false);
        PlayerPrefs.SetString("Music", "disabled");
    }
    public bool GetSFXEnabled()
    {
        return sfxEnabled;
    }
    public bool GetMusicEnabled()
    {
        return musicEnabled;
    }
}
