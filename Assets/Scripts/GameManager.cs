using UnityEngine;

public class GameManager : MonoBehaviour
{
   public int score = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddScore(int points)
    {
        score += points;
    }
}
