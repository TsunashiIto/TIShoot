using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Continue controller.
/// コンティニューパネルの表示
/// カウントダウン処理
/// YESボタン処理
/// NOボタン処理
/// timeを止めているのでコルーチンは使用しない
/// </summary>
public class ContinueController : MonoBehaviour {

	//ゲームオーバー
	public static UnityAction OnGameOver;
	public static UnityAction InvokeContinue;

	void OnEnable(){
		//受信イベント
	}

	void OnDisable(){
		
	}

	public void PressYes(){
		InvokeContinue ();
	}

	public void PressNo(){
		OnGameOver ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



}
