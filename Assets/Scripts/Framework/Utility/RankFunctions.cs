using System.Collections.Generic;
using UnityEngine;

namespace MarkFramework
{
    public class PlayerData
    {
        public int rankNum;
        public string playerName;
        public int score;
    }

    public class RankFunctions
    {
        private const string FileName = "PlayerData"; // JSON file name without extension
        public int RankRange { get; set; } = 50; // 默认排名范围为50

        public List<PlayerData> LoadPlayerData()
        {
            // 使用 JsonMgr 读取数据
            List<PlayerData> playersList = JsonMgr.Instance.LoadData<List<PlayerData>>(FileName, JsonType.LitJson);

            // 如果文件为空或不存在，返回空列表
            if (playersList == null || playersList.Count == 0)
            {
                Debug.LogWarning("No player data found, returning an empty list.");
                return new List<PlayerData>();
            }

            // 按 rankNum 排序后返回
            playersList.Sort((a, b) => a.rankNum.CompareTo(b.rankNum));
            return playersList;
        }

        public void WritePlayerData(List<PlayerData> playersList)
        {
            // 使用 JsonMgr 保存数据
            JsonMgr.Instance.SaveData(playersList, FileName, JsonType.LitJson);
            Debug.Log("Player data successfully saved using JsonMgr.");
        }

        public List<PlayerData> InsertPlayerData(PlayerData playerData, List<PlayerData> playersList)
        {
            // 比较 playerData 的 score，确定其排名
            int rank = 1;
            foreach (var player in playersList)
            {
                if (playerData.score < player.score)
                {
                    rank++;
                }
                else
                {
                    break;
                }
            }

            playerData.rankNum = rank;

            // 插入新玩家
            playersList.Insert(rank - 1, playerData);

            // 更新所有玩家的 rankNum
            for (int i = 0; i < playersList.Count; i++)
            {
                playersList[i].rankNum = i + 1;
            }

            // 移除超过 RankRange 范围的玩家
            if (playersList.Count > RankRange)
            {
                playersList.RemoveRange(RankRange, playersList.Count - RankRange);
            }

            return playersList;
        }
    }
}
