using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject hiddenObjectIconHolder;     //reference to Icon Holder object
    [SerializeField] private GameObject hiddenObjectIconPrefab;     //reference to Icon prefab
    [SerializeField] private GameObject gameCompleteObj;            //reference to GameComplete panel
    [SerializeField] private Text timerText;                        //reference to time text

    private List<GameObject> hiddenObjectIconList;                  //list to store Icons of active hidden objects

    public GameObject GameCompleteObj { get => gameCompleteObj; }   //getter
    public Text TimerText { get => timerText; }                     //getter

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        hiddenObjectIconList = new List<GameObject>();              //initialize the list
    }

    /// <summary>
    /// This method creates the icon for all the active hidden objects and populate them in hiddenObjectIconHolder
    /// </summary>
    /// <param name="hiddenObjectData">Data of active hidden objects</param>
    public void PopulateHiddenObjectIcons(List<HiddenObjectData> hiddenObjectData)
    {
        hiddenObjectIconList.Clear();                               //we clear the last data from the list
        for (int i = 0; i < hiddenObjectData.Count; i++)            //loop through hiddenObjectData count
        {
                                                                    //create the icon object and set its parent
            GameObject icon = Instantiate(hiddenObjectIconPrefab, hiddenObjectIconHolder.transform);
            icon.name = hiddenObjectData[i].hiddenObj.name;         //set the icon name similar to object name, use when tracking icon for selected object
            Image childImg = icon.transform.GetChild(0).GetComponent<Image>();  //get the image component from Icon object image child
            Text childText = icon.transform.GetChild(1).GetComponent<Text>();   //get the text component from Icon object text child

            childImg.sprite = hiddenObjectData[i].hiddenObj.GetComponent<SpriteRenderer>().sprite; //set the sprite
            childText.text = hiddenObjectData[i].name;                          //set the text
            hiddenObjectIconList.Add(icon);                                     //add the icon to the list
        }
    }

    /// <summary>
    /// Method called when the player tap on active hidden object
    /// </summary>
    /// <param name="index">Name of hidden object</param>
    public void CheckSelectedHiddenObject(string index)
    {
        for (int i = 0; i < hiddenObjectIconList.Count; i++)                    //loop through the list
        {
            if (index == hiddenObjectIconList[i].name)                          //check if index is same as name [our name is a number]
            {
                hiddenObjectIconList[i].SetActive(false);                       //deactivate the icon
                break;                                                          //break from the loop
            }
        }
    }

    public void NextButton()                                                    //Method called when NextButton is clicked
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);       //load the scene
    }

    public void HintButton()
    {
        //TODO: Using Coroutine is not recommended, try using TweenEngine. Eg:- DOtween, iTween
        StartCoroutine(LevelManager.instance.HintObject());
    }
}
