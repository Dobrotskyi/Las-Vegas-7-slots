using System;

public static class SoundSettings
{
    public const float EFFECTS_VOLUME = 0.3f;

    public static event Action OnSettingsChanged;
    public static bool AudioMuted { private set; get; }
    public static bool MusicMuted { private set; get; }

    public static void ToggleMusic()
    {
        MusicMuted = !MusicMuted;
        OnSettingsChanged?.Invoke();
    }

    public static void ToggleAudio()
    {
        AudioMuted = !AudioMuted;
        OnSettingsChanged?.Invoke();
    }
}
