using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Destruceible : MonoBehaviourPunCallbacks
{
    public float destructionTime = 1f;

    public float itemSpawnChance = 0.2f;

    private PhotonView view;

    private void Awake() {
        view = GetComponent<PhotonView>();
    }
    private void Start() {
        if(view.IsMine)
            StartCoroutine(DestroyCoroutune(gameObject, destructionTime));
    }
    IEnumerator DestroyCoroutune(GameObject go, float time) {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(go);
    }
    private void OnDestroy() {
        if(Random.value < itemSpawnChance) {
            int randomIndex = Random.Range(0, 5);
            if(randomIndex < 2) {
                PhotonNetwork.Instantiate("ItemPickUpBlastExtraBomb", transform.position, Quaternion.identity);
            }
            else if(randomIndex > 1 && randomIndex < 4) {
                PhotonNetwork.Instantiate("ItemPickUpBlastRadius", transform.position, Quaternion.identity);
            }
            else {
                PhotonNetwork.Instantiate("ItemPickUpSpeedIncrease", transform.position, Quaternion.identity);
            }
        }
    }
}
