using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    #region SingleTon

    private static ParticleManager _instance = null;
    public static ParticleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ParticleManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("ParticleManager").AddComponent<ParticleManager>();
                }
            }
            return _instance;
        }
    }

    #endregion

    public List<ParticleName> ParticleNames = new List<ParticleName>();

    public void InstantiateParticle(ParticleName _num)
    {

    }
}


[Serializable]
public class ParticleName
{
    public string name;
    public GameObject particle;
}

