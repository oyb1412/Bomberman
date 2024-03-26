using Photon.Pun;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ItemPickUp : MonoBehaviourPunCallbacks {
    private PhotonView view;

    private void Awake() {
        view = GetComponent<PhotonView>();
    }
    public enum ItemType {
       ExtraBomb,
       BlastRadius,
       SpeedIncrease,
    }

    public ItemType type;
    private void OnItemPickUp(GameObject player) {
        switch(type) {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;
            case ItemType.BlastRadius:
                player.GetComponent<BombController>().explosionRadius++;
                break;
            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().speed++;
                break; ;
        }
    }
    private void OnTriggerEnter2D(Collider2D c) {
        if(c.CompareTag("Player")) {
            OnItemPickUp(c.gameObject);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
