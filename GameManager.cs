using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Mirror;
using DG.Tweening;


public class GameManager : NetworkBehaviour
{

    public static GameManager Instance { get; private set; }
    public CustomNetworkManager networkManager;
    public CowController cowController;

    private int catchedPoop = 0;
    private int missedPoop = 0;

    private bool winner = false;
    private bool gameEnded = false;

    private int itemsUnlocked = 0;

    private bool isScaling = false;

    [SyncVar]
    private bool isGameStartClicked;

    //TEST 
    [SerializeField]
    private TextMeshProUGUI missedPoopText;

    //intro canvas
    [SerializeField]
    private Canvas cowIntroCanvas;
    [SerializeField]
    private Canvas farmerIntroCanvas;

    // player specific canvas
    [SerializeField]
    private Canvas farmerCanvas;
    [SerializeField]
    private Canvas cowCanvas;

    //superpoop slider
    [SerializeField]
    private Slider superPoopSlider;
    [SerializeField]
    private Slider environmentslider;

    //downgrade canvas
    [SerializeField]
    private Slider downgradeSlider;
    [SerializeField]
    private Image missedBorder;

    //upgrade canvas
    [SerializeField]
    private Slider upgradeSlider;
    [SerializeField]
    private TextMeshProUGUI upgradeText;

    [SerializeField]
    private RawImage[] itemIcons;
    [SerializeField]
    private Texture[] itemTextures;

    //popup canvas
    [SerializeField]
    private Canvas popUpCanvas;
    [SerializeField]
    private RawImage popUp;
    [SerializeField]
    private Button popUpButton;
    [SerializeField]
    private Texture[] popUps;

    //ending canvas
    [SerializeField]
    private Canvas endingCanvas;
    [SerializeField]
    private RawImage endingImg;
    [SerializeField]
    private TextMeshProUGUI endingTitle;
    [SerializeField]
    private Button endingButton;
    [SerializeField]
    private Texture winnerImg;

    //early quit canvas
    [SerializeField]
    private Canvas quitCanvas;

    //unlocked items
    public GameObject farmerBody;
    public GameObject farmerLegLeft;
    public GameObject farmerLegRight;
    [SerializeField]
    private Material blue;

    [SerializeField]
    private GameObject toilet;
    [SerializeField]
    private GameObject toiletPaper;
    [SerializeField]
    private GameObject cowPot;

    //environment
    [SerializeField]
    private GameObject tree1;
    [SerializeField]
    private GameObject tree2;
    [SerializeField]
    private GameObject tree3;
    [SerializeField]
    private GameObject tree4;
    [SerializeField]
    private GameObject tree5;
    [SerializeField]
    private GameObject tree6;
    [SerializeField]
    private Material applesBad;
    [SerializeField]
    private Material treeBad;
    [SerializeField]
    private Material treeTrunkBad;

    [SerializeField]
    private GameObject flowerRed1;
    [SerializeField]
    private GameObject flowerRed2;
    [SerializeField]
    private GameObject flowerPurple3;
    [SerializeField]
    private GameObject flowerPurple4;
    [SerializeField]
    private GameObject flowerPink5;
    [SerializeField]
    private GameObject flowerPink6;
    [SerializeField]
    private Material flowerCenterBad;
    [SerializeField]
    private Material flowerStemBad;
    [SerializeField]
    private Material flowerRedLeafBad;
    [SerializeField]
    private Material flowerPinkLeafBad;
    [SerializeField]
    private Material flowerPurpleLeafBad;

    [SerializeField]
    private GameObject plane;
    [SerializeField]
    private Material grassBad;
    [SerializeField]
    private Material waterBad;

    [SerializeField]
    private GameObject airFarmer;
    [SerializeField]
    private GameObject airCow;

    //renderers
    private Renderer rendBody;
    private Renderer rendLegLeft;
    private Renderer rendLegRight;

    private Renderer rendAppleTree1;
    private Renderer rendAppleTree2;
    private Renderer rendTree3;
    private Renderer rendTree4;
    private Renderer rendTree5;
    private Renderer rendTree6;

    private Renderer rendRedFlower1;
    private Renderer rendRedFlower2;
    private Renderer rendPurpleFlower3;
    private Renderer rendPurpleFlower4;
    private Renderer rendPinkFlower5;
    private Renderer rendPinkFlower6;

    private Renderer rendPlane;
/*    private Renderer rendAirFarmer;
    private Renderer rendAirCow;*/

    //CowController (CowCanvas) objecten
    [SerializeField]
    private GameObject pointer;
    private Button fireButton;
    //make difference between gameobject and button
    private GameObject superFireButtonGO;
    private Button superFireButton;

    void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this;
        }
    }

    void Start()
    {
        rendAppleTree1 = tree1.GetComponent<Renderer>();
        rendAppleTree2 = tree2.GetComponent<Renderer>();
        rendTree3 = tree3.GetComponent<Renderer>();
        rendTree4 = tree4.GetComponent<Renderer>();
        rendTree5 = tree5.GetComponent<Renderer>();
        rendTree6 = tree6.GetComponent<Renderer>();

        rendRedFlower1 = flowerRed1.GetComponent<Renderer>();
        rendRedFlower2 = flowerRed2.GetComponent<Renderer>();
        rendPurpleFlower3 = flowerPurple3.GetComponent<Renderer>();
        rendPurpleFlower4 = flowerPurple4.GetComponent<Renderer>();
        rendPinkFlower5 = flowerPink5.GetComponent<Renderer>();
        rendPinkFlower6 = flowerPink6.GetComponent<Renderer>();

        rendPlane = plane.GetComponent<Renderer>();

/*        rendAirFarmer = airFarmer.GetComponent<Renderer>();
        rendAirCow = rendAirCow.GetComponent<Renderer>();*/

        //CowCanvas superFireButton
        superFireButtonGO = cowCanvas.transform.GetChild(2).gameObject;
        enableSuperFireButton(false);
    }

    void Update()
    {
        if (isGameStartClicked)
        {
            StartGame();
        }
    }

    public override void OnStartClient()
    {
        Button startButton = cowIntroCanvas.transform.GetChild(3).GetComponent<Button>();

        if (!isServer)
        {
            farmerIntroCanvas.gameObject.SetActive(true);
            cowIntroCanvas.gameObject.SetActive(false);

            farmerCanvas.gameObject.SetActive(false);
            cowCanvas.gameObject.SetActive(false);
            pointer.SetActive(false);

            startButton.interactable = false;

            /*//Set the canvas active or inactive
            farmerCanvas.gameObject.SetActive(true);
            cowCanvas.gameObject.SetActive(false);
            //Set pointer to inactive
            pointer.SetActive(false);*/
        }
        else
        {
            farmerIntroCanvas.gameObject.SetActive(false);
            cowIntroCanvas.gameObject.SetActive(true);

            farmerCanvas.gameObject.SetActive(false);
            cowCanvas.gameObject.SetActive(false);
            pointer.SetActive(false);

            /*//Set the canvas active or inactive
            cowCanvas.gameObject.SetActive(true);
            farmerCanvas.gameObject.SetActive(false);
            //Set pointer to active
            pointer.SetActive(true);

            fireButton = cowCanvas.transform.GetChild(1).GetComponent<Button>();
            superFireButton = cowCanvas.transform.GetChild(2).GetComponent<Button>();
            networkManager.setUIObjects(pointer, fireButton, superFireButton);*/
        }

        isGameStartClicked = false;
    }

    public void setBodyOfFarmer(GameObject farmer)
    {
        /*GameObject farmerChild = farmer.transform.GetChild(0).gameObject;

        farmerLegLeft = farmerChild.transform.GetChild(2).gameObject;
        rendLegLeft = farmerLegLeft.GetComponent<Renderer>();

        farmerLegRight = farmerChild.transform.GetChild(3).gameObject;
        rendLegRight = farmerLegRight.GetComponent<Renderer>();

        farmerBody = farmerChild.transform.GetChild(5).gameObject;
        rendBody = farmerBody.GetComponent<Renderer>();*/

        farmerLegLeft = farmer.transform.GetChild(3).gameObject;
        rendLegLeft = farmerLegLeft.GetComponent<Renderer>();

        farmerLegRight = farmer.transform.GetChild(5).gameObject;
        rendLegRight = farmerLegRight.GetComponent<Renderer>();

        farmerBody = farmer.transform.GetChild(0).gameObject;
        rendBody = farmerBody.GetComponent<Renderer>();
    }

    //weg?
    public int getCatchedPoop()
    {
        return catchedPoop;
    }

    //Weg?
    public int getMissedPoop()
    {
        return missedPoop;
    }

    public void addCathedPoop()
    {
        catchedPoop++;

        updateScoreGUI();

        if (catchedPoop == 5)
        {
            itemsUnlocked++;
            StartCoroutine(openPopUp(itemsUnlocked));
        }
    }

    public void addMissedPoop()
    {
        missedPoop++;

        //TEST
        missedPoopText.text = "missed: " + missedPoop;

        if (!isServer)
        {
            missedBorder.DOFade(5f, .6f);
            missedBorder.DOFade(0f, .6f);
        }

        //animatie TESTEN!!!!
        //downgradeSlider.DOValue(downgradeSlider.value - 1, 1f).Play();
        //note to self: waarom moet dit?
        //downgradeSlider.value = downgradeSlider.value - 1;

        superPoopSlider.DOValue(superPoopSlider.value + 1, 1f).Play();
        superPoopSlider.value++;
        //environmentslider farmer
        downgradeSlider.DOValue(downgradeSlider.value - 1, 1f).Play();
        downgradeSlider.value--;
        //environmentslider cow
        environmentslider.DOValue(environmentslider.value + 1, 1f).Play();
        environmentslider.value++;


        if (missedPoop%5 == 0)
        {
            updateDowngradeGUI();
            enableSuperFireButton(true);

            //superPoopSlider?
            superPoopSlider.DOValue(0, 1f).Play();
            environmentslider.value = 0;
        }
    }

    public void enableSuperFireButton(bool isVisible)
    {
        superFireButtonGO.SetActive(isVisible);
    }

    public void resetSuperPoopSliderValue()
    {
        superPoopSlider.value = 0;
    }

    private void updateScoreGUI()
    {
        upgradeText.text = catchedPoop + "/5";
        upgradeSlider.DOValue(upgradeSlider.value + 1, 1f).Play();
    }

    IEnumerator openPopUp(int itemToUnlock)
    {
        //disable canvas --> geen nieuwe stront afvuren
        cowCanvas.gameObject.SetActive(false);
        farmerCanvas.gameObject.SetActive(false);

        //show item on coin
        itemIcons[itemToUnlock - 1].texture = itemTextures[itemToUnlock - 1];

        //show item on popup
        popUp.texture = popUps[itemToUnlock - 1];

        //set popup active
        popUpCanvas.gameObject.SetActive(true);

        //open popup with scale effect
        TryScaleUpCanvas();

        //Time.timeScale = 0;

        yield return new WaitForSeconds(10);

        ////sluit popup
        closePopUp();

        //show the unlocked items
        showUnlockedItem(itemToUnlock);
    }

    private void TryScaleUpCanvas()
    {
        if (isScaling) { return; }
        StartCoroutine(ScaleUpPopUp());
    }

    IEnumerator ScaleUpPopUp()
    {
        isScaling = true;
        var tweener = popUp.transform.DOScale(new Vector3(1, 1, 1), .4f);

        while (tweener.IsActive()) { yield return null; }
        isScaling = false;
        Time.timeScale = 0;

    }

    private void updateDowngradeGUI()
    {
        

        switch (downgradeSlider.value)
        {
            //bloemen en gras
            case 10:
                var matsRedFlower = rendRedFlower1.materials;
                var matsPinkFlower = rendPinkFlower5.materials;
                var matsPurpleFlower = rendPurpleFlower3.materials;

                matsRedFlower[0] = flowerRedLeafBad;
                matsRedFlower[1] = flowerCenterBad;
                matsRedFlower[2] = flowerStemBad;

                rendRedFlower1.materials = matsRedFlower;
                rendRedFlower2.materials = matsRedFlower;

                matsPurpleFlower[0] = flowerPurpleLeafBad;
                matsPurpleFlower[1] = flowerCenterBad;
                matsPurpleFlower[2] = flowerStemBad;

                rendPurpleFlower3.materials = matsPurpleFlower;
                rendPurpleFlower4.materials = matsPurpleFlower;

                matsPinkFlower[0] = flowerPinkLeafBad;
                matsPinkFlower[1] = flowerCenterBad;
                matsPinkFlower[2] = flowerStemBad;

                rendPinkFlower5.materials = matsPinkFlower;
                rendPinkFlower6.materials = matsPinkFlower;

                var matsPlane = rendPlane.materials;
                matsPlane[1] = grassBad;
                rendPlane.materials = matsPlane;
                break;

            //bomen
            case 5:
                var matsAppleTree = rendAppleTree1.materials;
                matsAppleTree[0] = applesBad;
                matsAppleTree[1] = treeBad;
                matsAppleTree[2] = treeTrunkBad;
                rendAppleTree1.materials = matsAppleTree;
                rendAppleTree2.materials = matsAppleTree;

                var matsTree = rendTree3.materials;
                matsTree[0] = treeBad;
                matsTree[1] = treeTrunkBad;
                rendTree3.materials = matsTree;
                rendTree4.materials = matsTree;
                rendTree5.materials = matsTree;
                rendTree6.materials = matsTree;
                break;

            //lucht en water
            case 0:


                matsPlane = rendPlane.materials;
                matsPlane[3] = waterBad;
                rendPlane.materials = matsPlane;

                break;
            default:
                break;
        }

        if(downgradeSlider.value == 0)
        {
            gameEnded = true;
            winner = false;
            StartCoroutine(endGame());
        }

    }

    public void closePopUp()
    {

        if (gameEnded)
        {
            StartCoroutine(endGame());
        }
        else
        {
            catchedPoop = 0;
            /*popUpCanvas.gameObject.SetActive(false);
            Time.timeScale = 1;*/
            TryScaleDownCanvas();
        }
    }

    private void TryScaleDownCanvas()
    {
        if (isScaling) { return; }
        StartCoroutine(ScaleDownPopUp());
    }

    IEnumerator ScaleDownPopUp()
    {
        Time.timeScale = 1;
        isScaling = true;
        var tweener = popUp.transform.DOScale(new Vector3(0, 0, 1), .4f);
        Debug.Log("scaling");
        while (tweener.IsActive()) { yield return null; }
        Debug.Log("finished scaling");
        isScaling = false;
        popUpCanvas.gameObject.SetActive(false);

    }

    private void showUnlockedItem(int itemToUnlock)
    {
        //show unlock item in world
        if (itemToUnlock == 1)
        {

            var matsBody = rendBody.materials;
            matsBody[1] = blue;
            rendBody.materials = matsBody;

            var matsLegLeft = rendLegLeft.materials;
            matsLegLeft[0] = blue;
            rendLegLeft.materials = matsLegLeft;

            var matsLegRight = rendLegRight.materials;
            matsLegRight[0] = blue;
            rendLegRight.materials = matsLegRight;
        }

        if (itemToUnlock == 2)
        {
            toilet.SetActive(true);
        }

        if (itemToUnlock == 3)
        {
            toiletPaper.SetActive(true);
        }

        if (itemToUnlock == 4)
        {
            cowPot.SetActive(true);
            gameEnded = true;
            winner = true;
        }
    }

   public void StartGame()
    {

        if (isServer)
        {
            cowIntroCanvas.gameObject.SetActive(false);
            cowCanvas.gameObject.SetActive(true);
            farmerCanvas.gameObject.SetActive(false);
            isGameStartClicked = true;

            fireButton = cowCanvas.transform.GetChild(1).GetComponent<Button>();
            superFireButton = cowCanvas.transform.GetChild(2).GetComponent<Button>();
            networkManager.setUIObjects(pointer, fireButton, superFireButton);
        }
        else
        {
            farmerIntroCanvas.gameObject.SetActive(false);
            farmerCanvas.gameObject.SetActive(true);
            cowCanvas.gameObject.SetActive(false);
            pointer.SetActive(false);
        }
    }
   

    IEnumerator endGame()
    {
        popUpCanvas.gameObject.SetActive(false);
        if (!isServer)
        {
            if (winner)
            {
                endingImg.texture = winnerImg;
                endingTitle.text = "Je hebt gewonnen";
            } 
        }
        else
        {
            if (!winner)
            {
                endingImg.texture = winnerImg;
                endingTitle.text = "Je hebt gewonnen";
            }
        }
        
        endingCanvas.gameObject.SetActive(true);

        cowCanvas.gameObject.SetActive(false);
        farmerCanvas.gameObject.SetActive(false);

        yield return new WaitForSeconds(10);

        endingCanvas.gameObject.SetActive(false);
        restartGame();

    }


    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void openQuitCanvas()
    {
        quitCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void cancelQuit()
    {
        quitCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
