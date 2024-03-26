using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using Photon.Pun;
using System.Collections.Generic;

public class BombController : MonoBehaviourPunCallbacks
{
    public KeyCode inputKey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombAmout = 1;
    private int bombsRemaining;
    
    public float explosionDuration = .7f;
    public int explosionRadius = 1;
    public LayerMask explosionLayerMask;
    public List<GameObject> myOjb = new List<GameObject>();
    public Tilemap destructibleTiles;
    private PhotonView view;
    private void Start() {
        destructibleTiles = GameObject.Find("Destructible").GetComponent<Tilemap>();
        view = GetComponent<PhotonView>();  
    }
    private void OnEnable() {
        bombsRemaining = bombAmout;
    }

    private void Update() {
        if(bombsRemaining > 0 && Input.GetKeyDown(inputKey) && view.IsMine) {
            StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb() {
            Vector2 position = transform.position;
            position.x = Mathf.RoundToInt(position.x);
            position.y = Mathf.RoundToInt(position.y);

            GameObject bomb = PhotonNetwork.Instantiate("Bomb", position, Quaternion.identity);
            myOjb.Add(bomb);
            bombsRemaining--;

            yield return new WaitForSeconds(bombFuseTime);

            position = bomb.transform.position;
            position.x = Mathf.RoundToInt(position.x);
            position.y = Mathf.RoundToInt(position.y);
            Explosion explosion = PhotonNetwork.Instantiate("Explosion", position, Quaternion.identity).GetComponent<Explosion>();
            myOjb.Add(explosion.gameObject);
            explosion.anime.SetTrigger("Start");
            StartCoroutine(DestroyCoroutune(explosion.gameObject, explosionDuration));

            Explode(position, Vector2.down, explosionRadius);
            Explode(position, Vector2.up, explosionRadius);
            Explode(position, Vector2.left, explosionRadius);
            Explode(position, Vector2.right, explosionRadius);

            PhotonNetwork.Destroy(bomb);
            myOjb.Remove(bomb);
            bombsRemaining++;
            

        
    }

    private void Explode(Vector2 position, Vector2 direction, int length) {
        if (length <= 0 || !view.IsMine)
            return;

        position += direction;

        if(Physics2D.OverlapBox(position,Vector2.one / 2f, 0f, explosionLayerMask)) {
            ClearDestructible(position);
            return;
        }

        Explosion explosion = PhotonNetwork.Instantiate("Explosion", position, Quaternion.identity).GetComponent<Explosion>();
        myOjb.Add(explosion.gameObject);
        explosion.anime.SetTrigger(length > 1 ? "Middle" : "End");
        explosion.SetDirection(direction);
        StartCoroutine(DestroyCoroutune(explosion.gameObject, explosionDuration));
        Explode(position, direction, length - 1);
    }

    IEnumerator DestroyCoroutune(GameObject go, float time) {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(go.gameObject);
        myOjb.Remove(go);
    }


    private void OnTriggerExit2D(Collider2D c) {
        if(c.gameObject.layer == LayerMask.NameToLayer("Bomb") && view.IsMine) {
            c.isTrigger = false;
        }
    }

    private void ClearDestructible(Vector2 position) {

        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if(tile != null)
        {
            PhotonNetwork.Instantiate("Destructible", position, Quaternion.identity);
            view.RPC("SetTileRPC", RpcTarget.AllBuffered, (Vector3)cell);
        }
    }

    [PunRPC]
    private void SetTileRPC(Vector3 cell) {
        destructibleTiles.SetTile(Vector3Int.FloorToInt(cell), null);
    }

    public void AddBomb() {
        
        bombAmout++;
        bombsRemaining++;
    }
}
