using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ParticleSystemCallback : MonoBehaviour
{
    public UnityEvent Stoped;
    private void OnParticleSystemStopped()
    {
        Stoped?.Invoke();
    }

    private void OnDestroy()
    {
        Stoped?.RemoveAllListeners();
    }
}
