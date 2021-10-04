using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLevelComplete : MonoBehaviour
{
    public AudioClip openSfx;
    public TMPro.TMP_Text label;
    
    private string[] quips = 
    {
        "Unstabled the unstable horses from the unstable stable!",
        "Stop horsin' around!",
        "Somebody's poisoned the water hole!",
        "Neigh! We are but men. ROCK ON!",
        "Where did you put the horses?",
        "We need to order some more horses!",
        "Horsefly!",
        "We need more cowbell!",
        "Which one is Charlie?",
        "Is the stable unstable... or are you?",
        "Why did the horses eat the fireworks in the first place?",
        "We miss our NEIGHbors!",
        "Do two unstables make a stable?",
        "Grab life by the reins!",
        "Saddle up up and away!",
        "Where do they go?",
        "Nobody can say we don't feed them!",
        "I've fallen and I can't giddyup!",
        "Why are there horseshoes on the roof?",
        "Hay. Stop feeding the horses."
    };

    private void OnEnable()
    {
        GameManager.Instance.sfx.PlaySoundEffect(openSfx);
        GameManager.Instance.UnlockCurrentLevel();
        
        var levelName = SceneManager.GetActiveScene().name;
        
        if (int.TryParse(levelName.Substring(5, levelName.Length - 5), out var levelIndex))
        {
            label.text = quips[levelIndex - 1];
        }
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnHomeClicked()
    {
        Close();
        GameManager.Instance.ShowTitleScreen();
    }

    public void OnRestartClicked()
    {
        Close();
        GameManager.Instance.ReloadLevel();
    }

    public void OnNextLevelClicked()
    {
        Close();
        GameManager.Instance.WinLevel();
    }
}
