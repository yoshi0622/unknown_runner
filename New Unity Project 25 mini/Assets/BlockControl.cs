﻿using UnityEngine;
using System.Collections;

public class BlockControl : MonoBehaviour {
	public MapCreator map_creator = null;

	// Use this for initialization
	void Start () {
		map_creator = GameObject.Find ("GameRoot").GetComponent<MapCreator> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.map_creator.isDelete(this.gameObject)) {
			GameObject.Destroy (this.gameObject);
		}
		
	}
}
