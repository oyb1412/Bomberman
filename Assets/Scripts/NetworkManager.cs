using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Vector2[] StartPos;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 pos = StartPos[PhotonNetwork.LocalPlayer.ActorNumber];
        PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);
    }
}
