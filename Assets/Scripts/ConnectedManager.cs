using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ConnectedManager : MonoBehaviourPunCallbacks
{
    private static ConnectedManager _instance;
    public static ConnectedManager Instance { get { return _instance; } }

    public InputField _inputField;
    public Button btn;
    private void Awake() {
        //Screen.SetResolution(960, 540, false);

        //�ʴ� Send Ƚ��
        PhotonNetwork.SendRate = 60;

        //�ʴ� ��Ŷ ����ȭ Ƚ��
        PhotonNetwork.SerializationRate = 30;
        btn.onClick.AddListener(OnConnected);
    }

    private new void OnConnected() {
        if(_inputField.text.Length > 0) {
            //���� Ŭ���忡 ����
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    /// <summary>
    /// PhotonNetwork.ConnectUsingSettings()���� ���� �ڵ����� ȣ���.
    /// </summary>
    public override void OnConnectedToMaster() {
        Debug.Log($"���� {_inputField.text} ������ ���� ���� ����");
        PhotonNetwork.LocalPlayer.NickName = _inputField.text;
        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// PhotonNetwork.JoinLobby()�Լ��� �ڵ����� ȣ���.
    /// </summary>
    public override void OnJoinedLobby() {
        base.OnJoinedLobby();
        PhotonNetwork.LoadLevel("Lobby");
        Debug.Log($"���� {_inputField.text} �κ� ���� ����");
    }
}
