using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public Card cardPrefab;
    public Transform cardSpawnPos;
    public Vector2 startPosition = new Vector2(-1.87f, 3.62f);
    public int pictureCount;
    [Space]
    [Header("End Game Screen")]
    public GameObject EndGamePanel;

    public GameObject NewBestTimeText;
    public GameObject YourTimeText;
    public GameObject RoundTimeText;

    private bool coroutineStarted = false;
    private int cardNumbers;
    private int removedCards;
    private Timer gameTimer;

    public enum GameState 
    {
        NoAction, 
        MovingToPositions, 
        DeletingPairs, 
        FlipBack, 
        Checking, 
        GameEnd
    };

    public enum CardState
    {
        CardRotating,
        CanRotate
    };

    public enum RevealedState
    {
        NoRevealed,
        OneRevealed,
        TwoRevealed
    };

    [HideInInspector]
    public GameState currentGameState;
    [HideInInspector]
    public CardState currentCardState;
    [HideInInspector]
    public RevealedState cardRevealedNumber;

    [HideInInspector]
    public List<Card> cardList;

    private Vector2 offset20C = new Vector2(1.25f, 1.25f);
    private Vector2 offset30C = new Vector2(1.08f, 1.22f);
    private Vector2 offset40C = new Vector2(1.08f, 1.0f);
    private Vector3 newScaleDown = new Vector3(0.9f, 0.9f, 0.001f);

    private List<Material> materialList = new List<Material>();
    private List<string> texturePathList = new List<string>();
    private Material firstMaterial;
    private string firstTexturePath;

    private int firstRevealedCard;
    private int secondRevealedCard;
    private int revealedCardNumber = 0;
    private int cardToDestroy1;
    private int cardToDestroy2;

    void Start()
    {
        currentGameState = GameState.NoAction;
        currentCardState = CardState.CanRotate;
        cardRevealedNumber = RevealedState.NoRevealed;
        revealedCardNumber = 0;
        firstRevealedCard = -1;
        secondRevealedCard = -1;

        removedCards = 0;
        cardNumbers = (int) GameSettings.Instance.GetCardNumber();
        gameTimer = GameObject.Find("CardManager").GetComponent<Timer>();
        LoadMaterials();

        if(GameSettings.Instance.GetCardNumber() == GameSettings.ECardNumber.E20Cards)
        {
        currentGameState = GameState.MovingToPositions;
        SpawnCardMesh(4, 5, startPosition, offset20C, false);
        MoveCard(4, 5, startPosition, offset20C);
        }
        else if (GameSettings.Instance.GetCardNumber() == GameSettings.ECardNumber.E30Cards)
        {
            currentGameState = GameState.MovingToPositions;
            SpawnCardMesh(5, 6, startPosition, offset30C, false);
            MoveCard(5, 6, startPosition, offset30C);
        }
        //else if (GameSettings.Instance.GetCardNumber() == GameSettings.ECardNumber.E40Cards)
        //{
        //    currentGameState = GameState.MovingToPositions;
        //    SpawnCardMesh(5, 8, startPosition, offset30C, true);
        //    MoveCard(5, 8, startPosition, offset30C);
        //}
    }

    public void CheckCard()
    {
        //Checks how many cards are showing front side up
        currentGameState = GameState.Checking;
        revealedCardNumber = 0;

        for (int id = 0; id < cardList.Count; id++)
        {
            if (cardList[id].revealed && revealedCardNumber < 2)
            {
                if (revealedCardNumber == 0)
                {
                    firstRevealedCard = id;
                    revealedCardNumber++;
                }
                else if (revealedCardNumber == 1)
                {
                    secondRevealedCard = id;
                    revealedCardNumber++;
                }
            }
        }

        if (revealedCardNumber == 2)
        {
            if(cardList [firstRevealedCard].GetIndex() == cardList[secondRevealedCard].GetIndex() && firstRevealedCard != secondRevealedCard)
            {
                currentGameState = GameState.DeletingPairs;
                cardToDestroy1 = firstRevealedCard;
                cardToDestroy2 = secondRevealedCard;
            }
            else
            {
                currentGameState = GameState.FlipBack;
            }
        }
       
        currentCardState = CardManager.CardState.CanRotate;

        if(currentGameState == GameState.Checking)
        {
            currentGameState = GameState.NoAction;
        }
    }

    private void DestroyCard()
    {
        cardRevealedNumber = RevealedState.NoRevealed;
        cardList[cardToDestroy1].Deactivate();
        cardList[cardToDestroy2].Deactivate();
        revealedCardNumber = 0;
        removedCards+=2; 
        currentGameState = GameState.NoAction;
        currentCardState = CardState.CanRotate;
    }

    private IEnumerator FlipBack()
    {
        coroutineStarted = true;
        yield return new WaitForSeconds(0.5f);

        cardList[firstRevealedCard].FlipBack();
        cardList[secondRevealedCard].FlipBack();

        cardList[firstRevealedCard].revealed = false;
        cardList[secondRevealedCard].revealed = false;

        cardRevealedNumber = RevealedState.NoRevealed;
        currentGameState = GameState.NoAction;
        coroutineStarted = false;
    }

    private void LoadMaterials()
    //This function loads the materials from the two Fighters art directories and makes them ready to be applied
    {
        var materialFilePath = GameSettings.Instance.GetMaterialDirectoryName();
        var textureFilePath = GameSettings.Instance.GetFighterSelectionTextureDirectoryName();
        var cardNumber = (int)GameSettings.Instance.GetCardNumber();
        const string matBaseName = "Pic";
        var firstMaterialName = "Back";

        for (var index = 1; index <= pictureCount; index++)
        {
            var currentFilePath = materialFilePath + matBaseName + index;
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
            materialList.Add(mat);

            var currentTextureFilePath = textureFilePath + matBaseName + index;
            texturePathList.Add(currentTextureFilePath);
        }

        firstTexturePath = textureFilePath + firstMaterialName;
        firstMaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;

    }

    private void Update()
    {
        if(currentGameState == GameState.DeletingPairs)
        {
            if(currentCardState == CardState.CanRotate)
            {
                DestroyCard();
                CheckGameEnd();
            }
        }
        if(currentGameState == GameState.FlipBack)
        {
            if (currentCardState == CardState.CanRotate && coroutineStarted == false)
            {
                StartCoroutine(FlipBack());
            }
        }

        if (currentGameState == GameState.GameEnd)
        {
            if (cardList[firstRevealedCard].gameObject.activeSelf == false &&
                cardList[secondRevealedCard].gameObject.activeSelf == false &&
                EndGamePanel.activeSelf == false)
            {
                ShowEndGameInfo();
            }
        }
    }

    private bool CheckGameEnd()
    {
        if (removedCards == cardNumbers && currentGameState != GameState.GameEnd)
        {
            currentGameState = GameState.GameEnd;
            gameTimer.StopTimer();
            Config.PlaceScoreOnBoard(gameTimer.GetCurrentTime());

        }

        return (currentGameState == GameState.GameEnd);
    }
    private void ShowEndGameInfo()
    {
        EndGamePanel.SetActive(true);

        if(Config.IsBestScore())
        {
            NewBestTimeText.SetActive(true);
            YourTimeText.SetActive(false);
        }
        else
        {
            NewBestTimeText.SetActive(false);
            YourTimeText.SetActive(true);
        }

        var timer = gameTimer.GetCurrentTime();
        var minutes = Mathf.Floor(timer / 60);
        var seconds = Mathf.RoundToInt(timer % 60);
        var newText = minutes.ToString("00") + ":" + seconds.ToString("00");
        RoundTimeText.GetComponent<Text>().text = newText;
    }


    private void SpawnCardMesh(int rows, int columns, Vector2 pos, Vector2 offset, bool scaleDown)
        //Adds cardprefabs for each row/column position available
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var tempCard = (Card)Instantiate(cardPrefab, cardSpawnPos.position, cardPrefab.transform.rotation);

                if (scaleDown)
                {
                    tempCard.transform.localScale = newScaleDown;
                }

                tempCard.name = tempCard.name + 'c' + col + 'r' + row;
                cardList.Add(tempCard);
            }
        }

        ApplyTextures();
    }

    public void ApplyTextures()
        //Randomizes the second material (front of cards) from the materialList so that it'll be different each time 
    {
        var randomMaterialIndex = Random.Range(0, materialList.Count);
        //Array used to apply every material twice (to different cards)
        var AppliedTimes = new int[materialList.Count];

        for (int i = 0; i > materialList.Count; i++)
        {
            AppliedTimes[i] = 0;
        }

        // 16 picture indices
        var pictureIndices = new List<int>(); // 0, 1, 2, 3, 4... 15
        for (var i = 0; i < materialList.Count; ++i)
        {
            pictureIndices.Add(i);
        }
        
        // 20 elements for the game (10 unique) 
        var pictureIndicesInGame = new List<int>();
        for (var i = 0; i < cardList.Count / 2; ++i)
        {
            var randomIndex = Random.Range(0, pictureIndices.Count);
            pictureIndicesInGame.Add(pictureIndices[randomIndex]);
            pictureIndicesInGame.Add(pictureIndices[randomIndex]);
            pictureIndices.RemoveAt(randomIndex);
        }

        ShuffleArray(pictureIndicesInGame);

        // foreach (var obj in cardList)
        for (var i = 0; i < cardList.Count; ++i)
        {
            var obj = cardList[i];
            var randPrevious = randomMaterialIndex;
            var counter = 0;
            var forceMaterial = false;

            /*while(AppliedTimes[randomMaterialIndex] >= 2 || ((randPrevious == randomMaterialIndex) && !forceMaterial))
            {
                randomMaterialIndex = Random.Range(0, materialList.Count);
                counter++;
                //Tries to apply a specific material, if it doesn't succeed in 100 tries, loop through the list and apply the next available material that hasn't been applied yet
                if (counter > 100)
                {
                    for (var j = 0; j < materialList.Count; j++)
                    {
                        if (AppliedTimes[j] < 2)
                        {
                            randomMaterialIndex = j;
                            forceMaterial = true;
                        }
                    }
                    if (forceMaterial == false)
                        return;
                }
            }*/

            randomMaterialIndex = pictureIndicesInGame[i];
            Debug.Log($"For card #{i} picture index is {randomMaterialIndex}");
            obj.SetFirstMaterial(firstMaterial, firstTexturePath);
            obj.ApplyFirstMaterial();
            obj.SetSecondMaterial(materialList[randomMaterialIndex], texturePathList[randomMaterialIndex]);
            obj.SetIndex(randomMaterialIndex);
            obj.revealed = false;
            AppliedTimes[randomMaterialIndex] += 1;
            forceMaterial = false;
        }
    }

    private void MoveCard(int rows, int columns, Vector2 pos, Vector2 offset)
        //Moves the cards to the positions available. Stacks the cards in the rows and columns
    {
        var index = 0;
        for (var col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var targetPosition = new Vector3((pos.x + (offset.x * row)), (pos.y - (offset.y * col)), 0.0f);
                StartCoroutine(MoveToPosition(targetPosition, cardList[index]));
                index++;
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 target, Card obj)
    {
        var randomDis = 5;

        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis * Time.deltaTime);
            yield return 0;
        }
    }

    private void ShuffleArray(List<int> array)
    {
        for (var n = array.Count - 1; n > -1; n--)
        {
            var k = Random.Range(0, n);
            var buffer = array[n];
            array[n] = array[k];
            array[k] = buffer;
        }
    }
}
