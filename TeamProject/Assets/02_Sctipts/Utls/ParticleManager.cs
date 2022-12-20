using System.Collections.Generic;
using UnityEngine;

// ��ƼŬ�� �����Ѵ� 
// ���ϴ� ��ġ�� ������ְ�, �ش� ��ƼŬ�� ������ ������ ���ش�!
// - ���� ���
//    �̱���, ��ƼŬ ������ ���� ���� �ִ�!
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

    #region �������̽�

    public enum ParticleType
    {
        BubbleParticle,
        FishDie,
        ScreenClick,
        ScreenClickRing,
    }

    /// <summary>
    /// ��ƼŬ ���纻�� �����Ѵ�
    /// </summary>
    /// <param name="pt"> ��ƼŬ �̸� </param>
    /// <param name="pos"> ��ġ </param>
    /// <returns></returns>
    /// "Resources" �������� �ٷ� �о� �ͼ� Map�� ��� �� �ϳ��� ������ ��� �ֵ��� �Ѵ�.
    public int AddParticle(ParticleType pt, Vector3 pos)
    {
        switch (pt)
        {
            case ParticleType.BubbleParticle:
                if (false == particleDic.ContainsKey(pt))
                {
                    particleDic[pt] = Resources.Load<GameObject>("VFX/Bubble");
                }
                break;


            case ParticleType.FishDie:
                if (false == particleDic.ContainsKey(pt))
                {
                    particleDic[pt] = Resources.Load<GameObject>("VFX/FishDie");
                }
                break;

            case ParticleType.ScreenClick:
                if (false == particleDic.ContainsKey(pt))
                {
                    particleDic[pt] = Resources.Load<GameObject>("VFX/Click");
                }
                break;


            case ParticleType.ScreenClickRing:
                if (false == particleDic.ContainsKey(pt))
                {
                    particleDic[pt] = Resources.Load<GameObject>("VFX/OnShotClick");
                }
                break;


            default:
                Debug.LogWarning("���� �������� ���� ��ƼŬ");
                return 0;
        }

        if (particleDic[pt] == null)
        {
            Debug.LogWarning($"�ε��� ����!!! {pt}");
            return 0;
        }

        // �ش� ��ƼŬ�� ���纻 ����!
        GameObject go = Instantiate<GameObject>(particleDic[pt], pos, Quaternion.identity);

        return 0;
    }

    // ��ƼŬ ������ ��Ƶ���
    private static Dictionary<ParticleType, GameObject> particleDic = new Dictionary<ParticleType, GameObject>();
    #endregion
}
