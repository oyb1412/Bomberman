using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Button CreateRoomBtn;
    public Button JoinRoomBtn;

    public InputField RoomNameField;
    public InputField RoomLimitNumberField;
    public GameObject RoomListItem;
    public Transform svContent;

    private Dictionary<string, RoomInfo> _dicRoomList = new Dictionary<string, RoomInfo>();

    private void Awake() {
        //RoomNameField.onValueChange.AddListener(OnNameValueChanged);
        //RoomLimitNumberField.onValueChange.AddListener(OnPlayerValueChange);
        CreateRoomBtn.onClick.AddListener(OnClickCreateRoom);
        JoinRoomBtn.onClick.AddListener(OnClickJoinRoom);
    }

    //�� ����� ��ȭ�� ���� �� ȣ��Ǵ� �Լ�
    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        base.OnRoomListUpdate(roomList);
        //Content�� �ڽ����� �پ��ִ� Item�� �� ����
        DeleteRoomListItem();
        //dicRoomInfo ������ roomList�� �̿��ؼ� ����
        UpdateRoomListItem(roomList);
        //dicRoom�� ������� roomListItem�� ������
        CreateRoomListItem();
    }
   
    private void SelectRoomListItem(string roomName) {
        RoomNameField.text = roomName;
    }

    private void DeleteRoomListItem() {
        foreach(Transform c in svContent) {
            Destroy(c.gameObject);
        }
    }

    private void UpdateRoomListItem(List<RoomInfo> roomList) {
        foreach(var info in roomList) {
            //dicRoomInfo�� info �� ���̸����� �Ǿ��ִ� key���� �����ϴ°�
            if (_dicRoomList.ContainsKey(info.Name)) {
                //���࿡ ���� �����Ǿ�����?
                if (info.RemovedFromList) {
                    _dicRoomList.Remove(info.Name); //����
                    continue;
                }
            }
            _dicRoomList[info.Name] = info; //�߰�
        }
    }

    void CreateRoomListItem() {
        foreach (RoomInfo info in _dicRoomList.Values) {
            //�� ���� ������ ���ÿ� ScrollView-> Content�� �ڽ����� ����
            GameObject go = Instantiate(RoomListItem, svContent);
            //������ item���� RoomListItem ������Ʈ�� �����´�.
            RoomList item = go.GetComponent<RoomList>();
            //������ ������Ʈ�� ������ �ִ� SetInfo �Լ� ����
            item.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //item Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ� ���
            item.onDelegate = SelectRoomListItem;
        }
    }

    void OnNameValueChanged(string s) {
        JoinRoomBtn.interactable = s.Length > 0;
        if (RoomNameField.text == "")
            CreateRoomBtn.interactable = false;
    }
    void OnPlayerValueChange(string s) {
        CreateRoomBtn.interactable = s.Length > 0;
        if (RoomLimitNumberField.text == "")
            CreateRoomBtn.interactable = false;
    }

    // ���� ��ư Ŭ���� ȣ��Ǵ� �Լ�
    public void OnClickCreateRoom() {
        //if (string.IsNullOrEmpty(RoomNameField.text) || string.IsNullOrEmpty(RoomLimitNumberField.text))
        //    return;

        //�� �ɼ�
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = int.Parse(RoomLimitNumberField.text);

        //�� ��Ͽ� ���̰� �Ұ��ΰ�?
        options.IsVisible = true;

        //�濡 ���� ���� ����
        options.IsOpen = true;

        //�� ����
        PhotonNetwork.CreateRoom(RoomNameField.text, options);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("�� ���� ����" + message);
    }
    public override void OnCreatedRoom() {
        base.OnCreatedRoom();
        Debug.Log("�� ���� ����");
    }
    public void OnClickJoinRoom() {
        // �� ����
        PhotonNetwork.JoinRoom(RoomNameField.text);
    }
    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        Debug.Log("�� ���� ����");
        SceneManager.LoadScene("Room");
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("�� ���� ����" + message);
    }
    void JoinOrCreateRoom() {
        //�� �ɼ�
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = int.Parse(RoomLimitNumberField.text);

        //�� ��Ͽ� ���̰� �Ұ��ΰ�?
        options.IsVisible = true;

        //�濡 ���� ���� ����
        options.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom(RoomNameField.text, options, TypedLobby.Default);
    }
    void JoinRandomRoom() {
        PhotonNetwork.JoinRandomRoom();
    } // ���� ��ư Ŭ���� ȣ��Ǵ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message) {
        base.OnJoinRandomFailed(returnCode, message);
    }
}
