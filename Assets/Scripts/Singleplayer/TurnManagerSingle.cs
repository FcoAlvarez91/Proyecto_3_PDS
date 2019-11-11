using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;

public class TurnManagerSingle: MonoBehaviour
{

    public bool newTurnBool;
    public int turn;
    public GameObject targetText;
    public GameObject enemies;
    public GameObject scrolls;
    public ScrollHolderSingle scrollHolderSingle;
    public static Dictionary<string, object> singleDeck;
    private Dictionary<string, object> allDeck;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    public DatabaseReference reference;

    // Start is called before the first frame update
    void Start()
    {
        turn = 0;
        targetText.GetComponent<TextMeshProUGUI>().text = "CHOOSE A SCROLL";
        GetComponent<ToggleButtonsSingle>().predeactivateButtons();

        InitializeFirebase();
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://four-mages.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.Child("decks").Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("Deck failed.");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Getting deck.");
                DataSnapshot snapshot = task.Result;
                allDeck = (Dictionary<string, object>)snapshot.Value;
                newTurnBool = true;
            }
        });
        
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (newTurnBool)
        {
            if (turn == 0)
            {
                singleDeck = new Dictionary<string, object>();
                int i = 0;
                foreach (var unit in allDeck)
                {
                    var scroll = (Dictionary<string, object>)unit.Value;
                    if (scroll["inDeck"].ToString().Equals("1"))
                    {
                        Debug.Log("Adding scroll " + scroll["id"].ToString() + " under scroll" + i);
                        singleDeck.Add("scroll" + i, scroll);
                        i++;
                    }
                }
            }
            DrawScrollsSingle.scrollsInDeck = singleDeck.Count;
            startEnemyTurn();
            newTurnBool = false;
        }

        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                // Quit the application
                SceneManager.LoadScene("Menu");
            }
        }
    }

    public void newTurn()
    {
        targetText.GetComponent<TextMeshProUGUI>().text = "CHOOSE A SCROLL";

        if (turn == 0)
        {
            scrolls.GetComponent<DrawScrollsSingle>().pullFromDeck();
        }
        else
        {
            scrolls.GetComponent<DrawScrollsSingle>().drawScroll();
        }

        turn++;
        scrollHolderSingle.played = false;
        GetComponent<ToggleButtonsSingle>().predeactivateButtons();
    }

    public void startEnemyTurn()
    {
        enemies.GetComponent<EnemyManagerSingle>().startTurn();
        targetText.GetComponent<TextMeshProUGUI>().text = "ENEMY TURN";
        enemies.GetComponent<EnemyManagerSingle>().enemyTurn();
        enemies.GetComponent<EnemyManagerSingle>().resetTurn();
        endEnemyTurn();
    }

    public void endEnemyTurn()
    {
        newTurn();
    }

    public void setNewTurnBool()
    {
        newTurnBool = true;
    }
}
