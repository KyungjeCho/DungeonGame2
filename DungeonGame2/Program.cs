using System;
using System.Collections.Generic;
using System.Threading;

namespace DungeonGame
{
    class Program
    {
        class Scene
        {
            private Screen Screen = new Screen(30, 30);
            Panel gamePanel;
            Panel uiPanel;

            Map map;
            Map map2;
            Snake snake;
            TextObject pause;

            Vector2 direction = new Vector2(0, 0);
            ConsoleKeyInfo keys;
            bool isStop = false;

            public Scene()
            {
                //생성 
                gamePanel = new Panel(Screen, 0, 0, 20, 20);
                uiPanel = new Panel(Screen, 20, 0, 10, 10);

                map = new Map(gamePanel);
                map2 = new Map(uiPanel);
                snake = new Snake(gamePanel, new Vector2(9, 9));
                pause = new TextObject(gamePanel, new Vector2(8, 9), "PAUSE");
                pause.isVisible = false;

                gamePanel.AddGameObject(map);
                gamePanel.AddGameObject(snake);
                gamePanel.AddGameObject(pause);

                uiPanel.AddGameObject(map2);

                // 사용할 패널들을 스크린에 추가합니다
                Screen.AddPanel(gamePanel);
                Screen.AddPanel(uiPanel);

                while(true)
                {
                    Update();
                    Render();
                }
            }
            
            private void Update()
            {
                Input();

                if(isStop is false)
                {
                    snake.isVisible = true;
                    pause.isVisible = false;
                    snake.direction = direction;
                    snake.Move();
                }
                else
                {
                    snake.isVisible = false;
                    pause.isVisible = true;
                }

                Thread.Sleep(snake.speed);
            }

            private void Render()
            {
                Console.SetCursorPosition(0, 0);
                Screen.Draw();
                Screen.PrintBufferToScreen();
            }

            private void Input()
            {
                if (Console.KeyAvailable)
                {
                    keys = Console.ReadKey(true);
                    switch (keys.Key)
                    {
                        case ConsoleKey.RightArrow:
                            direction.X = 1;
                            direction.Y = 0;
                            break;
                        case ConsoleKey.LeftArrow:
                            direction.X = -1;
                            direction.Y = 0;
                            break;
                        case ConsoleKey.UpArrow:
                            direction.X = 0;
                            direction.Y = -1;
                            break;
                        case ConsoleKey.DownArrow:
                            direction.X = 0;
                            direction.Y = 1;
                            break;
                        case ConsoleKey.S:
                            if (isStop)
                                isStop = false;
                            else
                                isStop = true;
                            break;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Console.SetWindowSize(200, 50);
            Console.CursorVisible = false;
            Scene Scene1 = new Scene();
        }
    }
}
