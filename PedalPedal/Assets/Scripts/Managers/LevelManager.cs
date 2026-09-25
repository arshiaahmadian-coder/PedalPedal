using UnityEngine;

public enum Characters
{
    Zaal,
    Roodabeh,
    Player
}

public class LevelManager : MonoBehaviour
{
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
    [SerializeField] bool hasStartDialog = false;
    [SerializeField] private DialogData StarterDialogData;
    [SerializeField] private float startDialogDelay = 1f;
    [SerializeField] bool hasEndDialog = false;
    [SerializeField] private DialogData endingDialogData;
    [SerializeField] private float endDialogDelay = 0f;

    private int gameStage = 0;
    private bool playerHasLatter = false;
    public static LevelManager instance;
    private void Awake() { instance = this; }

    private void Start()
    {
        if (letterSenderCharacter == Characters.Zaal)
        {
            letterThrower.spriteRenderer = ZaalLatterObject.GetComponent<SpriteRenderer>();
        } else
        {
            letterThrower.spriteRenderer = RoodabehLatterObject.GetComponent<SpriteRenderer>();
        }

        if (hasStartDialog)
        {
            if (dialogManager.dialogData.zoomCamera) {
                cinemachineSmoothZoom.ZoomBy(dialogManager.dialogData.zoomAmount);
            }

            Controllers.SetActive(false);
            dialogManager.touchArea.SetActive(false);
            dialogManager.dialogData = StarterDialogData;
            Invoke(nameof(StartDialog), startDialogDelay);
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
        if (hasEndDialog)
        {
            // play ending dialog
        } else
        {
            print("Game Over, you win !");
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
            // end game
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
            if (dialogManager.dialogData.zoomCamera) {
                cinemachineSmoothZoom.ZoomBy(-dialogManager.dialogData.zoomAmount);
            }
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
}
