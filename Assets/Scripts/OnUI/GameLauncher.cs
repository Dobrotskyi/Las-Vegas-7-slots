using UnityEngine;
using UnityEngine.UI;

public class GameLauncher : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private HorizontalLayoutGroup _slotsGroup;
    private string _selected = SlotMachineTypes.SlotsClassic.ToString();
    private LoadSceneClick _loadScene;

    public void PullTheHandle()
    {
        ParticleSystem first = null;
        foreach (Transform slot in _slotsGroup.transform)
        {
            if (first == null)
            {
                first = Instantiate(_effect, slot.position, Quaternion.identity);
                first.GetComponent<ParticleSystemCallback>().Stoped.AddListener(Launch);
            }
            else
                Instantiate(_effect, slot.position, Quaternion.identity);
        }
    }

    public void ChangeSelected(Toggle toggle)
    {
        if (toggle.isOn)
            _selected = toggle.gameObject.tag;
    }

    private void Launch()
    {
        _loadScene.LoadScene(_selected);
    }

    private void Awake()
    {
        _loadScene = GetComponent<LoadSceneClick>();
    }
}
