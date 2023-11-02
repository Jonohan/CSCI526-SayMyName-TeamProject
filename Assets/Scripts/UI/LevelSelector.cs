using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;
    public Sprite unlockedLevelSprite;

    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        // Unlock the levels player reached
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
            {
                // The button can't be clicked unless you reach this level
                levelButtons[i].interactable = false;
            }
            else
            {
                // If the level is unlocked, change the button image
                levelButtons[i].interactable = true;
                levelButtons[i].GetComponent<Image>().sprite = unlockedLevelSprite; // This line changes the sprite

                int level = i + 1;
                levelButtons[i].onClick.AddListener(() => SelectLevel(level));
            }
        }
    }

    void SelectLevel(int level)
        {
            SceneManager.LoadScene("level " + level);

        }
    }



