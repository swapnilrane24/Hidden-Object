using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private float timeLimit = 0;                       //Total time for single game player gets
    [SerializeField] private int maxHiddenObjectToFound = 6;            //maximum hidden objects available in the scene
    [SerializeField] private ObjectHolder objectHolderPrefab;           //ObjectHolderPrefab contains list of all the hiddenObjects available in it

    [HideInInspector] public GameStatus gameStatus = GameStatus.NEXT;   //enum to keep track of game status
    private List<HiddenObjectData> activeHiddenObjectList;              //list hidden objects which are marked as hidden from the above list
    private float currentTime;                                          //float to keep track of time remaining
    private int totalHiddenObjectsFound = 0;                            //int to keep track of hidden objects found
    private TimeSpan time;                                              //variable to help convert currentTime into time format
    private RaycastHit2D hit;
    private Vector3 pos;                                                //hold Mouse Tap position converted to WorldPoint

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
    }

    void Start()
    {
        activeHiddenObjectList = new List<HiddenObjectData>();          //we initialize the list
        AssignHiddenObjects();                                  
    }

    void AssignHiddenObjects()  //Method select objects from the hiddenobjects list which should be hidden
    {
        ObjectHolder objectHolder = Instantiate(objectHolderPrefab, Vector3.zero, Quaternion.identity);
        totalHiddenObjectsFound = 0;                                        //set it to 0 by default
        activeHiddenObjectList.Clear();                                     //clear the list by default
        gameStatus = GameStatus.PLAYING;                                    //set game status to playing
        UIManager.instance.TimerText.text = "" + timeLimit;                 //set the timer text of UIManager
        currentTime = timeLimit;                                            //set the currentTime to timeLimit

        for (int i = 0; i < objectHolder.HiddenObjectList.Count; i++)       //loop through all the hiddenObjects in the hiddenObjectList
        {
            //deacivate collider, as we only want selected hidden objects to have collider active
            objectHolder.HiddenObjectList[i].hiddenObj.GetComponent<Collider2D>().enabled = false; 
        }

        int k = 0; //int to keep count
        while (k < maxHiddenObjectToFound) //we check while k is less that maxHiddenObjectToFound, keep looping
        {
            //we randomly select any number between 0 to hiddenObjectList.Count
            int randomNo = UnityEngine.Random.Range(0, objectHolder.HiddenObjectList.Count);
            //then we check is the makeHidden bool of that hiddenObject is false
            if (!objectHolder.HiddenObjectList[randomNo].makeHidden)
            {
                //we are setting the object name similar to index, because we are going to use index to identify the tapped object
                //and this index will help us to deactive the hidden object icon from the UI
                objectHolder.HiddenObjectList[randomNo].hiddenObj.name = "" + k;    //set their name to index

                objectHolder.HiddenObjectList[randomNo].makeHidden = true;          //if false, then we set it to true
                                                                                    //activate its collider, so we can detect it on tap
                objectHolder.HiddenObjectList[randomNo].hiddenObj.GetComponent<Collider2D>().enabled = true;
                activeHiddenObjectList.Add(objectHolder.HiddenObjectList[randomNo]);//add the hidden object to the activeHiddenObjectList
                k++;                                                                //and increase the k
            }
        }

        UIManager.instance.PopulateHiddenObjectIcons(activeHiddenObjectList);   //send the activeHiddenObjectList to UIManager
        gameStatus = GameStatus.PLAYING;                                        //set gamestatus to Playing
    }

    private void Update()
    {
        if (gameStatus == GameStatus.PLAYING)                               //check if gamestatus is Playing
        {
            if (Input.GetMouseButtonDown(0))                                //check for left mouse tap
            {
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //get the position of mouse tap and conver it to WorldPoint
                hit = Physics2D.Raycast(pos, Vector2.zero);                 //create a Raycast hit from mouse tap position
                if (hit && hit.collider != null)                            //check if hit and collider is not null
                {
                    hit.collider.gameObject.SetActive(false);               //deactivate the hit object
                    //Remember we renamed all our object to their respective Index, we did it for UIManager
                    UIManager.instance.CheckSelectedHiddenObject(hit.collider.gameObject.name); //send the name of hit object to UIManager

                    for (int i = 0; i < activeHiddenObjectList.Count; i++)
                    {
                        if (activeHiddenObjectList[i].hiddenObj.name == hit.collider.gameObject.name)
                        {
                            activeHiddenObjectList.RemoveAt(i);
                            break;
                        }
                    }

                    totalHiddenObjectsFound++;                              //increase totalHiddenObjectsFound count

                    //check if totalHiddenObjectsFound is more or equal to maxHiddenObjectToFound
                    if (totalHiddenObjectsFound >= maxHiddenObjectToFound)  
                    {
                        Debug.Log("You won the game");                      //if yes then we have won the game
                        UIManager.instance.GameCompleteObj.SetActive(true); //activate GameComplete panel
                        gameStatus = GameStatus.NEXT;                       //set gamestatus to Next
                    }
                }
            }

            currentTime -= Time.deltaTime;  //as long as gamestatus i in playing, we keep reducing currentTime by Time.deltaTime

            time = TimeSpan.FromSeconds(currentTime);                       //set the time value
            UIManager.instance.TimerText.text = time.ToString("mm':'ss");   //convert time to Time format
            if (currentTime <= 0)                                           //if currentTime is less or equal to zero
            {
                Debug.Log("Time Up");                                       //if yes then we have lost the game
                UIManager.instance.GameCompleteObj.SetActive(true);         //activate GameComplete panel
                gameStatus = GameStatus.NEXT;                               //set gamestatus to Next
            }
        }
    }

    public IEnumerator HintObject() //Method called by HintButton of UIManager
    {
        int randomValue = UnityEngine.Random.Range(0, activeHiddenObjectList.Count);
        Vector3 originalScale = activeHiddenObjectList[randomValue].hiddenObj.transform.localScale;
        activeHiddenObjectList[randomValue].hiddenObj.transform.localScale = originalScale * 1.25f;
        yield return new WaitForSeconds(0.25f);
        activeHiddenObjectList[randomValue].hiddenObj.transform.localScale = originalScale;
    }
}

[System.Serializable]
public class HiddenObjectData
{
    public string name;
    public GameObject hiddenObj;
    public bool makeHidden = false;
}

public enum GameStatus
{
    PLAYING,
    NEXT
}