using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float startTimer;
    public float roundTime;
    public float spawnTimer = 0f;
    public static GameManager Instance;
    public WeaponDrop weaponDrop;
    public Weapon[] weaponLibrary = new Weapon[0];
    public KeyCode SpawnButton;
    public Vector3 WeaponSpawnCoords;
    public GameObject redPlayer;
    public GameObject greenPlayer;
    public float redWins = 0f;
    public float greenWins = 0f;

    [SerializeField]
    private TextMeshProUGUI timer;
    [SerializeField]
    private TextMeshProUGUI redWinsText;
    [SerializeField]
    private TextMeshProUGUI greenWinsText;

    private void Awake()
    {
        if (Instance != null)
        {
            Instance.RoundStart();
            Instance.redPlayer = redPlayer;
            Instance.greenPlayer = greenPlayer;
            Instance.timer = timer;
            Instance.redWinsText = redWinsText;
            Instance.greenWinsText = greenWinsText;
            Instance.ScoreText();
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        RoundStart();
        ScoreText();
    }

    public void RoundStart()
    {
        startTimer = 3f;
        roundTime = 0f;
    }

    private void Update()
    {
        //Timern för freeze perioden i början av spelet
        if (startTimer > 0)
        {
            countDownTimer(startTimer);
            startTimer -= Time.deltaTime;
        }
        else
        {
            //En timer som håller koll på hur länge den aktiva rundan har pågått
            roundTime += Time.deltaTime;
            countDownTimer(roundTime);
        }

        //Spawna in objekt manuellt (WeaponDrop)
        if (Input.GetKeyDown(SpawnButton))
        {
            SpawnWeaponDrop();
        }

        if (GameManager.Instance.startTimer <= 0)
        {
            //Spawnar in objekt automatiskt (Weapondrop)
            if (spawnTimer <= 0)
            {
                SpawnWeaponDrop();

            }
            else
            {
                spawnTimer -= Time.deltaTime;
            }
        }
    }

    private void countDownTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timer.text = string.Format("{00:00} : {01:00}", minutes, seconds);
    }

    private void ScoreText()
    {
        redWinsText.text = string.Format("{0}", redWins);
        greenWinsText.text = string.Format("{0}", greenWins);
    }

    void SpawnWeaponDrop()
    {
        //Ger spawn timern av objektet ett random värde mellan 5 - 10 sekunder
        spawnTimer = Random.Range(5, 10);
        WeaponDrop clone = Instantiate(weaponDrop, Camera.main.transform.position + new Vector3(Random.Range(-10, 10), 15, 10), Quaternion.identity);
        clone.weaponValue = weaponLibrary[Random.Range(0, weaponLibrary.Length)];
        Debug.Log("Objekt spawnas! " + "Tid till nästa objekt " + spawnTimer);
    }

    public void EndGame(GameObject targetedObject)
    {
        //Stänger av Spelaren som dör
        targetedObject.SetActive(false);

        //Kollar vilken spelare som vann och poängen för den spelaren
        if (targetedObject == redPlayer)
        {
            greenWins++;
            if (greenWins == 3)
            {
                //Laddar en Win Screen med ett meddelande att "Green Wins!"
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else if (targetedObject == greenPlayer)
        {
            redWins++;
            if (redWins == 3)
            {
                //Laddar en Win Screen med ett meddelande att "Red Wins!"
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
