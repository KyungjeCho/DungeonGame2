using System;
using System.Collections.Generic;

namespace Snake
{
    public partial class Vector2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2()
        {
            this.X = 0;
            this.Y = 0;
        }
        public Vector2(int _x, int _y)
        {
            this.X = _x;
            this.Y = _y;
        }
    }

    public partial class Panel
    {
        public Vector2 Position { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public List<List<string>> buffer = new List<List<string>>();
        public List<List<ConsoleColor>> colorMap = new List<List<ConsoleColor>>();
        public Screen screen;
        public List<GameObject> gameObjects = new List<GameObject>();

        public Panel(Screen _s, int _x, int _y, int _width, int _height, bool isMap=true)
        {
            _s.AddPanel(this);
            this.screen = _s;
            this.Position = new Vector2(_x, _y);
            this.Height = _height;
            this.Width = _width;
            
            for (int i = 0; i < this.Height; i++)
            {
                List<string> temp = new List<string>();
                List<ConsoleColor> colorTemp = new List<ConsoleColor>();

                for (int j = 0; j < this.Width; j++)
                {
                    temp.Add("  ");
                    colorTemp.Add(ConsoleColor.White);
                }
                buffer.Add(temp);
                colorMap.Add(colorTemp);
            }

            if (isMap)
                this.AddGameObject(new Map(this));
        }

        public void AddGameObject(GameObject _go)
        {
            gameObjects.Add(_go);
        }

        private void ResetPanel()
        {
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    this.buffer[i][j] = "  ";
                    this.colorMap[i][j] = ConsoleColor.White;
                }
            }
        }
        public void Draw()
        {
            // Panel reset
            ResetPanel();

            // 모든 오브젝트 Draw
            foreach (var go in gameObjects)
            {
                go.Draw();
            }

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    this.screen.buffer
                        [i + this.Position.Y]
                        [j + this.Position.X]
                        = this.buffer[i][j];
                    this.screen.colorMap
                        [i + this.Position.Y]
                        [j + this.Position.X]
                        = this.colorMap[i][j];
                }
            }
        }
    }

    public partial class Screen
    {
        public int Height { get; set; }
        public int Width { get; set; }


        public List<List<string>> buffer = new List<List<string>>();
        public List<Panel> Panels = new List<Panel>();
        public List<List<ConsoleColor>> colorMap = new List<List<ConsoleColor>>();

        public Screen(int _height, int _width)
        {
            this.Height = _height;
            this.Width = _width;

            InitializeBuffer();
        }

        public void AddPanel(Panel _p)
        {
            Panels.Add(_p);
        }

        // 리스트 초기화
        public void InitializeBuffer()
        {
            for (int i = 0; i < this.Height; i++)
            {
                List<string> temp = new List<string>();
                List<ConsoleColor> colorTemp = new List<ConsoleColor>();

                for (int j = 0; j < this.Width; j++)
                {
                    temp.Add("  ");
                    colorTemp.Add(ConsoleColor.White);
                }

                temp.Add("\n");
                buffer.Add(temp);

                colorTemp.Add(ConsoleColor.White);
                colorMap.Add(colorTemp);
            }
        }

        // 스크린 버퍼 리셋
        public void ResetBuffer()
        {
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    buffer[i][j] = "  ";
                }
            }
        }

        // 스크린 버퍼 -> 콘솔 출력 
        public void PrintBufferToScreen()
        {
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width + 1; j++)
                {
                    Console.ForegroundColor = colorMap[i][j];
                    Console.Write(buffer[i][j]);
                }

            }
        }

        public void Draw()
        {
            foreach (var p in Panels)
            {
                p.Draw();
            }
        }
    }
}
