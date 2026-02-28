using System.Collections.Generic;
using UnityEngine;

public class TowerRay : MonoBehaviour
{
    [SerializeField] public int _index;
    [SerializeField] private List<ParticleSystem> _rayParticles;

    public int Index => _index;

    public void Activate()
    {
        foreach (ParticleSystem ray in _rayParticles)
        {
            ray.Play();
        }
    }

    public void Deactivate()
    {
        foreach (ParticleSystem ray in _rayParticles)
        {
            ray.Stop();
        }
    }
}
