using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgress 
{
    void StartWave();
    bool IsDone();
    void ResetWave();
    void Spawn(GameObject obj, float x, float yMin, float yMax);
    void StopWave();
}
