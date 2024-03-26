using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Button StartBtn;
    // Start is called before the first frame update
    void Start()
    {
        StartBtn.onClick.AddListener(OnStartButtonPressed);
        StartBtn.onClick.AddListener(() => StartBtn.interactable = false);
    }
    public void OnStartButtonPressed() {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady", true } });
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps) {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (AllPlayersReady()) {
            // ��� �÷��̾ �غ�Ǿ��ٸ� ���� ������ ��ȯ
            PhotonNetwork.LoadLevel("InGame");
        }
    }

    bool AllPlayersReady() {
        foreach (Player player in PhotonNetwork.PlayerList) {
            object isPlayerReady;
            if (player.CustomProperties.TryGetValue("IsReady", out isPlayerReady)) {
                if (!(bool)isPlayerReady)
                    return false;
            } else {
                return false; // �غ� ���°� �������� ���� �÷��̾ �ִٸ�
            }
        }
        return true; // ��� �÷��̾ �غ�Ǿ���
    }
}
