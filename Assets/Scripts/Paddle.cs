using UnityEngine;
using UnityEngine.UI;
using System.Collections;



    public class Paddle : MonoBehaviour
    {
        public float MaxTimeToClick { get { return _maxTimeToClick; } set { _maxTimeToClick = value; } }  //zamiast pokazywać i umożliwiać modyfikowanie zmiennych publicznie używamy geta i seta
        public float MinTimeToClick { get { return _minTimeToClick; } set { _minTimeToClick = value; } }

        //deklaracje zmiennych chyba w miarę zrozumiałe    

        public float paddleSpeed = 1f;
        public float paddleSpeeda = 1f;
        public float paddleSpeedax = 1f;
        private float xPos;
        private bool currentPlatformAndroid = false;

        private float _maxTimeToClick = 0.60f;
        private float _minTimeToClick = 0.05f;
        private float _minCurrentTime;
        private float _maxCurrentTime;
        private float MinOffset = -8f;
        private float MaxOffset = 8f;
        private float cPosition;
        private bool gamePaused = false;
	public static bool touch=true;
	public static bool accel=false;




	public GameObject score;
	public GameObject menu;
	public GameObject options;
	public Button exitbutton;
	public Button optionsbutton;
	public Button savebutton;
	public Slider controlslider;
	public Button scorebutton;
	public Text nametext;
	public Text scoretext;

    private Rigidbody rb;


    //private Vector3 translateVector = new Vector3(10.0f, 0.0f, 0.0f); //to na razie nie używane

        private Vector3 playerPos = new Vector3(0, -9.5f, 0);

    /** Standardowa metoda Awake z klasy MonoBehaviour, w naszym przypadku zawiera kod mający na celu okreslenie dla jakiego systemu kompilujemy kod
       * #if dyrektywa postprocesora (to oznacza #) rozpoczyna instrukcję warunkową if wykonywaną w trakcie kompilacji, a czytaną przez postrocesor jeszcze przed przystąpieniem do kompilacji
       * #else kolejna dyrektywa postprocesora robiąca dokładnie to samo co zwykły else wykonywana jak wszystkie inne dyrektywy postprocesora
       * #endif po prostu kończy blok dyrektyw postprocesora dla kompilacji warunkowej
       **/ 


        private void Awake()                                        //Awake metoda uruchamiana nawet jeśli skrypt nie jest włączony (w sensie jako komponent może nie być nawet przypisany do obiektu)
    {                                                               //najlepsza metoda do referencji między skryptami i incjalizacji
#if UNITY_ANDROID                                                   //instrukcja warunkowa if...else sprawdza czy kompilujemy kod dla systemu Android 
        currentPlatformAndroid = true;                              //jeśli tak zmienia wartość zmiennej  currentPlatformAndroid na true

#else
        currentPlatformAndroid = false;                             //jeśli nie upewnia sie ze zmienna currentPlatformAndroid ma wartość false

#endif
        }

        private void Start()                                        //Start jest uruchamiany tylko jeśli skrypt jest włączony, uruchamiana przy starcie aplikacji
        {
            if (currentPlatformAndroid == true)                     //sprawdzenie wartości zmiennej currentPlatformAndroid
        {                                                           //jeśli ma wartość true wypisujemy do konsoli "Android" jeśli nie wypisujemy "Other system"
                Debug.Log("Android");


           
        }
            else
            {
                Debug.Log("Other system");

            }
            transform.position = new Vector3(0f, -9.5f, 0f);        //ustawiamy pozycję naszej paletki (x,y,z)


		exitbutton.onClick.AddListener (OnClick_exit);
		optionsbutton.onClick.AddListener (OnClick_options);
		savebutton.onClick.AddListener (OnClick_save);
		scorebutton.onClick.AddListener (OnClick_score);

        }
        void Update()                                               //Metoda Update tutaj wprowadzamy wszelkie zmiany wykonywane po starcie aplikacji
        {

        

        
            if (currentPlatformAndroid == true)                     //jeśli kod jest skompilowany na system Android to wykonujemy metodę zawierającą sterowanie dla systemów mobilnych
             {
            if (gamePaused == false)
            {


               
				if (touch==true) {
					TouchMove ();
				}
				if (accel==true) {
					Accelerator();
				}

                
            }

        }
        else
        {
            if (gamePaused == false)
            {
                MoveAxis();
            }

             
                                            //jeśli kod nie jest skompilowany na Androida uruchamiamy metodę MoveAxis zawierającą sterowanie dla systemów desktopowych
        }



        /* if (Input.touchCount == 1)
         {

             // Move object across XY plane
             transform.Translate(Input.touches[0].Position.x * paddleSpeed, 0, 0);
         }*/

        



        Exit();

			Pause ();

        }


         private bool Pause()                                               //metoda pauzy w grze po przeczytaniu komentarzy metody Exit chyba w miarę prosta
    {
		
		if (Input.GetKeyDown (KeyCode.Menu) || Input.GetKeyDown (KeyCode.Space)) {
			if (gamePaused) {
				
				Time.timeScale = 1;
				gamePaused = false;
				menu.SetActive (false);
				options.SetActive (false);
                
			} else {
				
				Time.timeScale = 0;
				gamePaused = true;
				menu.SetActive (true);


			}

		}
        return gamePaused;

    }


        private void Exit()
    {
        //EXIT
        /*
            Jeśli użytkownik kliknie przycisk wyjścia to rozpoczynamy wykonywanie bloku kodu poniżej tego komentarza
            tzn sprawdzamy czy czas od kliknięcia przycisku wyjścia mieści się w zdefiniowanych na początku skryptu widełkach
            tylko, że nie możemy sprawdzić tego wprost no bo jak tylko sprawdzamy czy czas od włączenia gry + nasze widełki się w nich mieści
            tak wiem macie w głowie takie zaraz,zaraz ale dlaczego ten if jest tak głupio zbudowany a no dlatego, że nie chcemy aby przy jednym kliknięciu (często przypadkowym)
            nasza gra się wyłączała bo przecież szczególnie na telefonie jest to niesamowicie irytujące (np został ostatni klocek na ostatnim lvlu i przypadkowo przycisk wyjścia)
            i tu paczajta na geniuszów ten wewnętrzny if nigdy nie wykona się za pierwszym razem (bo jak jeśli nie ma wartości) więc jedynie pobieramy wartości w ostatniej linijce
            zewnętrznej instrukcji warunkowej a dopiero gdy ponownie naciśniemy przycisk wyjścia (zaczniemy zewnętrzną instrukcję jeszcze raz z pobranymi już danymi)
            wykonujemy wewnętrzneną instrukcję czyli czyścimy zmienne, tak na wszelki wypadek i zamykamy aplikację
             */

        if (Input.GetKeyDown(KeyCode.Escape))                                               //Czekamy na kliknięcie przyciku wyjścia btw. bo to powtarzam a co to jest - jest to standardowy przycik wyjścia w danym systemie:
        {                                                                                   //w Widows klawisz ESC, Android ta śmieszna strzałka czy jakkolwiek macie to oznaczone jeden z dwóch skrajnych przycisków pod ekranem 
            if (Time.time > _minCurrentTime && Time.time < _maxCurrentTime)                 //sprawdzamy czy aktualny czas działania aplikacji mieści się w naszych widełkach
            {

                _minCurrentTime = 0;                                                    //resetujemy zmienne, na wszelki wypadek jakby coś poszło nie tak ("przezorny zawsze ubezpieczony" itp.)
                _maxCurrentTime = 0;

                Application.Quit();                                                     //wychodzimy z aplikacji
            }

            _minCurrentTime = Time.time + MinTimeToClick;                               //przypisujemy wartość
            _maxCurrentTime = Time.time + MaxTimeToClick;

        }
    }       

       private void TouchMove()                                                                    //sterowanie dotykiem
        {
        


            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);                                            //bierzemy tylko pierwsze dotknięcie, jeśli jest ich więcej w tym samym czasie (żeby nie było dziwnych sytuacji)
                float middle = Screen.width / 2;                                            //tworzymy nową zmienną i przypisujemy jej wartość wynoszącą połowę szerokości ekranu w pikselach


                if (touch.position.x < middle)                                              //sprawdzamy czy dotknięcie było z lewej czy z prawej strony (od środka) ekranu  i wykonujemy odpowiednią metodę
            {
                    MoveLeft();
                }
                if (touch.position.x > middle)
                {
                    MoveRight();
                }

            }

        }

    private void MoveAxis()                                              //sterowanie desktopowe 
    {
        xPos = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeedax);  //przypisujemy zmiennej wartość wynoszącą aktualnoą pozycję paletki + obsługę stadardowego wejścia osi czyli strzałek dla klawiatur i oś x dla jostików, padów
        playerPos = new Vector3(Mathf.Clamp(xPos, -8f, 8f), -9.5f, 0f);             //przypisujemy zmiennej wartość obiektowi klasy Vector3 przymującemu takie wartości(x,y,z) wartości stałe dla osi y i z
                                                                                    //oraz metodę Mathf.Clamp sprawdzającą czy dla podanego argumentu warości mieszą się w podanym przedziale (argument, min, max)
        transform.position = playerPos;                                             //zmieniamy pozycję paletki
    }

    private void MoveLeft()                                             // Ruch w lewo - to jeszcze będe optymalizował wię narazie nie komentuje
    {
        cPosition = transform.position.x;

        if (cPosition >= MinOffset)
        {
            transform.Translate(-paddleSpeed, 0f, 0f);
        }




    }
    private void MoveRight()                                             // Ruch w prawo
    {


        cPosition = transform.position.x;

        if (cPosition <= MaxOffset)
        {
            transform.Translate(paddleSpeed, 0f, 0f);
        }
    }

    private void Accelerator()                                                                //sterowanie akceleratorem
    {
        xPos = transform.position.x + (Input.acceleration.x * paddleSpeeda);  
        playerPos = new Vector3(Mathf.Clamp(xPos, MinOffset, MaxOffset), -9.5f, 0f);
        transform.position = playerPos;
    }

    
    
           

    //poniżej testy


    /*
    public void MoveLeft()                                                              //to też się nie rusza
     {
         rb.velocity=new Vector3(Mathf.Clamp(paddleSpeed, -8f, 8f), 0f, 0f);

     }
         public void MoveRight()
     {
         rb.velocity = new Vector3(Mathf.Clamp(-paddleSpeed, -8f, 8f), 0f, 0f);
     }
}/*

    public void MoveLeft()                                           //tu nie wiem co się odjaniepawla nie porusza się paletka
     {
     newPosition = rb.velocity.x;
     if (newPosition >= MinOffset && newPosition <= MaxOffset)
     {
         transform.Translate(-paddleSpeed, 0f, 0f);
     }
     


     }
*/




    /*public void MoveLeft()                                              //te działają ale nie są ograniczone btw nie ruszać osi x bo crashuje a tak działa
    {

        transform.Translate(-paddleSpeed, 0f, 0f);
    }
        public void MoveRight()
        {


            transform.Translate(paddleSpeed, 0f, 0f);
        }*/


	void OnClick_save(){
		if (controlslider.value == 1) {
			touch = true;
			accel = false;
		}
		if (controlslider.value == 0) {
			touch = false;
			accel = true;
		}
		if (controlslider.value == 2) {
			touch = false;
			accel = false;
			MoveAxis ();
		}
		options.SetActive (false);
		menu.SetActive (true);
		score.SetActive (false);
	}

	void OnClick_options(){
		options.SetActive (true);
		menu.SetActive (false);
		score.SetActive (false);

	}

	void OnClick_score(){
		options.SetActive (false);
		menu.SetActive (false);
		score.SetActive (true);
		GM.playername=PlayerPrefs.GetString ("Player Name");
		nametext.text = "Playername: " + GM.playername;
		GM.score=PlayerPrefs.GetInt ("Player Score");
		scoretext.text += GM.score;
	}


	void OnClick_exit(){
			Application.Quit(); 
	}
}
