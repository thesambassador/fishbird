using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class MenuStartText : MonoBehaviour {
	public PlayerSelectMenu p1;
	public PlayerSelectMenu p2;

	public string spP1 = "Press A or Space to Start \n Singleplayer";
	public string spP2 = "Press A or Num0 to Start";
	public string waitReady = "Both players need to ready";
	public string readyReady = "Press A or Space to Start\n Coop";

	public TextMeshProUGUI textObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!p1.ready && !p2.ready) {
			textObject.text = "";
		}
		else if(p1.ready && !p2.joined) {
			textObject.text = spP1;

			if (p1.pInput.GetButtonDown("Join")) {
				GameManager.instance.FishPlayerID = p1.playerNum;
				GameManager.instance.BirdPlayerID = p1.playerNum;
				GameManager.instance.StartGame();	
			}

		}
		else if (p2.ready && !p1.joined) {
			textObject.text = spP2;

			if (p2.pInput.GetButtonDown("Join")) {
				GameManager.instance.FishPlayerID = p2.playerNum;
				GameManager.instance.BirdPlayerID = p2.playerNum;
				GameManager.instance.StartGame();
			}
		}
		else if(!p1.ready || !p2.ready) {
			textObject.text = waitReady;
		}
		else if(p1.ready && p2.ready) {
			textObject.text = readyReady;
			GameManager.instance.StartGame(); //no need to set playernum since that happened when they joined
		}
	}
}
