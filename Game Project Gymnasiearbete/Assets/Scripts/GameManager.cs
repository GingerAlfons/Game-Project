using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float startTimer;
    public float spawnTimer = 0f;
    public static GameManager Instance;
    public WeaponDrop weaponDrop;
    public Weapon[] weaponLibrary = new Weapon[0];
    public KeyCode SpawnButton;
    public Vector3 WeaponSpawnCoords;


    private void Awake()
    {
        if (Instance != null)
        {
            Instance.RoundStart();
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        RoundStart();
    }

    public void RoundStart()
    {
        startTimer = 3f;
    }

    private void Update()
    {
        //Timern för freeze perioden i början av spelet
        if (startTimer > 0)
        {
            startTimer -= Time.deltaTime;
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

    void SpawnWeaponDrop()
    {
        //Ger spawn timern av objektet ett random värde mellan 5 - 10 sekunder
        spawnTimer = Random.Range(5, 10);
        WeaponDrop clone = Instantiate(weaponDrop, Camera.main.transform.position + new Vector3(Random.Range(-10, 10), 15, 10), Quaternion.identity);
        clone.weaponValue = weaponLibrary[Random.Range(0, weaponLibrary.Length)];
        Debug.Log("Objekt spawnas! " + "Tid till nästa objekt " + spawnTimer);
    }
}
