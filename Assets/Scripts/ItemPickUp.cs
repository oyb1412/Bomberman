using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ItemPickUp : MonoBehaviour {
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
            Destroy(gameObject);
        }
    }
}
