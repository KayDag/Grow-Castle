using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ManagerGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Progess1.Instance.SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if (Castle.Instance.health > 0 && Progess1.Instance.Done() == true)
        {
            Progess1.Instance.SpawnEnemies();
        }
    }
}
