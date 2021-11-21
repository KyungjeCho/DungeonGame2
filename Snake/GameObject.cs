using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Snake

{
    abstract public partial class GameObject
    {
        public Vector2 Position { get; set; }
        public Panel Panel;
        public bool isVisible = true;
        public abstract void Draw();
        public abstract bool Collision(GameObject _go);
    }

    public class Map : GameObject
    {
        public Map(Panel _p)
        {
            _p.AddGameObject(this);
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

        public override bool Collision(GameObject _go)
        {
            if (_go.Position.X == 0)
                return true;
            if (_go.Position.X == Panel.Width - 1)
                return true;
            if (_go.Position.Y == 0)
                return true;
            if (_go.Position.Y == Panel.Height - 1)
                return true;
            return false;
        }
    }

    public class TextObject : GameObject
    {
        public String Text { get; set; }

        public TextObject(Panel _p, Vector2 _position, string _t)
        {
            _p.AddGameObject(this);
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

        public override bool Collision(GameObject _go)
        {
            return false;
        }
    }

    public class AsciiObject : GameObject
    {
        public string Text { get; set; }

        public AsciiObject(Panel _p, Vector2 _position, string _t)
        {
            _p.AddGameObject(this);
            Panel = _p;
            Position = _position;
            Text = _t;
        }
        public override void Draw()
        {
            if (isVisible is false)
                return;

            
            int j = 0;
            if (Text.Length % 2 != 0)
                Text += " ";
            for (int i = 0; i < Text.Length; i+=2)
            {
                Panel.buffer[Position.Y][Position.X + j] = Text.Substring(i, 2);
                j++;
            }
        }

        public override bool Collision(GameObject _go)
        {
            return false;
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

        public override bool Collision(GameObject _go)
        {
            return false;
        }
    }

    public class Snake : GameObject
    {
        List<Body> bodies = new List<Body>();
        public Body tail;
        public Vector2 prevPosition;
        public Vector2 direction;

        public int speed = 50;

        public Snake(Panel _p, Vector2 _position)
        {
            _p.AddGameObject(this);
            Panel = _p;
            Position = _position;
            direction = new Vector2(0, 0);
            bodies.Add(new Body(_p, Position));
            tail = bodies[0];
        }

        public void Move()
        {
            // 테일 부터 하나씩 
            for(int i = bodies.Count - 1; i > 0; i--)
            {
                bodies[i].Position.X = bodies[i - 1].Position.X;
                bodies[i].Position.Y = bodies[i - 1].Position.Y;

            }
            prevPosition = Position;
            Position.X += direction.X;
            Position.Y += direction.Y;
            bodies[0].Position.X = Position.X;
            bodies[0].Position.Y = Position.Y;
        }

        public void AddBody(Vector2 _position)
        {
            bodies.Add(new Body(Panel, new Vector2(_position.X, _position.Y)));
            tail = bodies[bodies.Count - 1];
        }
        public override void Draw()
        {
            if (isVisible is false)
                return;

            foreach (var b in bodies)
            {
                b.Draw();
            }
        }

        public override bool Collision(GameObject _go)
        {
            for (int i = 1; i < bodies.Count; i++)
            {
                if (Position.X == bodies[i].Position.X && Position.Y == bodies[i].Position.Y)
                    return true;
            }
            return false;
        }
    }

    public class Body : GameObject
    {
        ConsoleColor color = ConsoleColor.Yellow;
        string shape = "■";

        public Body(Panel _p, Vector2 _position)
        {
            _p.AddGameObject(this);
            Panel = _p;
            Position = _position;
        }

        public override bool Collision(GameObject _go)
        {
            return false;
        }

        public override void Draw()
        {
            if (isVisible is false)
                return;

            Panel.buffer[Position.Y][Position.X] = shape;
            Panel.colorMap[Position.Y][Position.X] = color;
        }
    }

    public class Apple : GameObject
    {
        ConsoleColor color = ConsoleColor.Red;
        string shape = "★";

        public Apple(Panel _p)
        {
            _p.AddGameObject(this);
            Panel = _p;
            Position = new Vector2(
                new Random().Next(1, Panel.Width - 1),
                new Random().Next(1, Panel.Height - 1)
            );
        }
        public override bool Collision(GameObject _go)
        {
            if (_go.Position.X == Position.X && _go.Position.Y == Position.Y)
                return true;
            return false;
        }

        public override void Draw()
        {
            if (isVisible is false)
                return;

            Panel.buffer[Position.Y][Position.X] = shape;
            Panel.colorMap[Position.Y][Position.X] = color;
        }

        public void Move()
        {
            Position.X = new Random().Next(1, Panel.Width - 1);
            Position.Y = new Random().Next(1, Panel.Height - 1);
        }
    }

    public class IOTextObject : GameObject
    {
        public ConsoleColor color = ConsoleColor.White;
        List<List<string>> buffer = new List<List<string>>();

        public IOTextObject(Panel _p, string path, Vector2 _position)
        {
            _p.AddGameObject(this);
            Panel = _p;
            Position = _position;

            using (StreamReader sr = File.OpenText(path))
            {
                string s;

                while ((s = sr.ReadLine()) != null)
                {
                    List<string> temp = new List<string>();
                    if (s.Length % 2 != 0)
                        s += " ";

                    for (int i = 0; i < s.Length; i+=2)
                    {
                        temp.Add(s.Substring(i, 2));
                    }
                    buffer.Add(temp);
                }
            }
        }
        public override bool Collision(GameObject _go)
        {
            return false;
        }

        public override void Draw()
        {
            for (int i = 0; i < buffer.Count; i++)
            {
                for (int j = 0; j < buffer[i].Count; j++)
                {
                    Panel.buffer[Position.Y + i][Position.X + j] = buffer[i][j];
                    Panel.colorMap[Position.Y + i][Position.X + j] = color;
                }
            }

        }
    }

    public class RightArrow : GameObject
    {
        public ConsoleColor color = ConsoleColor.Yellow;
        public string shape = "▶";

        public RightArrow (Panel _p, Vector2 _position)
        {
            _p.AddGameObject(this);
            Panel = _p;
            Position = _position;
        }
        public override bool Collision(GameObject _go)
        {
            return false;
        }

        public override void Draw()
        {
            Panel.buffer[Position.Y][Position.X] = shape;
            Panel.colorMap[Position.Y][Position.X] = color;
        }
    }
}
