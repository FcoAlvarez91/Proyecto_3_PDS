using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    private PlayerManager playerManager;
    public TextMeshProUGUI targetText;
    public EnemyManager enemies;
    public DrawScrolls scrolls;

    public GameObject enemiesGO;

    public bool turnEnded;
    public static bool change;
    private bool change2;
    private bool playerTurn;

    public GameObject background;
    public GameObject ledger;
    public GameObject correspondence;

    public string playerID;

    public TargetManager fireMage;
    public TargetManager waterMage;
    public TargetManager earthMage;
    public TargetManager windMage;

    public TargetManager enemy1;
    public TargetManager enemy2;
    public TargetManager enemy3;

    public static Firebase.Auth.FirebaseAuth auth;
    public static DatabaseReference reference;

    public static Dictionary<string, object> data;

    // Start is called before the first frame update
    void Start()
    {
        targetText.text = "LOADING";
        background.SetActive(true);
        GetComponent<ToggleButtons>().predeactivateButtons();
        playerManager = GetComponent<PlayerManager>();

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://four-mages.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        loadDB();
        StartCoroutine(WaitForStart());
        //InvokeRepeating("gameChangeLoadDB", 5.0f, 10f);
        //InvokeRepeating("gameChangeStart", 7.0f, 10f);
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(3);
        gameStart();
    }

    public void loadDB()
    {
        /* var dataOut = new Dictionary<string, object>();
        dataOut["email"] = auth.CurrentUser.Email;
        dataOut["push"] = true;
        Debug.Log("Here goes nothing...");
        var function = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable("getDisplayName");
        function.CallAsync(dataOut).ContinueWith((task) =>
        {
            if(task.IsCompleted && task.Result.Data != null)
            {
                playerEmail = task.Result.Data.ToString().Replace('.',',');
            }
        }); */
        playerID = auth.CurrentUser.UserId;
        reference.Child("games").Child(GameManager.currentGame).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                data = (Dictionary<string, object>)snapshot.Value;
                Debug.Log("It worked.");
            }
        });
    }

    public void gameStart()
    {
        GetComponent<ToggleButtons>().predeactivateButtons();
        background.SetActive(false);
        enemies.getGrid();
        Dictionary<string, object> players = (Dictionary<string, object>)data["players"];
        foreach (var player in players)
        {
            string pK = player.Key;
            if (playerID.Length != pK.Length)
            {
                Debug.Log("Not length");
            }
            if (Equals(playerID, pK))
            {
                Debug.Log("element: " + player.Value.ToString());
                GetComponent<PlayerManager>().setElement(player.Value.ToString());
                Dictionary<string, object> decks = (Dictionary<string, object>)data["decks"];
                GameManager.deck = (Dictionary<string, object>)decks[player.Value.ToString()];
                foreach (var unit in decks)
                {
                    if (unit.Key == player.Value.ToString())
                    {
                        GameManager.deck = (Dictionary<string, object>)unit.Value;
                    }
                }
            }
        }

        checkTurnMoment();
    }

    // Update is called once per frame
    void Update()
    {
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
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("levelUp").SetValueAsync(0);
        Debug.Log("New turn.");
        ledger.SetActive(true);
        correspondence.SetActive(true);
        enemies.getEnemies();
        enemies.pullMageInfo();
        targetText.text = "CHOOSE A SCROLL";
        scrolls.pullFromDeck();
        ledger.GetComponent<LedgerManager>().pullLedger();
        correspondence.GetComponent<LedgerManager>().pullCorrespondence();
        GetComponent<ToggleButtons>().predeactivateButtons();
    }

    public void startEnemyTurn()
    {
        targetText.text = "ENEMY TURN";
        enemies.enemyTurn();
        enemies.resetTurn();
        endEnemyTurn();
    }

    public void endEnemyTurn()
    {
        Debug.Log("Enemy turn ended.");
        enemies.sendMageInfo();
        enemies.sendEnemyInfo();
        reference.Child("games").Child(GameManager.currentGame).Child("effect_" + auth.CurrentUser.UserId).Child("scroll").SetValueAsync(-1);
        reference.Child("games").Child(GameManager.currentGame).Child("starts").Child("start_" + playerManager.mageElement).SetValueAsync(1);


        //change = true;
        Debug.Log("Change.");
    }

    public void getAndRunScrolls()
    {
        targetText.text = "TURN OVER";
        Dictionary<string, object> turnDict = (Dictionary<string, object>)data["turn"];
        /* foreach (var dict in turnDict)
        {
            var effect = (Dictionary<string, object>)dict.Value;

            if (effect["target"].ToString() == "enemy1")
            {
                enemy1.scrollEffect(deckOptions[int.Parse(effect["scroll"].ToString())]);
            }
            if (effect["target"].ToString() == "enemy2")
            {
                enemy2.scrollEffect(deckOptions[int.Parse(effect["scroll"].ToString())]);
            }
            if (effect["target"].ToString() == "enemy3")
            {
                enemy3.scrollEffect(deckOptions[int.Parse(effect["scroll"].ToString())]);
            }
            if (effect["target"].ToString() == "mageFire")
            {
                fireMage.scrollEffect(deckOptions[int.Parse(effect["scroll"].ToString())]);
            }
            if (effect["target"].ToString() == "mageWater")
            {
                waterMage.scrollEffect(deckOptions[int.Parse(effect["scroll"].ToString())]);
            }
            if (effect["target"].ToString() == "mageEarth")
            {
                earthMage.scrollEffect(deckOptions[int.Parse(effect["scroll"].ToString())]);
            }
            if (effect["target"].ToString() == "mageWind")
            {
                windMage.scrollEffect(deckOptions[int.Parse(effect["scroll"].ToString())]);
            }
        } */
    }

    public void checkTurnMoment()
    {
        // Wizards turn, if scroll played, wait for other wizards.
        if (int.Parse(data["turnEnded"].ToString()) == 0)
        {
            Dictionary<string, object> starts = (Dictionary<string, object>)data["starts"];
            foreach (var start in starts)
            {
                if (start.Key == "start_" + GetComponent<PlayerManager>().mageElement)
                {
                    if (start.Value.ToString() == "1")
                    {
                        enemiesGO.SetActive(true);
                        scrolls.notTurn();
                        //change = true;
                        newTurn();

                    }
                    else
                    {
                        targetText.text = "RESOLVING TURNS";
                        scrolls.notTurn();
                    }
                }
            }
        }

        // Resolution of wizard's scrolls.
        else if (int.Parse(data["turnEnded"].ToString()) == 1)
        {
            enemiesGO.SetActive(true);
            //change = true;
            ledger.SetActive(false);
            correspondence.SetActive(false);
            scrolls.notTurn();
            getAndRunScrolls();   
        }

        // Enemy turn, if enemies dead, level up.
        else if (int.Parse(data["turnEnded"].ToString()) == 2)
        {
            enemiesGO.SetActive(true);
            if (enemy1.GetComponent<HealthManager>().currentHealth <= 0 &&
                enemy2.GetComponent<HealthManager>().currentHealth <= 0 &&
                enemy3.GetComponent<HealthManager>().currentHealth <= 0)
            {

                int level = int.Parse(TurnManager.data["level"].ToString());
                level++;
                Debug.Log("Level Up!");
                enemies.setEnemiesLevel(level);
            }
            else
            {
                startEnemyTurn();
            }
        }
    }
}
