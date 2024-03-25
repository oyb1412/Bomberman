using UnityEngine;

public class Destruceible : MonoBehaviour
{
    public float destructionTime = 1f;

    public float itemSpawnChance = 0.2f;
    public GameObject[] spawnbleItems;
    private void Start() {
        Destroy(gameObject, destructionTime);
    }

    private void OnDestroy() {
        if(spawnbleItems.Length > 0 && Random.value < itemSpawnChance) {
            int randomIndex = Random.Range(0, spawnbleItems.Length);
            Instantiate(spawnbleItems[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
