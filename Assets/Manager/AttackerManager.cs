using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Rendering;
using UnityEngine;

public class AttackerManager : MonoBehaviour
{
    public static AttackerManager Instance;

    public List<Transform> point;            // danh sách điểm spawn
    private List<bool> check = new List<bool>(); // theo dõi điểm đã spawn chưa 
    public List<Attacker> attacker = new List<Attacker>();
    private HashSet<Attacker> homeAttackers = new HashSet<Attacker>();
    public Attacker attPrefab;    

    public int baseGold = 35;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        check.Clear();
        for (int i = 0; i < point.Count; i++)
            check.Add(false);

        baseGold = 35;

        int startCount = Mathf.Min(5, point.Count);
        for (int i = 0; i < startCount; i++)
        {
            Attacker newAtt = Instantiate(attPrefab, point[i].position, Quaternion.identity);
            newAtt.SetIndex(i);
            Register(newAtt, i);
            ManagerGame.Instance.aliveScouts++;
        }
        UIManager.Instance.Scouts();
    }

    void Update()
    {

    }

    public void AddAttacker()
    {
        int gold = baseGold + ((int)ManagerGame.Instance.stats.attacker - 1) * 20;
        if (ManagerGame.Instance.stats.gold >= gold && attPrefab != null)
        {
            for (int i = 0; i < point.Count; i++)
            {
                if (!check[i] && point[i] != null)
                {
                    Attacker newAtt = Instantiate(attPrefab, point[i].position, Quaternion.identity);
                    newAtt.SetIndex(i);
                    Register(newAtt, i);
                    ManagerGame.Instance.aliveScouts++;
                    ManagerGame.Instance.stats.gold -= gold;
                    return;
                }
            }
        }
    }
    public void Register(Attacker att, int index)
    {
        attacker.Add(att);
        check[index] = true;
    }

    public void UnRegister(Attacker att)
    {
        int slot = att.GetIndex();

        if (slot >= 0 && slot < check.Count)
            check[slot] = false;

        attacker.Remove(att);
        ManagerGame.Instance.aliveScouts--;
    }
    public void NewWave()
    {
        for (int i = attacker.Count - 1; i >= 0; i--)
        {
            Attacker att = attacker[i];

            if (att == null)
            {
                attacker.RemoveAt(i);
                continue;
            }

            if (homeAttackers.Contains(att))
            {
                att.transform.position = point[att.GetIndex()].position;
                att.ResetState();
                att.gameObject.SetActive(true);
                homeAttackers.Remove(att);
            }
        }
    }

    public bool FullAttacker()
    {
        return (point.Count == attacker.Count);
    }
    public void ApplyStatsAll(PlayerStatsManager stats)
    {
        foreach (var att in attacker)
        {
            if (att != null)
                att.ApplyStats(stats);
        }
    }
    public void MarkReturnHome(Attacker att)
    {
        homeAttackers.Add(att);
    }

}
