using UnityEngine;

public class MainHud : MonoBehaviour
{
    public TMPro.TMP_Text nameLabel;
    
    void Start()
    {
        var settings = GameObject.Find("LevelSettings").GetComponent<LevelSettings>();
        nameLabel.text = settings.name;
    }
}
