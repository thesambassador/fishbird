using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;
using TMPro;

public class PlayerSelectMenu : MonoBehaviour {
	public int playerNum = 0;
	public Player pInput;

	public PlayerSelectMenu otherPlayer;

	public bool joined = false;
	public bool ready = false;

	string fishString = "Fish";
	string birdString = "Bird";

	public TextMeshProUGUI playerText;
	public TextMeshProUGUI joinText;
	public TextMeshProUGUI readyText;
	public TextMeshProUGUI startText;


	// Use this for initialization
	void Start () {
		pInput = ReInput.players.GetPlayer(playerNum);

	}
	
	// Update is called once per frame
	void Update () {
		if (!joined) {
			if (pInput.GetButtonDown("Join")) {
				joined = true;
				if (!otherPlayer.joined) {
					playerText.text = fishString;
					playerText.gameObject.SetActive(true);
					GameManager.instance.FishPlayerID = playerNum;
					//joined as fish
				}
				else {
					playerText.text = birdString;
					playerText.gameObject.SetActive(true);
					GameManager.instance.BirdPlayerID = playerNum;
					//joined as bird
				}

				joinText.gameObject.SetActive(false);
				readyText.gameObject.SetActive(true);
			}
		}
		else if (!ready) {
			if (pInput.GetButtonDown("Join")) {
				ready = true;
				readyText.gameObject.SetActive(false);
			}

			if (pInput.GetButtonDown("Cancel")) {
				joined = false;
				joinText.gameObject.SetActive(true);
				playerText.gameObject.SetActive(false);
				readyText.gameObject.SetActive(false);
			}
		}
		else {
			if (pInput.GetButtonDown("Join")) {


			}

			if (pInput.GetButtonDown("Cancel")) {
				ready = false;
				readyText.gameObject.SetActive(true);
			}
		}
	}
}
