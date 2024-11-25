using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*剧本数据*/
public class ScriptData
{
    public int loadType;//载入资源类型 1.背景 2.人物 3.事件
    public int characterPos;//1.左 2.右 3.中
    public int soundType;//音频类型 1.对话 2.音效 3.BGM
    public int favorability;//好感度
    public int energyValue;//精力值（改变值而非当前值
    public int characterID;//三人对话时人物ID
    public int eventID;//处理事件的ID 1.显示选择项 2.跳转到指定剧本位置
                       //3.特殊事件
    public int eventData;//事件数据id=1时.几个选项 id=2时.要跳转到的标记位
                         //id=3时.特殊事件的事件ID
    public int scriptID;//剧本标记位，用于跳转 与eventID=2时的eventData对应
    public int scriptIndex;//剧本索引
    public string name;//角色名称(改变值而非当前值
    public string spriteName;//图片资源路径
    public string dialogueContent;//对话内容
    public string soundPath;//音频路径
    public bool ifRotate;//立绘是否翻转
}