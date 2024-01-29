using UnityEngine;

public class MainMenuSoundSettings : MonoBehaviour
{
    private bool _settingsChangedByUser;

    public void SettingChanged() => _settingsChangedByUser = true;

    private void Start()
    {
        if (!SoundSettings.MusicMuted)
            SoundSettings.ToggleMusic();
    }
    private void OnDestroy()
    {
        if (!_settingsChangedByUser)
            if (SoundSettings.MusicMuted)
                SoundSettings.ToggleMusic();
    }
}
