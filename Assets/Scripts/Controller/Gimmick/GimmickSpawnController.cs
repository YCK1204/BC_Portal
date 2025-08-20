using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GimmickSpawnData
{
    public GameObject Gc;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale = Vector3.one;
    public Transform Parent;
}
[Serializable]
public class GimmickSpawnDataList
{
    public List<GimmickSpawnData> DataList = new List<GimmickSpawnData>();
}
public class GimmickSpawnController : BaseGimmickController
{
    [SerializeField]
    public GimmickSpawnDataList SpawnDataList;
    [SerializeReference]
    public List<GameObject> DeSpawnDataList;
    public override void Enter()
    {
        foreach (var data in SpawnDataList.DataList)
        {
            if (data.Gc == null)
            {
                Debug.LogError($"GimmickSpawnController({gameObject.name}) SpawnDataList has a empty GimmickController");
                continue;
            }
            var instance = Instantiate(data.Gc, data.Position, data.Rotation);
            instance.transform.localScale = data.Scale;
            if (data.Parent != null)
                instance.transform.SetParent(data.Parent);
            instance.gameObject.SetActive(true);
        }

        foreach (var gc in DeSpawnDataList)
        {
            if (gc == null)
            {
                Debug.LogError($"GimmickSpawnController({gameObject.name}) DeSpawnDataList has a empty GimmickController");
                continue;
            }
            Destroy(gc.gameObject);
        }
        if (DeSpawnDataList.Count > 0)
            DeSpawnDataList.Clear();
    }

    public override void Exit()
    {
    }

    protected override void Init()
    {
    }
}
