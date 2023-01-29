using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float startTimer;
    public static GameManager Instance;
    public GameObject WeaponDrop;
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

        //Spawna in objekt (WeaponDrop)
        if (Input.GetKeyDown(SpawnButton))
        {
            Instantiate(WeaponDrop, Camera.main.transform.position + new Vector3(Random.Range(-10, 10),15,10), Quaternion.identity);

        }
    }
}
