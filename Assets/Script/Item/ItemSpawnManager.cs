using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Tools;

/// <summary>
/// 싱글톤패턴으로 작성된 아이템 스폰 매니저
/// 아이템 스포너에서 스폰할 아이템을 넘겨주는 클래스입니다.
/// </summary>
public class ItemSpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemSpawnCandidate
    {
        public GameObject itemPrefab;
        public float spawnProbability;
        [MMReadOnly]
        public float calcProbability;
    }
    public List<ItemSpawnCandidate> spawnList;
    public float spawnCoolTime = 15f;
    private float totalProbability = 0;
    private bool isInitiated = false;
    public static ItemSpawnManager instance = null;

    private void Awake()
    {
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            instance = this; //내자신을 instance로 넣어줍니다.
            //DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else
        {
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
                Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
        }
    }

    public void InitProbability()
    {
        if (isInitiated)
            return;

        totalProbability = 0;
        foreach (var item in spawnList)
        {
            totalProbability += item.spawnProbability;
            item.calcProbability = totalProbability;
        }
        isInitiated = true;
    }

    public GameObject SpawnItem()
    {
        InitProbability();
        float rand = Random.Range(0f, totalProbability);
        foreach (var item in spawnList)
        {
            if (item.calcProbability > rand)
                return item.itemPrefab;
        }
        return spawnList[0].itemPrefab;
    }
}
