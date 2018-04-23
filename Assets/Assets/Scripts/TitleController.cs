using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// タイトルシーンの管理
/// メニュー
/// </summary>
public class TitleController : MonoBehaviour {

	public static UnityAction GAMESTART;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnGameStart(){
		GAMESTART ();
	}
}
