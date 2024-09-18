using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
	[SerializeField]
	private GameObject window;
	[SerializeField]
	private StartScreen startScreen;
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			window.gameObject.SetActive(!window.gameObject.activeInHierarchy);
		}
	}
	
	public void OnConcedeClicked()
	{
		Game.Concede();

		window.gameObject.SetActive(false);
	}
	
	public void MainMenuClicked()
	{
		if(PhotonNetwork.InRoom)
		{
			PhotonNetwork.LeaveRoom();
		}
		
		startScreen.gameObject.SetActive(true);
		window.gameObject.SetActive(false);
	}
	
	public void OnContinueClicked()
	{
		window.gameObject.SetActive(false);
	}
}
