using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

	public Image fadePlane;
	public GameObject gameOverUI;


	Spawner spawner;
	Player player;

	void Start () {
		player = FindObjectOfType<Player> ();
		player.OnDeath += OnGameOver;
	}

	void Awake() {
		spawner = FindObjectOfType<Spawner> ();
	}

	void Update() {

	}


		
	void OnGameOver() {
		Cursor.visible = true;
		StartCoroutine(Fade (Color.clear, new Color(0,0,0,.95f),1));
		gameOverUI.SetActive (true);
	}


		
	IEnumerator Fade(Color from, Color to, float time) {
		float speed = 1 / time;
		float percent = 0;

		while (percent < 1) {
			percent += Time.deltaTime * speed;
			fadePlane.color = Color.Lerp(from,to,percent);
			yield return null;
		}
	}

	// UI Input
	public void StartNewGame() {
		SceneManager.LoadScene ("Game");
	}

	public void ReturnToMainMenu() {
		SceneManager.LoadScene ("Menu");
	}

	public void QuitGame(){
		Application.Quit();
	}
}