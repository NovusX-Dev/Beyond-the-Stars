using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;


    private void Awake()
    {

    }

    void Start()
    {
        scoreText.text = 0.ToString();
    }

    void Update()
    {
        
    }

    public void UpdateScoreUI(int score)
    {
        scoreText.text = score.ToString();
    }
}
