using UnityEngine;

public class AudioSettingsButtonClicked : MonoBehaviour
{
    [SerializeField] private GameObject _onImage;
    [SerializeField] private GameObject _offImage;
    [SerializeField] private ButtonType _type;
    public enum ButtonType
    {
        Audio,
        Music
    }

    public void ToggleSetting()
    {
        if (_type == ButtonType.Audio)
            SoundSettings.ToggleAudio();
        if (_type == ButtonType.Music)
            SoundSettings.ToggleMusic();
        ToggleIcons();
    }

    private void OnEnable()
    {
        if (_type == ButtonType.Audio)
            if (SoundSettings.AudioMuted)
                if (!_offImage.activeSelf)
                    ToggleIcons();

        if (_type == ButtonType.Music)
            if (SoundSettings.MusicMuted)
                if (!_offImage.activeSelf)
                    ToggleIcons();
    }

    private void ToggleIcons()
    {
        _onImage.SetActive(!_onImage.activeSelf);
        _offImage.SetActive(!_offImage.activeSelf);
    }
}
