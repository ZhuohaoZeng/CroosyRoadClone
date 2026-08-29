using UnityEngine;

public enum GameState
{
    Ready,
    Playing,
    GameOver,
    Completed
}

public class GameManager: MonoBehaviour
{
    bool isLoaded;
    private void Awake()
    {
        if(isLoaded)
        {
            Destroy(this.gameObject);
        }
        else
        {
            isLoaded = true;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    
}


