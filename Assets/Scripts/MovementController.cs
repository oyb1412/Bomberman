using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementController : MonoBehaviourPunCallbacks
{
    public new Rigidbody2D rigidbody { get; private set; }
    private BombController bombController;
    private Vector2 direction = Vector2.zero;
    private Animator anime;
    public float speed = 5f;
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;
    bool isDied;
    public enum moveDir {
        DOWN = 1,
        UP,
        RIGHT,
        LEFT,
        IDLE,
        DIE,
    }

    public moveDir dir = moveDir.DOWN;
    public Text NickNameText;
    public PhotonView View;

    private void Awake() {
        bombController = GetComponent<BombController>();
        rigidbody = GetComponent<Rigidbody2D>();
        anime = GetComponentInChildren<Animator>();
    }

    private void Start() {
        NickNameText.text = View.IsMine ? PhotonNetwork.LocalPlayer.NickName : View.Owner.NickName;
        NickNameText.color = View.IsMine ? Color.green : Color.red;
    }

    private void Update() {
        if (!View.IsMine || isDied)
            return;

        if (Input.GetKey(inputUp)) {
            direction = Vector2.up;
            dir = moveDir.UP;
        } else if (Input.GetKey(inputDown)) {
            direction = Vector2.down;

            dir = moveDir.DOWN;

        } else if (Input.GetKey(inputLeft)) {
            direction = Vector2.left;

            dir = moveDir.LEFT;

        } else if (Input.GetKey(inputRight)) {

            direction = Vector2.right;

            dir = moveDir.RIGHT;
        } else {
            direction = Vector2.zero;
            dir = moveDir.IDLE;

        }

        anime.SetInteger("Move", (int)dir);
    }

    private void FixedUpdate() {
        if (!View.IsMine)
            return;

        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + translation);
    }
 

    private void OnTriggerEnter2D(Collider2D c) {
        if (c.CompareTag("Explosion") && !isDied && View.IsMine) {
            DeathSequence();
        }
    }

    private void DeathSequence() {
        enabled = false;
        isDied = true;
        anime.SetInteger("Move", (int)moveDir.DIE);
        GetComponent<BombController>().enabled = false;
        Invoke("OnDeathSequenceEnded", .8f);
    }
    
    private void OnDeathSequenceEnded() {
        View.RPC("DeathRPC", RpcTarget.AllBuffered);
        PhotonNetwork.Destroy(gameObject);
    }


    [PunRPC]
    private void DeathRPC() {
        List<GameObject> list = bombController.myOjb;
        foreach(var t in list) {
            if(t.gameObject != null)
                PhotonNetwork.Destroy(t);
        }
    }
}
