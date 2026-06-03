using UnityEngine;
using TMPro; 
public class GameManager : MonoBehaviour
{
   public int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreUI();
    }
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = " " + score.ToString();
        }
    }
}
