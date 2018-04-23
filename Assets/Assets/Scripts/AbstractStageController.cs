using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbstractStageController : MonoBehaviour {

	public static UnityAction OnSpawn;

	// Use this for initialization
	void Start () {
		StartCoroutine ("StageSequence");
	}
	
	public IEnumerator StageSequence(){
		OnSpawn ();
		yield return 0;
	}
}
