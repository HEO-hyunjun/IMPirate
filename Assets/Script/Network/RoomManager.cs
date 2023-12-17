using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoomManager : NetworkRoomManager
{
    public static new RoomManager singleton { get; private set; }
    public override void Awake()
    {
        base.Awake();
        singleton = this;
    }
    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerConnect(conn);

        var spawnPosition = FindObjectOfType<SpawnPositions>().GetSpawnPosition();

        var player = Instantiate(spawnPrefabs[0], spawnPosition, Quaternion.identity);
        
        NetworkServer.Spawn(player, conn);
    }
    public override void OnClientConnect()
    {
        base.OnClientConnect();
    }

    public GameObject FindItemByGameObject(GameObject trg)
    {
        ItemGiver trgItemGiver = trg.GetComponent<ItemGiver>();
        if (trgItemGiver == null)
            return null;
        foreach(GameObject i in spawnPrefabs)
        {
            ItemGiver itemGiver = i.GetComponent<ItemGiver>();
            if(itemGiver != null &&itemGiver.item.itemID == trgItemGiver.item.itemID)
            {
                return i;
            }
        }
        return null;
    }
}
