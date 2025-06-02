using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //[SerializeField] private float delay;
    [SerializeField] private bool lastScene;
    void Start()
    {
        //Tween.Delay(delay).OnComplete(target: this, target => target.LoadLevelFromStartGame());
    }
    public void LoadLevel()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("Level"));
        }
        else
        {
            SceneManager.LoadScene("Level1");
            PlayerPrefs.SetInt("Level", 1);
        }
    }
    public void NextLevel()
    {
        if (!lastScene)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(1);
            PlayerPrefs.SetInt("Level", 1);
        }
        
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
