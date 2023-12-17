using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Tools;
using Mirror;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    [MMReadOnly]
    private GameObject spawnedItem;
    [SerializeField]
    [MMReadOnly]
    private bool isCoolDown = false;
    private bool isManagerExist
    {
        get { return ItemSpawnManager.instance != null; }
    }
    void SpawnItemHere()
    {
        if (spawnedItem != null || isCoolDown || !isManagerExist)
            return;

        spawnedItem = Instantiate(ItemSpawnManager.instance.SpawnItem());

        spawnedItem.transform.position = transform.position;
        spawnedItem.transform.parent = transform;
    }


    private void Start()
    {
        
        SpawnItemHere();
    }

    private void Update()
    {
        if (spawnedItem == null)
            WaitCoolDown();
    }

    private void WaitCoolDown()
    {
        if (isCoolDown)
            return;
        StartCoroutine(CorWaitCoolDown());
    }

    private IEnumerator CorWaitCoolDown()
    {
        isCoolDown = true;
        yield return new WaitForSeconds(ItemSpawnManager.instance.spawnCoolTime);
        isCoolDown = false;
        SpawnItemHere();
    }
}
