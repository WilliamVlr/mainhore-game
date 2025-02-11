using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehavior : MonoBehaviour, IDataPersistence
{
    private Vector3 backgroundPosition;

    private void Update()
    {
        backgroundPosition = transform.position;
    }

    public void LoadData(GameData data)
    {
        backgroundPosition = data.mainBackgroundPos;
        transform.position = backgroundPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.mainBackgroundPos = backgroundPosition;
    }
}
