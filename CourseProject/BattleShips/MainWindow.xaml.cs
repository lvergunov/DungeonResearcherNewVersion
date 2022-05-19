using DungeonResearcher.RenderingProcess;
using GameClasses.Enums;
using GameClasses.GameMap;
using GameClasses.GameObjects;
using GameClasses.GameObjects.Chests;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using OpenTK.Wpf;
using OpenTK.Graphics;

namespace DungeonResearcher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameCycle mainCycle = new GameCycle();
        Renderer Renderer;
        bool IsMapOpened { get; set; }
        bool IsGameWindowLoaded { get; set; } = false;
        public MainWindow()
        {
            InitializeComponent();
            var settings = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 6
            };
            OpenTkControl.Start(settings);
            Dictionary<Directions,string> heroPositions = new Dictionary<Directions,string>();
            heroPositions.Add(Directions.Left, @"D:\Dungeon\CourseProject\BattleShips\Images\skull.png");
            Dictionary<Type,string> mapObjects = new Dictionary<Type,string>();
            mapObjects.Add(typeof(Room), @"D:\Dungeon\CourseProject\BattleShips\Images\Floor.png");
            mapObjects.Add(typeof(DangerousRoom), @"D:\Dungeon\CourseProject\BattleShips\Images\Floor.png");
            Renderer = new Renderer(heroPositions, null, mapObjects);
        }
        private void OnAnimationTick(object sender, EventArgs e)
        {
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Home)
            {
                //this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
            if (mainCycle.IsPlaying)
            {
                switch (e.Key)
                {
                    case Key.D:
                            mainCycle.Player.Move(Directions.Right,
                                (int)mainCycle.Player.SpeedPerMS);
                        break;
                    case Key.A:
                            mainCycle.Player.Move(Directions.Left,
                                (int)mainCycle.Player.SpeedPerMS);
                        break;
                    case Key.S:
                            mainCycle.Player.Move(Directions.Down,
                                (int)mainCycle.Player.SpeedPerMS);
                        break;
                    case Key.W:
                            mainCycle.Player.Move(Directions.Up,
                                (int)mainCycle.Player.SpeedPerMS);
                        break;
                    case Key.Space:
                            mainCycle.Player.Punch();
                        break;
                    case Key.Q:
                        mainCycle.Player.ChangeWeapon();
                        break;
                        //case Key.E:
                        //  mainCycle.HerosInteractiveAction();
                        //    break;
                        //case Key.M:
                        //    if (this.MapControl.Visibility == Visibility.Hidden)
                        //    {
                        //        MapControl.Visibility = Visibility.Visible;
                        //        Parameters.Visibility = Visibility.Hidden;
                        //    }
                        //    else
                        //    {
                        //        MapControl.Visibility = Visibility.Hidden;
                        //        Parameters.Visibility = Visibility.Visible;
                        //    }
                        //    break;
                }
            }
        }

        private void RewardInfoOut(string paramName)
        {
            MessageBox.Show(paramName + " param was increased.");
        }
        private void EndGameInfoOut()
        {
            MessageBox.Show("Hero is dead. Game is over.");
        }

        private void OnOpeningChest(AbstractChest chest)
        {

        }

        private void OpenTkControl_OnRender(TimeSpan delta)
        {
            if (mainCycle.IsLevelLoaded && mainCycle.IsPlaying)
            {
                if (!IsGameWindowLoaded)
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();

                    GL.Ortho(1000, 1000, 1000, 1000, 0.0, 1.0);
                    GL.Enable(EnableCap.Texture2D);

                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    GL.ClearColor(Color4.Black);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    GL.Clear(ClearBufferMask.ColorBufferBit);
                    IsGameWindowLoaded = true;
                }

                Matrix4 viewMatrix = Matrix4.CreateOrthographicOffCenter(mainCycle.Player.NativeRoom.Colider.Left - 10,
                    mainCycle.Player.NativeRoom.Colider.Right + 10, mainCycle.Player.NativeRoom.Colider.Bottom + 10,
                    mainCycle.Player.NativeRoom.Colider.Top - 10, -1.0f, 1.0f);


                GL.MatrixMode(MatrixMode.Modelview);

                GL.LoadMatrix(ref viewMatrix);

                RenderObject(mainCycle.Player.NativeRoom.Colider,
                    Renderer.GetMapObjectTexture(mainCycle.Player.NativeRoom.GetType()), 1);

                RenderObject(mainCycle.Player.Colider, Renderer.GetPlayerTexture(Directions.Left), 0);

                GL.LoadIdentity();
                GL.Flush();
            }
        }


        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() => mainCycle.LoadGame());
            thread.Start();
            while (!mainCycle.IsLevelLoaded)
                Thread.Sleep(1);
            thread = new Thread(() => mainCycle.StartGame());
            mainCycle.onReward += RewardInfoOut;
            mainCycle.onEndGame += EndGameInfoOut;
            foreach (Room room in mainCycle.GameMap.Insertions)
            {
                foreach (IGenerable obj in room.Stuffs)
                {
                    if (obj is AbstractChest chest) chest.onOpen += OnOpeningChest;
                }
            }
            thread.Start();
            BackgroundImage.Visibility = Visibility.Hidden;
            OpenTkControl.Visibility = Visibility.Visible;
            StartButton.Visibility = Visibility.Hidden;
            Parameters.Visibility = Visibility.Visible;
        }

        private void RenderObject(Rectangle colider,int texture,int depth)
        {
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.Begin(BeginMode.Polygon);

            GL.Color3(Color.White);

            GL.TexCoord2(0, 0);
            GL.Vertex3(colider.Left, colider.Top, -depth);

            GL.TexCoord2(1, 0);
            GL.Vertex3(colider.Right, colider.Top, -depth);

            GL.TexCoord2(1, 1);
            GL.Vertex3(colider.Right, colider.Bottom, -depth);

            GL.TexCoord2(0, 1);
            GL.Vertex3(colider.Left, colider.Bottom, -depth);

            GL.End();
        }

        private void OpenTkControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
