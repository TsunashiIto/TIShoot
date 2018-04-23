using System.Collections;
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
	}

	void OnDisable(){
		
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
		SceneManager.UnloadSceneAsync (2);
	}

}
