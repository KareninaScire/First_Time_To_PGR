using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Image imgBG;
    public Image imgCharacter;
    public Image imgCharacter2;
    public Text textName;
    public Text textTalkLine;
    public Text textEnergyValue;
    public Text textFavorValue;
    public GameObject talkLineGo;//对话框父对象游戏物体
    public GameObject empChoiceUIGo;//多项选择框父对象
    public GameObject favorUIGo;

    public Transform[] charactersPosTrans;
    public GameObject[] choiceUIGos;
    public Text[] textChoiceUIs;
    

    private void Awake()
    {
        Instance = this;
    }
    /*设置背景图片*/
    public void SetBGImageSprite(string spriteName)
    {
        imgBG.sprite = Resources.Load<Sprite>("Sprites/BG/" + spriteName);
    }
    /*设置角色图片*/
    public void ShowCharacter(string name,int characterId=0,int scriptID=0)
    {
        ShowOrHideTalkLine();
        CloseChoiceUI();
        textName.text = name;
        if (name=="害羞")
        {
            textName.text = "卡列尼娜";
        }
        if (characterId==0)
        {
            imgCharacter.sprite = Resources.Load<Sprite>("Sprites/Characters/" + name);
            imgCharacter.gameObject.SetActive(true);
        }else
        {
            imgCharacter2.sprite = Resources.Load<Sprite>("Sprites/Characters/" + name);
            imgCharacter2.gameObject.SetActive(true);
        }

        if (scriptID==114514)
        {
            imgCharacter2.gameObject.SetActive(false);
        }
    }
    /*显示好感度UI*/
    public void ShowFavorUI(bool show)
    {
        favorUIGo.SetActive(show);
    }    
    /*更新对话内容*/
    public void UpdateTalkLineText(string dialogueContent)
    {
        textTalkLine.text = dialogueContent;
    }
    /*设置角色位置*/
    public void SetPos(int posID,Image imgTargetChareacter,bool ifRotate=false)
    {
        imgTargetChareacter.transform.localPosition = charactersPosTrans[posID-1].localPosition;
        if (ifRotate)
        {
            imgTargetChareacter.transform.eulerAngles = new Vector3(0,180,0);
        }
        else
        {
            imgTargetChareacter.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    public void SetCharacterPos(int posID,bool ifRotate=false,int charId=0)
    {
        if (charId==0)
        {
            SetPos(posID,imgCharacter,ifRotate);
        }
        else
        {
            SetPos(posID,imgCharacter2,ifRotate);
        }
    }
    /*更新精力值UI*/
    public void UpdateEnergyValue(int value = 0,string name=null)
    {
        textEnergyValue.text = value.ToString();
    }
    /*更新好感度UI*/
    public void UpdateFavorValue(int value = 0,string name=null)
    {
        textFavorValue.text = value.ToString();
    }
    /*显示或隐藏对话框*/
    public void ShowOrHideTalkLine(bool show=true)
    {
        talkLineGo.SetActive(show);
    }
    /*显示多项选择对话框 选项数量 选项文本内容*/
    public void ShowChoiceUI(int choiceNum, string[] choiceContent)
    {
        empChoiceUIGo.SetActive(true);
        ShowOrHideTalkLine(false);
        for (int i = 0; i < choiceUIGos.Length; i++)
        {
            choiceUIGos[i].SetActive(false);
        }

        for (int i = 0; i < choiceNum; i++)
        {
            choiceUIGos[i].SetActive(true);
            textChoiceUIs[i].text = choiceContent[i];
        }
    }
    /*关闭多项选择对话框UI*/
    public void CloseChoiceUI()
    {
        empChoiceUIGo.SetActive(false);
    }
}