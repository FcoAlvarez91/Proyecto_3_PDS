﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Linq;
using System.Text.RegularExpressions;

public class TurnManager : MonoBehaviour
{

    public TextMeshProUGUI targetText;
    public EnemyManager enemies;
    public DrawScrolls scrolls;

    public Scroll[] deckOptions;

    public bool turnEnded;
    public static bool change;
    private bool change2;
    private bool playerTurn;

    public GameObject background;
    public GameObject ledger;

    public string playerEmail;

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
        GetComponent<ToggleButtons>().predeactivateButtons();

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://four-mages.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        loadDB();
        StartCoroutine(WaitForStart());
        InvokeRepeating("gameChangeLoadDB", 5.0f, 10f);
        InvokeRepeating("gameChangeStart", 7.0f, 10f);
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(3);
        gameStart();
        background.SetActive(false);
        ledger.SetActive(true);
    }

    public void loadDB()
    {
        var dataOut = new Dictionary<string, object>();
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
        });
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

    void gameChangeLoadDB()
    {
        Debug.Log("Checking change.");
        if (change)
        {
            Debug.Log("Change found.");
            change2 = true;
            loadDB();
        }
        else
        {
            Debug.Log("Change not found.");
        }
    }

    void gameChangeStart()
    {
        if (change2)
        {
            change = false;
            change2 = false;
            gameStart();
        }
    }

    public void gameStart()
    {
        targetText.text = "CHOOSE A SCROLL";
        GetComponent<ToggleButtons>().predeactivateButtons();
        background.SetActive(false);
        ledger.SetActive(true);
        enemies.getEnemies();
        enemies.pullMageInfo();
        Dictionary<string, object> players = (Dictionary<string, object>)data["players"];
        foreach (var player in players)
        {
            Debug.Log(playerEmail);
            Debug.Log(player.Key);
            string pK = player.Key;
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            playerEmail = rgx.Replace(playerEmail, "");
            pK = rgx.Replace(pK, "");
            if (playerEmail.Length != pK.Length)
            {
                Debug.Log("not length");
            }
            if (Equals(playerEmail, pK))
            {
                Debug.Log("element: " + player.Value.ToString());
                GetComponent<PlayerManager>().setElement(player.Value.ToString());
            }
        }
           
        checkTurnEnd();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void newTurn()
    {
        Debug.Log("New turn.");
        reference.Child("games").Child(GameManager.currentGame).Child("start_" + GetComponent<PlayerManager>().mageElement).SetValueAsync(0);
        enemies.getEnemies();
        targetText.text = "CHOOSE A SCROLL";
        scrolls.pullFromDeck();
        ledger.GetComponent<LedgerManager>().pullLedger();
        GetComponent<ToggleButtons>().activateScrollButtons();
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
        reference.Child("games").Child(GameManager.currentGame).Child("turn").RemoveValueAsync();
        reference.Child("games").Child(GameManager.currentGame).Child("turnEnded").SetValueAsync(0);

        reference.Child("games").Child(GameManager.currentGame).Child("start_" + GetComponent<PlayerManager>().mageElement).SetValueAsync(1);

        change = true;
        Debug.Log("Change.");
    }

    public void getAndRunScrolls()
    { 
        Dictionary<string, object> turnDict = (Dictionary<string, object>)data["turn"];
        foreach (var dict in turnDict)
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
        }

        startEnemyTurn();
            
    }

    public void checkTurnEnd()
    {
        if(int.Parse(data["turnEnded"].ToString()) == 1)
        {
            change = true;
            scrolls.notTurn();
            getAndRunScrolls();
        }
        else
        {
            Debug.Log(GetComponent<PlayerManager>().mageElement);
            if (int.Parse(data["start_" + GetComponent<PlayerManager>().mageElement].ToString()) == 1)
            {
                change = true;
                newTurn();

            }
        }
    }
}
