using System.IO;
using System.Text;
using Game.Core;
using GameFramework;
using Newtonsoft.Json;
using Log = GameFramework.GameFrameworkLog;

namespace Game.Core
{

    public static class GameplayTagHelper
    {
        private static string s_TagFilePath = string.Empty;
        private const string DefaultTagFilePath = "Assets/GameRes/Configs/GameplayTag.json";

        private static GameplayTagTree m_TagTree;

        public static GameplayTagTree TagTree
        {
            get
            {
                if (m_TagTree == null)
                {
                    m_TagTree = InitializeGameplayTag();
                }

                return m_TagTree;
            }
            set => m_TagTree = value;
        }

        public static void SaveTagFile(string relativePathToProject = DefaultTagFilePath)
        {

            // 获取完整路径（基于当前工作目录）
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePathToProject);

            // 获取文件的目录路径
            string directoryPath = Path.GetDirectoryName(fullPath);

            // 检查目录是否存在，如果不存在则创建
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Log.Info($"目录不存在，创建目录：{directoryPath}");
            }

            if (!File.Exists(fullPath))
            {
                File.CreateText(fullPath).Dispose();
                Log.Info($"文件不存在，创建文件：{fullPath}");
            }

            string content = Utility.Json.ToJson(TagTree);

            File.WriteAllText(fullPath, content, Encoding.UTF8);

            Log.Info($"Tag已写入：{fullPath}");

            s_TagFilePath = fullPath;
        }

        internal static string ReadTagFile()
        {
            string tagFilePath = s_TagFilePath;
            if (string.IsNullOrEmpty(tagFilePath))
            {
                tagFilePath = DefaultTagFilePath;
            }

            if (!File.Exists(tagFilePath))
            {
                return string.Empty;
            }

            return File.ReadAllText(tagFilePath);
        }

        internal static GameplayTagTree InitializeGameplayTag()
        {
            string tagContent = ReadTagFile();
            m_TagTree = Utility.Json.ToObject<GameplayTagTree>(tagContent);
            if (null == m_TagTree)
            {
                Log.Warning($"{nameof(GameplayTagTree)} 加载失败, 新建GameplayTagTree");
                m_TagTree = new GameplayTagTree();
            }

            return m_TagTree;
        }

    }
}