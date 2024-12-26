using GameFramework.Localization;
using System;
using Game.Gameplay;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Client
{
    /// <summary>
    /// 流程 => 启动器。
    /// </summary>
    public class ProcedureYooLaunch : ProcedureBase
    {

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            // 语言配置：设置当前使用的语言，如果不设置，则默认使用操作系统语言
            InitLanguageSettings();

            // 声音配置：根据用户配置数据，设置即将使用的声音选项
            InitSoundSettings();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            // 运行一帧即切换到 Splash 展示流程
            ChangeState<ProcedureYooSplash>(procedureOwner);
        }

        private void InitLanguageSettings()
        {
            bool isEditorMode = Application.platform == RuntimePlatform.WindowsEditor || 
                                Application.platform == RuntimePlatform.OSXEditor || 
                                Application.platform == RuntimePlatform.LinuxEditor;
            if (isEditorMode && GameEntry.Base.EditorLanguage != Language.Unspecified)
            {
                // 编辑器资源模式直接使用 Inspector 上设置的语言
                return;
            }

            Language language = GameEntry.Localization.Language;
            if (GameEntry.Setting.HasSetting(Constant.Setting.Language))
            {
                try
                {
                    string languageString = GameEntry.Setting.GetString(Constant.Setting.Language);
                    language = (Language)Enum.Parse(typeof(Language), languageString);
                }
                catch(Exception exception)
                {
                    Log.Error("Init language error, reason {0}",exception.ToString());
                }
            }

            if (language != Language.English
                && language != Language.ChineseSimplified
                && language != Language.ChineseTraditional
                && language != Language.Korean)
            {
                // 若是暂不支持的语言，则使用英语
                language = Language.English;

                GameEntry.Setting.SetString(Constant.Setting.Language, language.ToString());
                GameEntry.Setting.Save();
            }

            GameEntry.Localization.Language = language;
            Log.Info("Init language settings complete, current language is '{0}'.", language.ToString());
        }

        private void InitSoundSettings()
        {
            //@TODO:
            // GameModule.Sound.Mute("Music", GameModule.Setting.GetBool(Constant.Setting.MusicMuted, false));
            // GameModule.Sound.SetVolume("Music", GameModule.Setting.GetFloat(Constant.Setting.MusicVolume, 0.5f));
            // GameModule.Sound.Mute("Sound", GameModule.Setting.GetBool(Constant.Setting.SoundMuted, false));
            // GameModule.Sound.SetVolume("Sound", GameModule.Setting.GetFloat(Constant.Setting.SoundVolume, 0.5f));
            // GameModule.Sound.Mute("UISound", GameModule.Setting.GetBool(Constant.Setting.UISoundMuted, false));
            // GameModule.Sound.SetVolume("UISound", GameModule.Setting.GetFloat(Constant.Setting.UISoundVolume, 0.5f));
            
            
            Log.Info("Init sound settings complete.");
        }
    }
}
