using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ConcedeScreen : MonoBehaviour, INetworkSubscriber
{
	public void NetworkAwake()
	{
	   NetworkEvents.AnotherLeftRoom += () => this.gameObject.SetActive(true);
	}
	
	public void ClickedOK()
	{
		PhotonNetwork.LeaveRoom();
		this.gameObject.SetActive(false);
	}
}
