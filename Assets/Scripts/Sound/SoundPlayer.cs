using UnityEngine;
using UnityEngine.UI;

public class SoundPlayer : MonoBehaviour
{
    //Music track: Vibing by Pufino
    //Source: https://freetouse.com/music
    //Music for Videos(Free Download)
    [SerializeField] private AudioSource _musicAS;
    [SerializeField] private AudioSource _soundAS;
    [SerializeField] private float _musicVolume = 0.5f;
    Button[] _buttons;

    private void Awake()
    {
        if (SoundSettings.MusicMuted)
            _musicAS.volume = 0;
        SoundSettings.OnSettingsChanged += OnAudioSettingsChanged;
        _buttons = FindObjectsOfType<Button>(true);
        foreach (var button in _buttons)
            button.onClick.AddListener(PlayClickSound);
    }

    private void OnDestroy()
    {
        SoundSettings.OnSettingsChanged -= OnAudioSettingsChanged;
        foreach (var button in _buttons)
            button.onClick.RemoveListener(PlayClickSound);
    }

    private void OnAudioSettingsChanged()
    {
        if (_musicAS.volume != _musicVolume && !SoundSettings.MusicMuted)
            _musicAS.volume = _musicVolume;
        else if (_musicAS.volume != 0 && SoundSettings.MusicMuted)
            _musicAS.volume = 0;
    }

    private void PlayClickSound()
    {
        if (!SoundSettings.AudioMuted)
            _soundAS.Play();
    }
}
