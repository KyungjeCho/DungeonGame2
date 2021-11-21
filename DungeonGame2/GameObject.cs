using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonGame
{
    abstract public partial class GameObject
    {
        public Vector2 Position { get; set; }
        public Panel Panel;
        public bool isVisible = true;
        public abstract void Draw();
    }

    public class Map : GameObject
    {
        public Map(Panel _p)
        {
            this.Panel = _p;
        }
        public override void Draw()
        {
            if (isVisible is false)
                return;

            for(int i = 0; i < Panel.Height; i ++)
            {
                for (int j = 0; j < Panel.Width; j++)
                {
                    if (
                        i == 0 || 
                        j == 0 || 
                        i == Panel.Height - 1 || 
                        j == Panel.Width - 1)
                    {
                        Panel.buffer[i][j] = "□";
                    }
                }
            }
        }
    }

    public class TextObject : GameObject
    {
        public String Text { get; set; }

        public TextObject(Panel _p, Vector2 _position, string _t)
        {
            Panel = _p;
            Position = _position;
            Text = _t;
        }
        public override void Draw()
        {
            if (isVisible is false)
                return;

            for (int i = 0; i < Text.Length; i++)
            {
                char current_char = char.Parse(Text.Substring(i, 1));
                Panel.buffer[Position.Y][Position.X + i] = Text.Substring(i, 1);
                if (current_char < 128)
                    Panel.buffer[Position.Y][Position.X + i] += " ";
            }
        }
    }

    public class Character : GameObject
    {
        public TextObject Name { get; set; }
        public int Hp { get; set; }

        public Character(Vector2 _p, TextObject _t, int _hp)
        {
            Position = _p;
            Name = _t;
            Hp = _hp;
        }

        public override void Draw()
        {
            
        }
    }

    public class Snake : GameObject
    {
        ConsoleColor color = ConsoleColor.Yellow;
        string shape = "■";
        public Vector2 direction;

        public int speed = 50;

        public Snake(Panel _p, Vector2 _position)
        {
            Panel = _p;
            Position = _position;
            direction = new Vector2(0, 0);
        }

        public void Move()
        {
            Position.X += direction.X;
            Position.Y += direction.Y;
        }
        public override void Draw()
        {
            if (isVisible is false)
                return;

            Panel.buffer[Position.Y][Position.X] = shape;
            Panel.colorMap[Position.Y][Position.X] = color;
        }
    }
}
