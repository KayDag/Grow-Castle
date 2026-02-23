using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerManager : MonoBehaviour
{
    public static AttackerManager Instance;

    public List<Transform> point;            // danh sách điểm spawn
    public List<bool> check = new List<bool>(); // theo dõi điểm đã spawn chưa 
    public List<Attacker> attacker = new List<Attacker>();
    private HashSet<Attacker> homeAttackers = new HashSet<Attacker>();
    public Attacker attPrefab;    

    public int baseGold = 25;

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

        baseGold = 25;

        int startCount = Mathf.Min(ManagerGame.Instance.scout, point.Count);
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
        int gold = baseGold + ((int)ManagerGame.Instance.stats.attacker - 1) * 5;
        if (ManagerGame.Instance.stats.gold < gold) return;
        if (attacker.Count >= point.Count) return;

        int index = attacker.Count;

        Attacker newAtt = Instantiate(attPrefab, point[index].position, Quaternion.identity);
        newAtt.SetIndex(index);
        attacker.Add(newAtt);

        ManagerGame.Instance.aliveScouts++;
        ManagerGame.Instance.stats.gold -= gold;
        ManagerGame.Instance.scout++;
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
        }
        int needSpawn = ManagerGame.Instance.completedScouts;
        ManagerGame.Instance.completedScouts = 0;

        for (int i = 0; i < point.Count && needSpawn > 0; i++)
        {
            if (!check[i] && point[i] != null)
            {
                Attacker newAtt = Instantiate(attPrefab, point[i].position, Quaternion.identity);
                newAtt.SetIndex(i);
                Register(newAtt, i);
                ManagerGame.Instance.aliveScouts++;
                needSpawn--;
            }
        }
    }
    public void ResetScouts()
    {
        int startAtt = 0;
        // Xóa toàn bộ scout cũ
        for (int i = attacker.Count - 1; i >= 0; i--)
        {
            if (attacker[i] != null)
            {
                Destroy(attacker[i].gameObject);
                startAtt++;
            }
        }
        attacker.Clear();

        // Reset slot
        for (int i = 0; i < check.Count; i++)
            check[i] = false;

        // Spawn lại scout ban đầu
        for (int i = 0; i < ManagerGame.Instance.scout; i++)
        {
            Attacker newAtt = Instantiate(attPrefab, point[i].position, Quaternion.identity);
            newAtt.SetIndex(i);
            Register(newAtt, i);
        }

        ManagerGame.Instance.aliveScouts = attacker.Count;
        UIManager.Instance.Scouts();
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
}
