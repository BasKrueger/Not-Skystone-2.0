using System;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkEvents : MonoBehaviourPunCallbacks
{
	public static event Action JoinedMaster;
	public static event Action LeftMaster;

	public static event Action JoinedLobby;
	public static event Action LeftLobby;

	public static event Action JoinedRoom;
	public static event Action LeftRoom;
	public static event Action AnotherJoinedRoom;
	public static event Action AnotherLeftRoom;

	public override void OnConnected() => JoinedMaster?.Invoke();
	public override void OnDisconnected(DisconnectCause cause) => LeftMaster?.Invoke();

	public override void OnJoinedLobby() => JoinedLobby?.Invoke();
	public override void OnLeftLobby() => LeftLobby?.Invoke();

	public override void OnLeftRoom() => LeftRoom?.Invoke();
	public override void OnJoinedRoom() => JoinedRoom?.Invoke();
	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) => AnotherJoinedRoom?.Invoke();
	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) => AnotherLeftRoom?.Invoke();

	void Awake()
	{
		FindObjectsOfType<MonoBehaviour>(true)
		.Where(o => o.GetComponent<INetworkSubscriber>() != null)
		.Select(o => o.GetComponent<INetworkSubscriber>())
		.ToList().ForEach(listener => listener.NetworkAwake());
	}
}

public interface INetworkSubscriber
{
	public void NetworkAwake();
}