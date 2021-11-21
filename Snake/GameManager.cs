using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public sealed class GameManager
    {
        public int score = 0;
        public int speed = 100;
        public int level = 1;

        private GameManager() { }

        private static readonly Lazy<GameManager> _instance = new Lazy<GameManager>(() => new GameManager());

        public static GameManager Instance { get { return _instance.Value; } }

        public void checkLevel()
        {
            level = 1 + score / 10;
            if (level > 10)
                speed = 20;
            else
                speed = 100 - (level - 1) * 10;
        }
    }
}
