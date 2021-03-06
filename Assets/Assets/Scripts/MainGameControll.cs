﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージ遷移管理。シングルトン
/// ハイスコア HIGHSCORE
/// 残機の最大数 maxZanki
/// 最大コンティニュー回数 maxContinue
/// 初期ボム数 bomb
/// 最大ボム数 maxBomb
/// 難易度 initDifficulty
/// ゲームオーバーの管理
/// </summary>
public class MainGameControll : MonoBehaviour {

	//singleton
	public static MainGameControll Instance { get; private set; }

	//value
	public int HIGHSCORE = 0;
	public int Bomb = 5;
	public Transform[] jikis;
	public Transform jiki;
	public int initDifficulty = 3;
	public bool isContinueEnable = false;

	private int StageNum = 0;//現在のステージのインデックス

	void Awake(){
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}			
	}

	void OnEnable(){
		//受信イベント
		TitleController.GAMESTART += this.GAMESTART;
		GameManager.OnDestroyBoss += this.OnDestroyBoss;
		GameManager.OnContinue += this.OnContinue;
		GameManager.OnGameOver += this.OnGameOver;
		ContinueController.OnGameOver += this.OnGameOver;
		ContinueController.InvokeContinue += this.InvokeContinue;
	}

	void OnDisable(){
		TitleController.GAMESTART -= this.GAMESTART;
		GameManager.OnDestroyBoss -= this.OnDestroyBoss;
		GameManager.OnContinue -= this.OnContinue;
		GameManager.OnGameOver -= this.OnGameOver;
		ContinueController.OnGameOver -= this.OnGameOver;
		ContinueController.InvokeContinue -= this.InvokeContinue;
	}

	// Use this for initialization
	void Start () {
		SceneManager.LoadScene ("SceneTitle", LoadSceneMode.Additive);
	}
	


	/// <summary>
	/// Deathイベント受信
	/// </summary>
	public void Death(){
		//GameObject.Find("SceanController").SendMessage("death");
	}

	/// <summary>
	/// ゲームスタートボタンが押されてからの遷移
	/// stage01だけ名前で取得。あとはindexを足していく
	/// </summary>
	public void GAMESTART(){
		SceneManager.LoadScene ("Stage01", LoadSceneMode.Additive);
		StageNum = SceneManager.GetSceneByName ("Stage01").buildIndex;
		SceneManager.UnloadSceneAsync (1);//unload title
	}

	/// <summary>
	/// ボスを破壊したときの遷移。
	/// ラスボス倒したときはどうしよう・・・
	/// </summary>
	public void OnDestroyBoss(){
		///とりあえずエンディング画面にとばす

		SceneManager.LoadScene ("EndingSimple",LoadSceneMode.Additive);
		//ここは後で現在のステージを取得する処理に変更
		SceneManager.UnloadSceneAsync (2);
	}

	/// <summary>
	/// コンティニュー時処理
	/// コンティニューのウィンドウを読み込む
	/// </summary>
	public void OnContinue(){
		Time.timeScale = 0;	
		SceneManager.LoadScene ("ContinueControll",LoadSceneMode.Additive);
		isContinueEnable = true;
		Debug.Log ("CONTINUE");
	}

	/// <summary>
	/// Invokes continue.
	/// コンティニューウィンドウを開く
	/// </summary>
	public void InvokeContinue(){
		Time.timeScale = 1;
		SceneManager.UnloadSceneAsync ("ContinueControll");
		isContinueEnable = false;
	}

	/// <summary>
	/// ゲームオーバー時処理
	/// なんか演出してタイトル画面に遷移する
	/// </summary>
	public void OnGameOver(){
		Time.timeScale = 1;

		if(isContinueEnable == true){
			SceneManager.UnloadSceneAsync ("ContinueControll");
			isContinueEnable = false;
		}
		SceneManager.UnloadSceneAsync (2);
		//ここは後で現在のステージを取得する処理に変更
		SceneManager.LoadScene ("SceneTitle", LoadSceneMode.Additive);
		GameObject.Find ("GameManager").GetComponent<GameManager> ().SendMessage ("ResetContinue") ;
		Debug.Log ("GAMEOVER");
	}

}
