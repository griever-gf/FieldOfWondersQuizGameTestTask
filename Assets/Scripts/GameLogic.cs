using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField]
    private FoWStateMachineData data;

    private StateMachineBase stateMachine;

    void Start()
    {
        stateMachine = new StateMachineBase();
        data.context = stateMachine;
        stateMachine.ChangeStay(data.gamePlayingState, data);
    }

    private void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }
}

[System.Serializable]
public class FoWStateMachineData : IStateData
{
    public StateMachineBase context;

    public GamePlayingState gamePlayingState;

    public GameFinishedState gameFinishedState;

    public GameWordGuessedState gameWordGuessedState;

    public MonoBehaviour CoroutineRunner;
}

[System.Serializable]
public abstract class GameState : IState
{
    protected FoWStateMachineData data;

    public virtual void Enter(IStateData data)
    {
        if (!(data is FoWStateMachineData))
            throw new System.ArgumentException("Invalid state data");

        this.data = data as FoWStateMachineData;
    }

    public abstract void Exit();

    public abstract void Update(float deltaTime);
}

[System.Serializable]
public class GamePlayingState : GameState
{
    [SerializeField]
    private GameSpawnController gameSpawnController;

    [SerializeField]
    private GameData gameData;

    [SerializeField]
    private UIGroupsSwitcher uiGroupsSwitcher;

    bool isFirstPlay = true;

    public override void Enter(IStateData data)
    {
        base.Enter(data);
        Debug.Log("Entering play mode");
        if (isFirstPlay)
        {
            gameData.SetDefaultTriesCount();
            
            gameData.ResetWords();
            gameData.ResetScore();
            gameSpawnController.RefreshLabelScores(gameData.GetCurrentScores());
            isFirstPlay = false;
        }

        gameData.ResetTries();
        gameSpawnController.RefreshLabelTries(gameData.GetCurrentTries());
        gameSpawnController.SpawnWord(gameData.SelectRandomWord());

        gameSpawnController.SpawnLetterButtons();
        gameSpawnController.OnAnySymbolButtonPressed = SymbolButtonPressed;
    }

    private void SymbolButtonPressed(char symbol)
    {
        gameData.DescreaseTries();
        gameSpawnController.RefreshLabelTries(gameData.GetCurrentTries());

        gameData.CheckCurrentWordForSymbol(symbol);
        gameSpawnController.OpenWordLetters(gameData.GetOpenedLettersStates());

        if (gameData.IsCurrentWordOpened())
        {
            //victory
            gameData.RemoveCurrentWord();
            gameData.AddScore(gameData.GetCurrentTries());
            gameSpawnController.RefreshLabelScores(gameData.GetCurrentScores());

            Debug.Log("Current word is opened!");
            
            if (gameData.IsAnyWordsLeft())
            {
                data.context.ChangeStay(data.gameWordGuessedState, data);
                uiGroupsSwitcher.ShowUIGroupWordGuessed();
            }
            else //no more words
            {
                data.context.ChangeStay(data.gameFinishedState, data);
                uiGroupsSwitcher.ShowUIGroupGameFinished();
            }
        }
        else
            if (!gameData.IsAnyTriesLeft()) //game over
            {
                data.context.ChangeStay(data.gameFinishedState, data);
                uiGroupsSwitcher.ShowUIGroupGameFinished();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting play mode");
    }

    public override void Update(float deltaTime)
    {

    }
}

[System.Serializable]
public class GameFinishedState : GameState
{
    [SerializeField]
    private Button buttonRestartGame;

    [SerializeField]
    private Text labelInfo;

    [SerializeField]
    private GameSpawnController gameSpawnController;

    [SerializeField]
    private GameData gameData;

    [SerializeField]
    private UIGroupsSwitcher uiGroupsSwitcher;

    public override void Enter(IStateData data)
    {
        base.Enter(data);
        gameSpawnController.OpenWordFull();
        buttonRestartGame.onClick.AddListener(RestartGame);
        if (!gameData.IsAnyTriesLeft())
            labelInfo.text = "Проигрыш. Игра окончена!";
        else
            labelInfo.text = "Поздравляем, все слова угаданы!";
    }

    private void RestartGame()
    {
        gameData.ResetWords();
        gameData.ResetScore();
        gameSpawnController.RefreshLabelScores(gameData.GetCurrentScores());

        data.context.ChangeStay(data.gamePlayingState, data);
        uiGroupsSwitcher.ShowUIGroupPlay();
    }

    public override void Exit()
    {
        buttonRestartGame.onClick.RemoveListener(RestartGame);
    }

    public override void Update(float deltaTime)
    {
    }
}

[System.Serializable]
public class GameWordGuessedState : GameState
{
    [SerializeField]
    private Button buttonNextWord;
    [SerializeField]
    private UIGroupsSwitcher uiGroupsSwitcher;

    public override void Enter(IStateData data)
    {
        base.Enter(data);
        buttonNextWord.onClick.AddListener(SpawnNextWord);
    }

    void SpawnNextWord()
    {
        data.context.ChangeStay(data.gamePlayingState, data);
        uiGroupsSwitcher.ShowUIGroupPlay();
    }

    public override void Exit()
    {
        buttonNextWord.onClick.RemoveListener(SpawnNextWord);
    }

    public override void Update(float deltaTime)
    {
    }
}