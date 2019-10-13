using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class TurnManager : MonoBehaviour
{

    public bool newTurnBool;
    public int turn;
    public GameObject targetText;
    public GameObject enemies;
    public GameObject scrolls;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    public DatabaseReference reference;

    // Start is called before the first frame update
    void Start()
    {
        turn = 0;
        targetText.GetComponent<TextMeshProUGUI>().text = "CHOOSE A SCROLL";
        GetComponent<ToggleButtons>().predeactivateButtons();

        InitializeFirebase();
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://four-mages.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
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
            startEnemyTurn();
            newTurnBool = false;
        }
    }

    public void newTurn()
    {
        turn++;
        targetText.GetComponent<TextMeshProUGUI>().text = "CHOOSE A SCROLL";
        scrolls.GetComponent<DrawScrolls>().pullFromDeck();
        GetComponent<ToggleButtons>().activateScrollButtons();
        GetComponent<ToggleButtons>().predeactivateButtons();
    }

    // ------ Add DBListener here for new turn -------- //


    // ------------------------------------------------ //

    public void startEnemyTurn()
    {
        enemies.GetComponent<EnemyManager>().resetTurn();
        targetText.GetComponent<TextMeshProUGUI>().text = "ENEMY TURN";
        enemies.GetComponent<EnemyManager>().enemyTurn();
        endEnemyTurn();
    }

    public void endEnemyTurn()
    {
        newTurn();
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);
    }

    public void setNewTurnBool()
    {
        newTurnBool = true;
    }
}
