using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Std enemy controller.
/// 標準敵コントローラー
/// 敵のスクリプトはこのクラスを継承する
/// SceanControllerというgameObjectに自分をアタッチします
/// lifeTimer　　　一定時間経つと自動で消滅します　時間の設定に注意
/// sequenceChar　敵の動きはこの関数をオーバーライドしてください　コルーチンです
/// ボスフラグがtrueの場合　ダメージが0になるかタイマーが0になるとSceanControllにStageClearメッセージを送信します
/// </summary>
public class StdEnemyController : MonoBehaviour {

	public int HP = 100;
	public int ADDSCORE = 100;
	public float lifeTime = 200;
	public int damage = 1000;
	public bool BOSS = false;

	public Transform bullet;
	public GameObject cannon;

	//配信イベント
	public static UnityAction<bool,int> EnemyDestroy;

	void OnEnable(){
		//受信イベント
	
	}

	void OnDisable(){
		//イベントを削除

	}

	void Awake(){
		Transform p = GameObject.Find("SceneController").transform;
		transform.parent = p;
	}	
	// Use this for initialization
	void Start () {
		
	}


	///イベント送受信部

	/// <summary>
	/// Hit
	/// プレイヤー機の球とコリジョンが発生したときのイベント実行部
	/// HPからbulletDamageをマイナスする
	/// hiteffectを出す
	/// HPが0を下回ったらデストロイ
	/// HPが0を下回ったら破壊イベントとボスフラグと得点を配信
	/// 破壊シーケンスを再生
	/// </summary>
	public void Hit(int bulletDamage){
		HP -= bulletDamage;
		Debug.Log (HP);
		if (HP < 0) {
			EnemyDestroy (BOSS, ADDSCORE);
			Destroy (gameObject);
		}
	}

	//この関数はテスト中
	void OnCollisionEnter(Collision collision){
		Debug.Log ("col");
	}


}
