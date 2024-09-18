using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ActiveOnConnectionState : MonoBehaviour, INetworkSubscriber
{
	private enum ConnectionState
	{
		Master,
		Lobby,
		Room
	}

	[SerializeField]
	private List<ConnectionState> activateOnJoin;
	[SerializeField]
	private List<ConnectionState> activateOnLeave;

	[SerializeField]
	private List<ConnectionState> disableOnJoin;
	[SerializeField]
	private List<ConnectionState> disableOnLeave;
	
	public void NetworkAwake()
	{
	   	NetworkEvents.JoinedMaster += () => ChangedTo(ConnectionState.Master, true);
		NetworkEvents.LeftMaster += () => ChangedTo(ConnectionState.Master, false);

		NetworkEvents.JoinedLobby += () => ChangedTo(ConnectionState.Lobby, true);
		NetworkEvents.LeftLobby += () => ChangedTo(ConnectionState.Lobby, false);

		NetworkEvents.JoinedRoom += () => ChangedTo(ConnectionState.Room, true);
		NetworkEvents.LeftRoom += () => ChangedTo(ConnectionState.Room, false);
		
		if(activateOnLeave.Contains(ConnectionState.Master))
		{
			this.gameObject.SetActive(true);
		}
	}
	
	private void ChangedTo(ConnectionState state, bool active)
	{
		if(active)
		{
			if(activateOnJoin.Contains(state))
			{
				this.gameObject.SetActive(true);
			}
			if(disableOnJoin.Contains(state))
			{
				this.gameObject.SetActive(false);
			}
		}
		
		if(!active)
		{
			if(activateOnLeave.Contains(state))
			{
				this.gameObject.SetActive(true);
			}
			if(disableOnLeave.Contains(state))
			{
				this.gameObject.SetActive(false);
			}
		}
	}
}
