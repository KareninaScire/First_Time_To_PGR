using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using Newtonsoft.Json; 
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public List<ScriptData> scriptDatas;
    public int scriptIndex;
    public int energyValue; // 精力值
    public Dictionary<string, int> favorAbilityDict; // 每个角色对玩家的好感度
    public Dictionary<int, Action<object>> eventDict;
    public GameObject hitPointGo;
    public GameObject creditsPanel;
    public GameObject startButton; // 引用开始游戏按钮的 GameObject
    string path = Path.Combine(Application.streamingAssetsPath, "ScriptDatas.json");

    private void Awake()
    {
        Instance = this;
    }

    public void InitializeGame()
    {
        startButton.SetActive(false);
        scriptDatas = LoadScriptData(path);
        scriptIndex = 0;
        energyValue = 50;
        favorAbilityDict = new Dictionary<string, int>()
        {
            {"害羞", 50},
            {"卡列尼娜", 50},
            {"露西亚", 90}
        };
        eventDict = new Dictionary<int, Action<object>>()
        {
            {1, StartHitPointEvent}
        };
        for (int i = 0; i < scriptDatas.Count; i++)
        {
            scriptDatas[i].scriptIndex = i;
        }

        HandleData();
    }

    private void Start()
    {
        // 显示开始按钮
        startButton.SetActive(true);
    }

    /*处理每一条剧情数据*/
    private void HandleData()
    {
        //游戏结束
        if (scriptIndex >= scriptDatas.Count)
        {
            EndGame();
            return;
        }
        //显示好感度UI
        ShowFavorUI();
        //音频处理
        PlaySound(scriptDatas[scriptIndex].soundType);
        //UI处理
        if (scriptDatas[scriptIndex].loadType == 1)
        {//背景
            StartCoroutine(FadeEffect.instance.FadeOutAndIn(() =>
            {
                SetBGImageSprite(scriptDatas[scriptIndex].spriteName);
                LoadNextScript();
            }));
        }
        else if (scriptDatas[scriptIndex].loadType == 2)
        {//人物
            HandleCharacter();
        }
        else if (scriptDatas[scriptIndex].loadType == 3)
        {
            switch (scriptDatas[scriptIndex].eventID)
            {
                case 1:
                    ShowChoiceUI(scriptDatas[scriptIndex].eventData,
                        GetChoiceContent(scriptDatas[scriptIndex].eventData));
                    break;
                case 2:
                    SetScriptIndex();
                    break;
                case 3:
                    eventDict[scriptDatas[scriptIndex].eventData](null);
                    break;
            }
        }
        else
        {
            LoadNextScript();
        }
    }
    /*处理人物*/
    public void HandleCharacter()
    {
        //显示人物
        ShowCharacter(scriptDatas[scriptIndex].name,scriptDatas[scriptIndex].characterID);
        //更新对话框文本
        UpdateTalkLineText(scriptDatas[scriptIndex].dialogueContent);
        //设置人物位置
        SetCharacterPos(scriptDatas[scriptIndex].characterPos, scriptDatas[scriptIndex].ifRotate,scriptDatas[scriptIndex].characterID);
        //更新好感度和精力值
        ChangeEnergyValue(scriptDatas[scriptIndex].energyValue);
        ChangeFavorValue(scriptDatas[scriptIndex].favorability, scriptDatas[scriptIndex].name);
    }
    
    //加载下一条剧情数据
    public void LoadNextScript(int addNum=1)
    {
        scriptIndex+=addNum;
        HandleData();
    }
    //设置背景图片
    private void SetBGImageSprite(string spriteName)
    {
        UIManager.Instance.SetBGImageSprite(spriteName);
    }
    //显示人物
    private void ShowCharacter(string name,int characterId=0)
    {
        UIManager.Instance.ShowCharacter(name, characterId,scriptDatas[scriptIndex].scriptID);
    }
    //更新对话框文本
    private void UpdateTalkLineText(string dialogueContent)
    {
        UIManager.Instance.UpdateTalkLineText(dialogueContent);
    }
    //改变角色位置
    private void SetCharacterPos(int posID,bool ifRotate=false,int characterId=0)
    {
        UIManager.Instance.SetCharacterPos(posID,ifRotate,characterId);
    }
    //播放音频
    public void PlaySound(int soundType)
    {
        switch (soundType)
        {
            case 1:
                AudioSourceManager.Instance.PlayDialogue(
                    scriptDatas[scriptIndex].name+"/"+scriptDatas[scriptIndex].soundPath);
                break;
            case 2:
                AudioSourceManager.Instance.PlaySound(scriptDatas[scriptIndex].soundPath);
                break;
            case 3:
                AudioSourceManager.Instance.PlayMusic(scriptDatas[scriptIndex].soundPath);
                break;
        }
    }
    //改变精力值 传入的值value是变化值而不是变化后的值
    public void ChangeEnergyValue(int value = 0)
    {
        if (value==0)
        {
            return;
        }else if(value>0)
        {
            AudioSourceManager.Instance.PlaySound("Energy");
        }
        
        energyValue += value;

        if (energyValue>=100)
        {
            energyValue = 100;
        }else if (energyValue <= 0)
        {
            energyValue = 0;
        }
        UpdateEnergyValue(energyValue);
    }
    //更新精力值UI
    public void UpdateEnergyValue(int value = 0)
    {
        UIManager.Instance.UpdateEnergyValue(value);
    }
    //改变好感度
    public void ChangeFavorValue(int value = 0,string name =null)
    {
        if (value==0)
        {
            return;
        }else if(value>0)
        {
            AudioSourceManager.Instance.PlaySound("Favor");
        }
        
        favorAbilityDict[name] += value;

        if (favorAbilityDict[name]>=100)
        {
            favorAbilityDict[name] = 100;
        }else if (favorAbilityDict[name] <= 0)
        {
            favorAbilityDict[name] = 0;
        }
        UpdateFavorValue(favorAbilityDict[name],name);
    }
    //更新好感度UI
    public void UpdateFavorValue(int value = 0,string name = null)
    {
        UIManager.Instance.UpdateFavorValue(value,name);
        if (name=="卡列尼娜")
        {
            UIManager.Instance.UpdateFavorValue(value,"害羞");
        }
        if (name=="害羞")
        {
            UIManager.Instance.UpdateFavorValue(value,"卡列尼娜");
        }
    }
    //显示好感度
    public void ShowFavorUI()
    {
        if (scriptDatas[scriptIndex].scriptID==114514)
        {
            UIManager.Instance.ShowFavorUI(true);
        }
    }
    //显示多项选择对话框
    public void ShowChoiceUI(int choiceNum, string[] choiceContent)
    {
        UIManager.Instance.ShowChoiceUI(choiceNum,choiceContent);
    }
    //获取选项上的文本
    public string[] GetChoiceContent(int num)
    {
        string[] choiceContent = new string[num];
        for (int i = 0; i < num; i++)
        {
            choiceContent[i] = scriptDatas[scriptIndex+1+i].dialogueContent;
        }
        return choiceContent;
    }
    //把剧本跳转到对应位置
    public void SetScriptIndex(int index = 0)
    {
        for (int i = 0; i < scriptDatas.Count; i++)
        {
            if (scriptDatas[scriptIndex+index].eventData==scriptDatas[i].scriptID)
            {
                scriptIndex = scriptDatas[i].scriptIndex;
                break;
            }
        }
        HandleData();
    }
    //训练事件
    public void StartHitPointEvent(object src)
    {
        UIManager.Instance.ShowOrHideTalkLine(false);
        hitPointGo.SetActive(true);
    }
    //解析器
    private List<ScriptData> LoadScriptData(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<ScriptData>();
        }

        string jsonContent = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<ScriptData>>(jsonContent);
    }
    //场景渐变
    public void SwitchSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeEffect.instance.FadeOutAndIn(() =>
        {
            SceneManager.LoadScene(sceneName);
        }));
    }
    //游戏结束播放名单
    public void EndGame()
    {
        creditsPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
