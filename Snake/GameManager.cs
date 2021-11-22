using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Snake
{
    public class Ranking
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
    }
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
            level = 1 + score / 5;
            if (level > 10)
                speed = 20;
            else
                speed = 100 - (level - 1) * 10;
        }

        public void Reset()
        {
            score = 0;
            speed = 100;
            level = 1;
        }

        public void SaveXML(List<Ranking> rankings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Ranking>));

            using (FileStream stream = File.OpenWrite("saveFile.xml"))
            {
                serializer.Serialize(stream, rankings);
            }
        }

        public List<Ranking> LoadXML()
        {
            var serializer = new XmlSerializer(typeof(List<Ranking>));
            
            List<Ranking> dezerializedList;
            
            string path = "saveFile.xml";

            if (!File.Exists(path))
            {
                dezerializedList = new List<Ranking>();

                for (int i = 0; i < 10; i++)
                {
                    Ranking temp = new Ranking();
                    temp.Name = "AAA";
                    temp.Score = 0;
                    temp.Level = 1;

                    dezerializedList.Add(temp);
                }
            }
            
            else
            {
                using (var stream = File.OpenRead(path))
                {
                    dezerializedList = (List<Ranking>)(serializer.Deserialize(stream));
                }
            }
            return dezerializedList;
        }
    }
}
