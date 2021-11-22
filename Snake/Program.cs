using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
            string path = "./title.txt";

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
                            new RankingScene();
                            break;
                        }
                        else if (select == 3)
                        {
                            new StaffScene();
                            break;
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
            private Screen Screen = new Screen(40, 40);
            GameManager gm = GameManager.Instance;
            Panel gamePanel;
            Panel uiPanel;
            Panel staffPanel;

            Snake snake;
            Apple apple;
            TextObject pause;
            AsciiObject staff;
            AsciiObject version;
            AsciiObject score;
            AsciiObject level;

            Vector2 direction = new Vector2(0, 0);
            ConsoleKeyInfo keys;
            bool isStop = false;
            bool isGameOver = false;

            public InGameScene()
            {
                //패널 생성 
                gamePanel = new Panel(Screen, 0, 0, 30, 30);
                uiPanel = new Panel(Screen, 30, 0, 10, 10);
                staffPanel = new Panel(Screen, 30, 10, 10, 10, false);

                // 게임 오브젝트 생성
                snake = new Snake(gamePanel, new Vector2(gamePanel.Width / 2, gamePanel.Height / 2));
                apple = new Apple(gamePanel);
                pause = new TextObject(gamePanel, new Vector2(8, 9), "PAUSE");
                pause.isVisible = false;

                score = new AsciiObject(uiPanel, new Vector2(1, 2), "SCORE: " + gm.score.ToString());
                level = new AsciiObject(uiPanel, new Vector2(1, 4), "LEVEL: " + gm.level.ToString());

                staff = new AsciiObject(staffPanel, new Vector2(1, 2), "MADE BY KJC");
                version = new AsciiObject(staffPanel, new Vector2(1, 4), "VER. 0.1.0");

                gm.Reset();
                while(true)
                {
                    if (isGameOver)
                    {
                        new EnrollRankingScene();
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
            bool isEnter = false;
            public StaffScene()
            {
                staffPanel = new Panel(screen, 0, 0, 30, 30);

                creater = new AsciiObject(staffPanel, new Vector2(2, 3), "WHO IS CREATER:");

                devName = new AsciiObject(staffPanel, new Vector2(4, 4), "KJC-student");

                refer = new AsciiObject(staffPanel, new Vector2(2, 8), "REF FOR TITLE");
                title = new AsciiObject(staffPanel, new Vector2(4, 9), "https://patorjk.com/software/taag");
                while (true)
                {
                    if (isEnter)
                    {
                        new IntroScene();
                        break;
                    }
                    Update();
                    Render();
                }
            }

            private void Update()
            {
                Input();
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
                            isEnter = true;
                            break;
                    }
                }
            }

        }

        class EnrollRankingScene
        {
            private Screen screen = new Screen(20, 20);

            Panel enrollRankingPanel;

            UpDownArrow upDownArrow;

            List<AlphabetObject> alphabetObjects = new List<AlphabetObject>();
            ConsoleKeyInfo keys;

            int index = 0;
            bool isEnter = false;

            public EnrollRankingScene()
            {
                Console.Clear();
                enrollRankingPanel = new Panel(screen, 0, 0, 20, 20);

                upDownArrow = new UpDownArrow(enrollRankingPanel, new Vector2(8, 8));

                for (int i = 0; i < 3; i++)
                    alphabetObjects.Add(new AlphabetObject(enrollRankingPanel, new Vector2(8 + (i * 2), 10)));

                alphabetObjects[0].isSelected = true;

                while (true)
                {
                    if (isEnter)
                    {
                        //todo: File IO and Ranking Update 
                        GameManager gm = GameManager.Instance;

                        List<Ranking> rankings = gm.LoadXML();
                        
                        string name = "";
                        for (int i = 0; i < alphabetObjects.Count; i++)
                        {
                            name += (char)(65 + alphabetObjects[i].alphanumber);
                        }

                        Ranking temp = new Ranking() { Name = name, Score = gm.score, Level = gm.level };

                        rankings.Add(temp);

                        // Score 순으로 내림차순 
                        rankings = rankings.OrderByDescending(x => x.Score).ToList();
                        rankings.RemoveAt(10);

                        gm.SaveXML(rankings);

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

                upDownArrow.Position.X = 8 + (index * 2);
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
                            alphabetObjects[index].alphanumber = (alphabetObjects[index].alphanumber + 1) % 26;
                            break;
                        case ConsoleKey.DownArrow:
                            alphabetObjects[index].alphanumber = (26 + alphabetObjects[index].alphanumber - 1) % 26;
                            break;
                        case ConsoleKey.RightArrow:
                            if (index < alphabetObjects.Count - 1)
                            {
                                alphabetObjects[index].isSelected = false;
                                index++;
                                alphabetObjects[index].isSelected = true;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            if (index > 0)
                            {
                                alphabetObjects[index].isSelected = false;
                                index--;
                                alphabetObjects[index].isSelected = true;
                            }
                            break;
                        case ConsoleKey.Enter:
                            isEnter = true;
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

        class RankingScene
        {
            private Screen screen = new Screen(20, 20);

            public Panel rankingPanel;
            public RankingObject rankingObject;

            ConsoleKeyInfo keys;

            bool isEnter = false;
            public RankingScene()
            {
                Console.Clear();

                List<Ranking> rankings = GameManager.Instance.LoadXML();

                rankingPanel = new Panel(screen, 0, 0, 20, 20);

                rankingObject = new RankingObject(rankingPanel, new Vector2(2, 2), rankings);

                while(true)
                {
                    if (isEnter)
                    {
                        new IntroScene();
                        return;
                    }

                    Update();
                    Render();
                }
            }

            private void Update()
            {
                Input();

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
