﻿
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Client
{
    
public enum UpdateType
{
    None = 0,

    //底包更新
    PackageUpdate = 1,

    //资源更新
    ResourceUpdate = 2,
}

public enum UpdateStyle
{
    None = 0,
    Froce = 1, //强制
    Optional = 2, //非强制
}

public enum UpdateNotice
{
    None = 0,
    Notice = 1, //提示
    NoNotice = 2, //非提示
}

public enum GameStatus
{
    First = 0,
    AssetLoad = 1
}

/// <summary>
/// 资源存放地
/// </summary>
[Serializable]
public class ResourcesArea
{
    [Tooltip("资源管理类型")] [SerializeField] private string m_ResAdminType = "Default";

    public string ResAdminType
    {
        get { return m_ResAdminType; }
    }

    [Tooltip("资源管理编号")] [SerializeField] private string m_ResAdminCode = "0";

    public string ResAdminCode
    {
        get { return m_ResAdminCode; }
    }

    [SerializeField] private ServerTypeEnum m_ServerType = ServerTypeEnum.Intranet;

    public ServerTypeEnum ServerType
    {
        get { return m_ServerType; }
    }

    [Tooltip("是否在构建资源的时候清理上传到服务端目录的老资源")] [SerializeField]
    private bool m_CleanCommitPathRes = true;

    public bool CleanCommitPathRes
    {
        get { return m_CleanCommitPathRes; }
    }

    [Tooltip("内网地址")] [SerializeField] private string m_InnerResourceSourceUrl = "http://127.0.0.1:8081";

    public string InnerResourceSourceUrl
    {
        get { return m_InnerResourceSourceUrl; }
    }

    [Tooltip("外网地址")] [SerializeField] private string m_ExtraResourceSourceUrl = "http://127.0.0.1:8081";

    public string ExtraResourceSourceUrl
    {
        get { return m_ExtraResourceSourceUrl; }
    }

    [Tooltip("正式地址")] [SerializeField] private string m_FormalResourceSourceUrl = "http://127.0.0.1:8081";

    public string FormalResourceSourceUrl
    {
        get { return m_FormalResourceSourceUrl; }
    }
}

[Serializable]
public class ServerIpAndPort
{
    public string ServerName;
    public string Ip;
    public int Port;
}

[Serializable]
public class ServerChannelInfo
{
    public string ChannelName;
    public string CurUseServerName;
    public List<ServerIpAndPort> ServerIpAndPorts;
}

[Serializable]
public class FrameworkGlobalSettings
{
    [SerializeField] [Tooltip("脚本作者名")] private string m_ScriptAuthor = "Default";

    public string ScriptAuthor
    {
        get { return m_ScriptAuthor; }
    }

    [SerializeField] [Tooltip("版本")] private string m_ScriptVersion = "0.1";

    public string ScriptVersion
    {
        get { return m_ScriptVersion; }
    }

    [SerializeField] private AppStageEnum m_AppStage = AppStageEnum.Debug;

    public AppStageEnum AppStage
    {
        get { return m_AppStage; }
    }

    [Header("Resources")] [Tooltip("资源存放地")] [SerializeField]
    private ResourcesArea m_ResourcesArea;

    public ResourcesArea ResourcesArea
    {
        get { return m_ResourcesArea; }
    }

    [Header("SpriteCollection")] [SerializeField]
    private string m_AtlasFolder = "Assets/AssetRaw/Atlas";

    public string AtlasFolder
    {
        get { return m_AtlasFolder; }
    }

    public string WindowsAppUrl = "http://127.0.0.1";
    public string MacOSAppUrl = "http://127.0.0.1";
    public string IOSAppUrl = "http://127.0.0.1";
    public string AndroidAppUrl = "http://127.0.0.1";
    [Header("Server")] [SerializeField] private string m_CurUseServerChannel;

    public string CurUseServerChannel
    {
        get => m_CurUseServerChannel;
    }

    [SerializeField] private List<ServerChannelInfo> m_ServerChannelInfos;

    public List<ServerChannelInfo> ServerChannelInfos
    {
        get => m_ServerChannelInfos;
    }
    
    [SerializeField] private string @namespace = "GameLogic";
    
    [Header("PreLoad")] [SerializeField]
    private string[] m_PreLoadTags = new []{"PRELOAD"};

    public string[] PreLoadTags => m_PreLoadTags;

    public string NameSpace => @namespace;

    [SerializeField] private string @uiWidgetName = "m_item";

    public string UIWidgetName => @uiWidgetName;
    
    [SerializeField] private List<ScriptGenerateRuler> scriptGenerateRule = new List<ScriptGenerateRuler>()
    {
        new ScriptGenerateRuler("m_go", "GameObject"),
        new ScriptGenerateRuler("m_item", "GameObject"),
        new ScriptGenerateRuler("m_tf", "Transform"),
        new ScriptGenerateRuler("m_rect", "RectTransform"),
        new ScriptGenerateRuler("m_text", "Text"),
        new ScriptGenerateRuler("m_richText", "RichTextItem"),
        new ScriptGenerateRuler("m_btn", "Button"),
        new ScriptGenerateRuler("m_img", "Image"),
        new ScriptGenerateRuler("m_rimg", "RawImage"),
        new ScriptGenerateRuler("m_scrollBar", "Scrollbar"),
        new ScriptGenerateRuler("m_scroll", "ScrollRect"),
        new ScriptGenerateRuler("m_input", "InputField"),
        new ScriptGenerateRuler("m_grid", "GridLayoutGroup"),
        new ScriptGenerateRuler("m_hlay", "HorizontalLayoutGroup"),
        new ScriptGenerateRuler("m_vlay", "VerticalLayoutGroup"),
        new ScriptGenerateRuler("m_red", "RedNoteBehaviour"),
        new ScriptGenerateRuler("m_slider", "Slider"),
        new ScriptGenerateRuler("m_group", "ToggleGroup"),
        new ScriptGenerateRuler("m_curve", "AnimationCurve"),
        new ScriptGenerateRuler("m_canvasGroup", "CanvasGroup"),
#if ENABLE_TEXTMESHPRO
        new ScriptGenerateRuler("m_tmp","TextMeshProUGUI"),
#endif
    };

    public List<ScriptGenerateRuler> ScriptGenerateRule => scriptGenerateRule;
}

[Serializable]
public class ScriptGenerateRuler
{
    public string uiElementRegex;
    public string componentName;

    public ScriptGenerateRuler(string uiElementRegex, string componentName)
    {
        this.uiElementRegex = uiElementRegex;
        this.componentName = componentName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ScriptGenerateRuler))]
public class ScriptGenerateRulerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        var uiElementRegexRect = new Rect(position.x, position.y, 120, position.height);
        var componentNameRect = new Rect(position.x + 125, position.y, 150, position.height);
        EditorGUI.PropertyField(uiElementRegexRect, property.FindPropertyRelative("uiElementRegex"), GUIContent.none);
        EditorGUI.PropertyField(componentNameRect, property.FindPropertyRelative("componentName"), GUIContent.none);
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
#endif
}