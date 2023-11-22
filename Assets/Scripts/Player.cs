

using UnityEngine;

namespace Element
{
    public class Player : MonoBehaviour
    {
        public string playerName;
        public string discord;
        public string inGame;
        public float score;
        public string team;

        public Player(string playerName, string discord, string inGame, float score, string team)
        {
            this.playerName = playerName;
            this.discord = discord;
            this.inGame = inGame;
            this.score = score;
            this.team = team;
        }
    }
}

