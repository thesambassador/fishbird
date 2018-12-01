using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class BallChain : MonoBehaviour {

	public HingeJoint2D chainPrefab;

	public int numChains = 10;
	public float chainLength = 1;

	public HingeJoint2D[] chains;
	public Transform chainStart;
	public HingeJoint2D ballObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Button("GenerateChains")]
	void GenerateLinks() {
		for (int i = chainStart.childCount-1; i >= 0; i--) {
			DestroyImmediate(chainStart.GetChild(i).gameObject);
		}

		chains = new HingeJoint2D[numChains];

		Vector2 chainPos = Vector2.zero;

		for (int i = 0; i < numChains; i++) {
			chains[i] = Instantiate(chainPrefab);
			chains[i].transform.parent = chainStart;
			chains[i].transform.position = chainStart.position - (chainStart.up * i * chainLength);
		}

		for (int i = 0; i < numChains - 1; i++) {
			chains[i].connectedBody = chains[i + 1].GetComponent<Rigidbody2D>();
		}

		ballObject.connectedBody = chains[0].GetComponent<Rigidbody2D>();
		ballObject.GetComponent<DistanceJoint2D>().connectedBody = ballObject.connectedBody;
		chains[chains.Length - 1].GetComponent<Rigidbody2D>().isKinematic = true;
		chains[chains.Length - 1].enabled = false;
	}
}
