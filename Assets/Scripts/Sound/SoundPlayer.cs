using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundPlayer : MonoBehaviour
{
    //Music track: Sunset by Lukrembo
    //Source: https://freetouse.com/music
    //Free Vlog Music Without Copyright
    [SerializeField] private AudioSource _musicAS;
    [SerializeField] private AudioSource _soundAS;
    [SerializeField] private AudioMixer _audioMixer;
    Button[] _buttons;

    private void Awake()
    {
        SoundSettings.OnSettingsChanged += OnAudioSettingsChanged;
        _buttons = FindObjectsOfType<Button>(true);
        foreach (var button in _buttons)
            if (!button.CompareTag("Handle"))
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
        if (SoundSettings.MusicMuted)
            _audioMixer.SetFloat("MusicVolume", -80);
        else
            _audioMixer.SetFloat("MusicVolume", 0);

        if (SoundSettings.AudioMuted)
            _audioMixer.SetFloat("EffectsVolume", -80);
        else
            _audioMixer.SetFloat("EffectsVolume", 0);
    }

    private void PlayClickSound()
    {
        if (!SoundSettings.AudioMuted)
            _soundAS.Play();
    }
}
