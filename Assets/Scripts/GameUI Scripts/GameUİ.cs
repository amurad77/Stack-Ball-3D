using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GameUİ : MonoBehaviour
{
    
    public GameObject homeUI, inGameUI;
    public GameObject allbtns;

    private bool btns;

    [Header("PreGame")]
    public Button soundBtn;
    public Sprite soundOnSpr, soundOffSpr;

    [Header("InGame")]
    public Image levelSlider;
    public Image currentLevelImg;
    public Image nextLevelImg;

    private Material playerMat;
    private Player player;


    void Awake()
    {
        playerMat = FindObjectOfType<Player>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        player = FindObjectOfType<Player>();

        levelSlider.transform.parent.GetComponent<Image>().color = playerMat.color + Color.gray;
        levelSlider.color = playerMat.color;
        currentLevelImg.color = playerMat.color;
        nextLevelImg.color = playerMat.color;

        soundBtn.onClick.AddListener(() => SoundManager.instance.SoundOff());
    } 

    void Update()
    {

        if(player.playerState == Player.PlayerState.Prepare)
        {
            if(SoundManager.instance.sound && soundBtn.GetComponent<Image>().sprite != soundOnSpr)
                soundBtn.GetComponent<Image>().sprite = soundOnSpr;
            else if(!SoundManager.instance.sound && soundBtn.GetComponent<Image>().sprite != soundOffSpr)
                soundBtn.GetComponent<Image>().sprite = soundOffSpr;
        }

        if(Input.GetMouseButtonDown(0) && !IgnoreUI() && player.playerState == Player.PlayerState.Prepare)
        {
            player.playerState = Player.PlayerState.Playing;
            homeUI.SetActive(false);
            inGameUI.SetActive(true);

        }
    }

    private bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if(raycastResultList[i].gameObject.GetComponent<Ignore>() != null)
            {
                raycastResultList.RemoveAt(i);
                i--;
            }
        }

        return raycastResultList.Count > 0;
    }

    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }


    public void Settings()
    {
        btns = !btns;
        allbtns.SetActive(btns);
    }

}
