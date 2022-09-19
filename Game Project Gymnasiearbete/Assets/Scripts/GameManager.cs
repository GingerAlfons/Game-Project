using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float startTimer;
    public static GameManager Instance;

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
        if (startTimer > 0)
        {
            startTimer -= Time.deltaTime;
        }
    }
}
