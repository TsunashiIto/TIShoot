using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ゲーム進行の管理
/// シングルトン
/// イベント処理
/// スコア管理
/// 難易度処理
/// ボム処理・管理
/// ゲームスタート管理
/// スポーン管理
/// デス・リスポーン管理
/// 残機・ゲームオーバー管理
/// (イベントの配信のみ）
/// コンティニュー管理
/// コンティニューYES時の処理
/// ステージクリア管理
/// ステージ読み込み等は行わない
/// </summary>
public class GameManager : MonoBehaviour {

	public int maxZanki = 3;
	public int maxContinue =3;
	public Transform[] jikis;//自機配列
	public Transform jiki;//自機prefab
	private GameObject jikiInstance;//自機インスタンス
	public int jikiNo = 0;//自機何番目か
	private int SCORE = 0;
	private int Zanki = 0;
	private int Continue = 0;
	private bool DEMO = false;

	//配信イベント
	//得点が変化した
	public static UnityAction<int> OnChangeScore;
	//ボスを倒した
	public static UnityAction OnDestroyBoss;
	//コンティニュー
	public static UnityAction OnContinue;
	//ゲームオーバー
	public static UnityAction OnGameOver;
	//自機やられ

	//singleton
	public static GameManager Instance { get; private set; }

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
		StdEnemyController.EnemyDestroy += this.EnemyDestroy;
		AbstractStageController.OnSpawn += this.OnSpawn;
		AbstractJikiControll.Death += this.Death;
	}

	void OnDisable(){
		//イベントを削除
		StdEnemyController.EnemyDestroy -= this.EnemyDestroy;
		AbstractStageController.OnSpawn -= this.OnSpawn;
		AbstractJikiControll.Death -= this.Death;
	}

	// Use this for initialization
	void Start () {
		Continue = maxContinue;
		Zanki = maxZanki;
	}
	

	/// <summary>
	/// 複数自機があるときに自機の配列を参照する.
	/// </summary>
	/// <param name="no">No.</param>
	public void SetJiki(int no){
		//自機配列の長さ以下なら配列を参照する
		//1のときはデフォルト？なのかな？
		if(no < jikis.Length){
			jiki = jikis[no];
		}
	}

	//イベント送受信部

	/// <summary>
	/// enemyDestroy
	/// 敵を破壊したときにボスフラグと得点を受け取る
	/// 得点を加算
	/// ボスフラグの判定
	/// </summary>
	/// <param name="BOSS">ボスかどうか<c>true</c> ボス</param>
	/// <param name="addscore">現在のスコア</param>
	public void EnemyDestroy(bool BOSS ,int ADDSCORE){
		//得点を加算してイベント配信
		SCORE += ADDSCORE;
		//OnChangeScore (SCORE);
		Debug.Log (SCORE);
		if (BOSS) {
			//ボスを倒しましたよ
			OnDestroyBoss();
		}
	}

	/// <summary>
	/// IsDemo
	/// デモンストレーションモード
	/// 入力を受け付けない
	/// 当たり判定をなくす
	/// スポーンアニメーションを再生しない
	/// </summary>
	/// <param name="">If set to <c>true</c> .</param>
	public void IsDemo(bool isDemo){
		DEMO = isDemo;
	}

	/// <summary>
	/// Raises the spawn event.
	/// 自機のインスタンスを生成
	/// デモモードがfalseなら自機のspawnアニメーション再生
	/// </summary>
	public void OnSpawn(){
		if (DEMO) {
			SpawnCommon ();
		} else {
			//デモモードfalse(通常はこっち)
			SpawnCommon();
			//ここにアニメ再生イベントを
			jikiInstance.SendMessage("SpawnJiki");
		}
	}

	private void Respawn(){
		Zanki--;
		jikiInstance = Instantiate(jiki.gameObject,new Vector3(0.0f,0.0f,-20.0f),Quaternion.identity);
		//スポーンアニメーション再生
		jikiInstance.SendMessage("SpawnJiki");	
	}

	/// <summary>
	/// プレイヤーやられ時の処理
	/// </summary>
	public void Death(){
		Debug.Log ("death");
		SpawnCommon ();
	}

	/// <summary>
	/// MaingamemanagerのOngameOverからsendmessageで呼ばれる
	/// </summary>
	public void ResetContinue(){
		Continue = maxContinue;
		Zanki = maxZanki;
	}

	//メソッド部

	/// <summary>
	/// スポーン処理の共通部分
	/// 残機確認
	/// ゲームオーバー時のコンティニュー遷移
	/// maingamecontrollerでUIシーン呼び出し
	/// 残機がある場合のリスポーン処理
	/// ステージまたぎの場合は
	/// </summary>
	public void SpawnCommon(){

		if (Zanki < 0) {
			//ゲームオーバー処理（コンティニュー）
			if (Continue > 0) {
				//コンティニュー遷移
				Continue--;
				//イベント配信
				OnContinue();
				//残機の数を戻す
				Zanki = maxZanki;
				Respawn ();

			} else {
				//ゲームオーバー処理
				//残機の数を戻す
				Zanki = maxZanki;
				OnGameOver();
			}
		} else {
			Respawn ();
		}
	}





}
