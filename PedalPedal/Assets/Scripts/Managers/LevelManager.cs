using ArabicTextSupport;
using UnityEngine;
using UnityEngine.UI;

public enum Characters
{
    Zaal,
    Roodabeh,
    Player
}

public class LevelManager : MonoBehaviour
{
    public LevelData levelData;
    public GameObject Controllers;
    public LevelTimer levelTimer;
    public ColideDetection playerColideDetection; // for colide number data 

    [Header("Letter Settings")]
    [SerializeField]  Characters letterSenderCharacter;
    [SerializeField]  Transform ZaalLetterPos;
    [SerializeField]  Transform RoodabehLetterPos;
    [SerializeField]  Transform PlayerLetterPos;
    [SerializeField]  LetterThrower letterThrower;
    [SerializeField]  GameObject ZaalLatterObject;
    [SerializeField]  GameObject RoodabehLatterObject;

    [Header("Dialog settings")]
    [SerializeField] CinemachineSmoothZoom cinemachineSmoothZoom;
    [SerializeField] DialogManager dialogManager;

    [Header("Win Window")]
    [SerializeField] Animator winWindowAnimator;
    [SerializeField] GameObject winCanv;
    [SerializeField] ArabicUIText letterMesageTxt;
    [SerializeField] ArabicUIText letterTitleTxt;
    [SerializeField] ArabicUIText letterSenderTxt;
    [SerializeField] ArabicUIText timerTxt;
    [SerializeField] ArabicUIText coinCountTxt;
    [SerializeField] Image[] StarImageList;
    [SerializeField] Sprite falseStar;
    [SerializeField] Sprite trueStar;

    private int gameStage = 0;
    private bool playerHasLatter = false;
    public static LevelManager instance;
    private void Awake() { instance = this; }

    private void Start()
    {
        Controllers.SetActive(false);
        dialogManager.touchArea.SetActive(false);

        if (letterSenderCharacter == Characters.Zaal)
        {
            letterThrower.spriteRenderer = ZaalLatterObject.GetComponent<SpriteRenderer>();
        } else
        {
            letterThrower.spriteRenderer = RoodabehLatterObject.GetComponent<SpriteRenderer>();
        }

        if (levelData.hasStartDialog)
        {
            cinemachineSmoothZoom.ZoomBy(-2.5f);
            dialogManager.dialogData = levelData.dialogData;
            Invoke(nameof(StartDialog), levelData.dialogData.startDialogDelay);
        } else
        {
            Invoke(nameof(ThrowLetter), 0.2f);
            cinemachineSmoothZoom.ForceZoom(-2.5f);
        }
    }

    public void PlayerReachedEndPoint()
    {
        levelTimer.StopTimer();
        Controllers.SetActive(false);
        // throw letter to reciver
        Invoke(nameof(ThrowLetter), 0.8f);
    }

    private void LetterRecived()
    {
        if (false)
        {
            // TODO: play ending dialog
        } else
        {
            GameOver();
        }
    }

    public void EndOfDialogCycle()
    {
        gameStage += 1;

        if (gameStage == 1) // after first dialog
        {
            ThrowLetter();
        } else if (gameStage == 2) // after second dialog
        {
            GameOver();
        }
    }

    public void ThrowLetter()
    {
        Vector2 endPos = PlayerLetterPos.position;
        Vector2 startPos = PlayerLetterPos.position;

        if (playerHasLatter)
        {
            if (letterSenderCharacter == Characters.Zaal)
                endPos = RoodabehLetterPos.position;
            else
                endPos = ZaalLetterPos.position;
        } else
        {
            if (letterSenderCharacter == Characters.Zaal) 
                startPos = ZaalLetterPos.position;
            else 
                startPos = RoodabehLetterPos.position;
        }

        playerHasLatter = !playerHasLatter;

        // set values
        letterThrower.ThrowLetter(
            (letterSenderCharacter == Characters.Zaal) 
                ? ZaalLatterObject.transform 
                : RoodabehLatterObject.transform,
            startPos, endPos,
            LetterOnMidPoint, LetterOnComplete
        );
    }

    private void LetterOnMidPoint() {
        letterThrower.spriteRenderer.sortingOrder = playerHasLatter ? 8 : 20;
    }

    private void LetterOnComplete() {
        if (playerHasLatter)
        {
            Controllers.SetActive(true);
            cinemachineSmoothZoom.ZoomBy(2.5f);
            levelTimer.StartTimer();
        }
        else
        {
            // letter recived to Roodabeh/Zaal
            LetterRecived();
        }

        // TODO: particle effects
    }

    private void StartDialog()
    {
        dialogManager.StartDialogCycle();
    }

    public void OnRestartClicked()
    {
        NavigationAnimator.Instance.TransparentToDart("Level " + levelData.levelID);
    }

    public void OnNextLevelClicked()
    {
        NavigationAnimator.Instance.TransparentToDart("Level " + (levelData.levelID + 1));
    }

    private void GameOver()
    {
        winWindowAnimator.SetBool("IsUp", true);
        // save data
        Database playerData = new Database();
        if (SaveLoadManager.CheckFileExists(SaveLoadManager.GetInstnce().DataFileName))
            SaveLoadManager.GetInstnce().Load(playerData, SaveLoadManager.GetInstnce().DataFileName);

        levelTimer.StopTimer();
        winCanv.SetActive(true);
        int starNumber = 1;
        if (levelTimer.seconds <= levelData.threeStarTime) starNumber = 3;
        else if (levelTimer.seconds <= levelData.TwoStarTime) starNumber = 2;

        int coinCount = 10 * (starNumber - playerData.levelData[levelData.levelID]);
        if (coinCount < 0) coinCount = 0;

        // update ui
        letterMesageTxt.Text = levelData.latterMessage;
        timerTxt.Text = levelTimer.timeStr;
        coinCountTxt.Text = coinCount.ToString();
        if (starNumber >= 1) StarImageList[0].sprite = trueStar;
        if (starNumber >= 2) StarImageList[1].sprite = trueStar;
        if (starNumber == 3) StarImageList[2].sprite = trueStar;

        if (letterSenderCharacter == Characters.Zaal)
        {
            letterTitleTxt.Text = "نامه ای به رودابه";
            letterSenderTxt.Text = "زال";
        } else
        {
            letterTitleTxt.Text = "نامه ای به رودابه";
            letterSenderTxt.Text = "زال";
        }

        if (starNumber > playerData.levelData[levelData.levelID])
            playerData.levelData[levelData.levelID] = starNumber;

        playerData.coinAmount += coinCount;
        

        if (playerData.lastUnlockedLevelID == levelData.levelID) 
            playerData.lastUnlockedLevelID = levelData.levelID + 1; // new level unlocked

        SaveLoadManager.GetInstnce().Save(playerData, SaveLoadManager.GetInstnce().DataFileName);
    }
}
