using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace Snake
{
    class Program
    {
        class IntroScene
        {
            private Screen Screen = new Screen(30, 30);

            Panel introPanel;

            TextObject start;
            TextObject manual;
            TextObject ranking;
            TextObject staff;
            TextObject quit;

            RightArrow rightArrow;
            List<TextObject> menu = new List<TextObject>();

            IOTextObject title;
            ConsoleKeyInfo keys;
            int select = 0;
            string path = "../../../../title.txt";

            bool isEnter = false;

            public IntroScene()
            {
                //패널 생성 
                introPanel = new Panel(Screen, 0, 0, 30, 30);

                // 게임 오브젝트 생성
                title = new IOTextObject(introPanel, path, new Vector2(5, 5));
                start = new TextObject(introPanel, new Vector2(0, 0), "START");
                manual = new TextObject(introPanel, new Vector2(0, 0), "MANUAL");
                ranking = new TextObject(introPanel, new Vector2(0, 0), "RANKING");
                staff = new TextObject(introPanel, new Vector2(0, 0), "STAFF");
                quit = new TextObject(introPanel, new Vector2(0, 0), "QUIT");
                rightArrow = new RightArrow(introPanel, new Vector2(8, 16));

                menu.Add(start);
                menu.Add(manual);
                menu.Add(ranking);
                menu.Add(staff);
                menu.Add(quit);

                int i = 0;
                foreach(var m in menu)
                {
                    m.Position = new Vector2(10, 14 + i);
                    i += 3;
                }

                while (true)
                {
                    if (isEnter)
                    {
                        if (select == 0)
                        {
                            new InGameScene();
                            break;
                        }
                        else if (select == 1)
                        {
                            
                        }
                        else if (select == 2)
                        {
                            
                        }
                        else if (select == 3)
                        {
                            new StaffScene();
                        }
                        else if (select == 4)
                        {
                            break;
                        }
                        isEnter = false;
                    }
                    Update();
                    Render();
                }
            }

            private void Update()
            {
                Input();

                title.color = (ConsoleColor)new Random().Next(1, 16);

                rightArrow.Position.Y = 14 + (select * 3);

                Thread.Sleep(100);
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
                        case ConsoleKey.UpArrow:
                            if (select > 0)
                                select--;
                            break;
                        case ConsoleKey.DownArrow:
                            if (select < menu.Count - 1)
                                select++;
                            break;
                        case ConsoleKey.Enter:
                            isEnter = true;
                            break;
                    }
                }
            }
        }

        class InGameScene
        {
            private Screen Screen = new Screen(30, 30);
            GameManager gm = GameManager.Instance;
            Panel gamePanel;
            Panel uiPanel;
            Panel staffPanel;

            Snake snake;
            Apple apple;
            TextObject pause;
            TextObject staff;
            TextObject version;
            AsciiObject score;
            AsciiObject level;

            Vector2 direction = new Vector2(0, 0);
            ConsoleKeyInfo keys;
            bool isStop = false;
            bool isGameOver = false;

            public InGameScene()
            {
                //패널 생성 
                gamePanel = new Panel(Screen, 0, 0, 20, 20);
                uiPanel = new Panel(Screen, 20, 0, 10, 10);
                staffPanel = new Panel(Screen, 20, 10, 10, 10, false);

                // 게임 오브젝트 생성
                snake = new Snake(gamePanel, new Vector2(9, 9));
                apple = new Apple(gamePanel);
                pause = new TextObject(gamePanel, new Vector2(8, 9), "PAUSE");
                pause.isVisible = false;

                score = new AsciiObject(uiPanel, new Vector2(1, 2), "SCORE: " + gm.score.ToString());
                level = new AsciiObject(uiPanel, new Vector2(1, 4), "LEVEL: " + gm.level.ToString());

                staff = new TextObject(staffPanel, new Vector2(1, 2), "MADEBYKJC");
                version = new TextObject(staffPanel, new Vector2(1, 4), "V.0.0.0");

                while(true)
                {
                    if (isGameOver)
                    {
                        new GameOverScene();
                        break;
                    }
                    Update();
                    Render();
                }
            }
            
            private void Update()
            {
                Input();

                if (isStop is false)
                {
                    snake.isVisible = true;
                    apple.isVisible = true;
                    pause.isVisible = false;
                    snake.direction = direction;
                    snake.Move();
                    
                }
                else
                {
                    snake.isVisible = false;
                    apple.isVisible = false;
                    pause.isVisible = true;
                }

                foreach (var go in gamePanel.gameObjects)
                {
                    if (go == apple && go.Collision(snake))
                    {
                        snake.AddBody(snake.tail.Position);
                        apple.Move();
                        gm.score += 1;
                        break;
                    }
                    if (go.Collision(snake))
                    {
                        isGameOver = true;
                        break;
                    }
                }

                score.Text = "SCORE: " + gm.score;
                level.Text = "LEVEL: " + gm.level;
                gm.checkLevel();
                Thread.Sleep(gm.speed);
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

        class StaffScene
        {
            Screen screen = new Screen(30, 30);

            Panel staffPanel;

            AsciiObject creater;
            AsciiObject devName;
            AsciiObject refer;
            AsciiObject title;

            ConsoleKeyInfo keys;

            public StaffScene()
            {
                staffPanel = new Panel(screen, 0, 0, 30, 30);

                creater = new AsciiObject(staffPanel, new Vector2(2, 3), "WHO IS CREATER:");

                devName = new AsciiObject(staffPanel, new Vector2(4, 4), "KJC-student");

                refer = new AsciiObject(staffPanel, new Vector2(2, 8), "REF FOR TITLE");
                title = new AsciiObject(staffPanel, new Vector2(4, 9), "https://patorjk.com/software/taag");
                while (true)
                {
                    Update();
                    Render();
                }
            }

            private void Update()
            {
                Thread.Sleep(100);
            }

            private void Render()
            {
                Console.SetCursorPosition(0, 0);
                screen.Draw();
                screen.PrintBufferToScreen();
            }

            private void Input()
            {
                if (Console.KeyAvailable)
                {
                    keys = Console.ReadKey(true);
                    switch (keys.Key)
                    {
                        case ConsoleKey.Enter:
                            break;
                    }
                }
            }

        }

        class GameOverScene
        {
            private Screen screen = new Screen(20, 20);

            Panel gameoverPanel;

            AsciiObject gameover;

            List<AsciiObject> menu = new List<AsciiObject>();
            RightArrow rightArrow;

            int select = 0;
            ConsoleKeyInfo keys;

            bool isEnter = false;
            public GameOverScene()
            {
                Console.Clear();
                gameoverPanel = new Panel(screen, 0, 0, 20, 20);

                gameover = new AsciiObject(gameoverPanel, new Vector2(5, 5), "GAME OVER");
                rightArrow = new RightArrow(gameoverPanel, new Vector2(5, 10));

                menu.Add(new AsciiObject(gameoverPanel, new Vector2(7, 10), "RESTART"));
                menu.Add(new AsciiObject(gameoverPanel, new Vector2(7, 12), "QUIT"));

                while (true)
                {
                    if (isEnter)
                    {
                        switch(select)
                        {
                            case 0:
                                new InGameScene();
                                break;
                            case 1:
                                break;
                        }
                        break;
                    }
                    Update();
                    Render();
                }
            }

            private void Update()
            {
                Input();

                rightArrow.Position.Y = 10 + select * 2;
                Thread.Sleep(100);
            }

            private void Render()
            {
                Console.SetCursorPosition(0, 0);
                screen.Draw();
                screen.PrintBufferToScreen();
            }

            private void Input()
            {
                if (Console.KeyAvailable)
                {
                    keys = Console.ReadKey(true);
                    switch (keys.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (select > 0)
                                select--;
                            break;
                        case ConsoleKey.DownArrow:
                            if (select < menu.Count - 1)
                                select++;
                            break;
                        case ConsoleKey.Enter:
                            isEnter = true;
                            break;
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Console.SetWindowSize(200, 50);
            Console.CursorVisible = false;
            IntroScene intro = new IntroScene();
        }
    }
}
