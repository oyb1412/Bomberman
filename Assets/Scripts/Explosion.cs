using Photon.Pun;
using UnityEngine;

public class Explosion : MonoBehaviourPunCallbacks
{
    private PhotonView view;
    public Animator anime;
    private void Awake() {
        view = GetComponent<PhotonView>();
        anime = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 direction) {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

}
