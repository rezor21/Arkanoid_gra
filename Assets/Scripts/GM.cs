using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


    public class GM : MonoBehaviour
    {


        public int bricks = 20;
        public float resetDelay = 1f;
        public Text livesText;
        public Text timeTextW;
        public Text timeTextL;
        public GameObject gameOver;
        public GameObject youWon;
        public GameObject bricksPrefab;
	public GameObject paddle;
	public GameObject lvl1;
	public GameObject lvl2;
	public GameObject lvl3;
        public GameObject deathParticles;
        public static GM instance = null;
        private static float playTime=0f;
        private float cTime = 0f;
	public static int lvl=1;
	public bool paddle_touch = false;
	public static int score=0;
	public static int lives_global=3;
	public int lives = lives_global;
	public static string playername="rezor";





        private GameObject clonePaddle;

        // Use this for initialization
        void Awake()
        {
            
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
		lives = lives_global;
		livesText.text = "Lives: " + lives;
            Setup(lvl);


        }


        
            



	public void Setup(int level)
        {
            clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity) as GameObject;
            Instantiate(bricksPrefab, transform.position, Quaternion.identity);
		if (level == 1) 
		{
			Instantiate(lvl1, transform.position, Quaternion.identity);
		}

		if (level == 2) 
		{
			Instantiate(lvl2, transform.position, Quaternion.identity);
		}

		if (level == 3) 
		{
			Instantiate(lvl3, transform.position, Quaternion.identity);
		}

		if (level > 3) 
		{
			Instantiate(lvl1, transform.position, Quaternion.identity);
			lvl = 1;
		}
        }

        void CheckGameOver()
        {
		if (bricks < 1 && lvl<4)
		{
			cTime = Time.time;
			cTime = cTime - playTime;
			timeTextW.text = "Next level \r\n Score: " + score + "  \r\n Time: " + cTime + "s";
			youWon.SetActive(true);
			lives_global = lives;
			Time.timeScale = .25f;
			playTime = Time.time;
			Invoke("Reset", resetDelay);
			lvl++;

		}

		if (bricks < 1 && lvl==4)
            {
                cTime = Time.time;
			timeTextW.text = "YOU WON \r\n Score: " + score + "  \r\n Time: " + cTime + "s";
			PlayerPrefs.SetString ("Player Name", playername);
			PlayerPrefs.Save ();
			PlayerPrefs.SetInt ("Player Score", score);
			PlayerPrefs.Save ();
                youWon.SetActive(true);
			lives_global = 3;
                Time.timeScale = .25f;
                
                Invoke("Reset", resetDelay);
			++lvl;

            }

            if (lives < 1)
            {
				lives_global = 3;
                cTime = Time.time;
                cTime = cTime - playTime;
			timeTextL.text = "GAME OVER \r\n Score: " + score + "  \r\n Time: " + cTime + "s";
			PlayerPrefs.SetString ("Player Name", playername);
			PlayerPrefs.Save ();
			PlayerPrefs.SetInt ("Player Score", score);
			PlayerPrefs.Save ();
                gameOver.SetActive(true);
                Time.timeScale = .25f;
				score = 0;
                playTime = Time.time;
				lvl = 1;
                Invoke("Reset", resetDelay);


            }
			
			

        }

        void Reset()
        {
		
            Time.timeScale = 1f;
            Application.LoadLevel(Application.loadedLevel);


    }

        public void LoseLife()
        {
		
		lives--;
            livesText.text = "Lives: " + lives;
            Instantiate(deathParticles, clonePaddle.transform.position, Quaternion.identity);
            Destroy(clonePaddle);
            Invoke("SetupPaddle", resetDelay);
            CheckGameOver();
        }

        void SetupPaddle()
        {
            clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity) as GameObject;
        }

        public void DestroyBrick()
        {
            bricks--;
            CheckGameOver();
			score++;
        }

       
    }
