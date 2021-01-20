using Microsoft.Xna.Framework;
using XNALib;
using XNALib.Menu;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders
{
    public class MainMenu : ScrollMenu
    {
        private const string Title = "Main Menu";
        private readonly SpriteFont Font = Common.str2Font("MenuTitle");

        public MainMenu() :
            base(Engine.Instance.SpriteBatch, "MenuChoice", Engine.Instance.Width, Engine.Instance.Height, 210, 100)
        {
            //Texture = Common.str2Tex("Menu/MainBG01");
            Engine.Instance.Audio.PlayMusic("music");
            Engine.Instance.Audio.SetMusicVolume(5);
            Engine.Instance.Audio.SetSoundVolume(4);

            AddChoice("play", "Play");
            AddChoice("fullscreen", "Toggle full screen");
            AddChoice("credits", "Credits");
            AddChoice("exit", "Exit");
            SelectChoice += new OnSelectChoice(MainMenu_SelectChoice);
        }

        ~MainMenu()
        {
            SelectChoice -= new OnSelectChoice(MainMenu_SelectChoice);
        }

        void MainMenu_SelectChoice(ScrollChoice choice)
        {
            switch (choice.Name)
            {
                case "play":
                    //Engine.Instance.Audio.StopAllMusic();
                    Engine.Instance.Audio.PlaySound("bonusHit");
                    Engine.Instance.ActiveState = new GameOptions();
                    break;
                case "fullscreen":
                    Engine.Instance.Graphics.IsFullScreen = !Engine.Instance.Graphics.IsFullScreen; ;
                    Engine.Instance.Graphics.ApplyChanges();
                    break;
                case "credits":
                    //Engine.Instance.Audio.StopAllMusic();
                    Engine.Instance.Audio.PlaySound("bonusHit");
                    Engine.Instance.ActiveState = new Credits(this);
                    break;
                case "exit":
                    Engine.Instance.Game.Exit();
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        public override void Draw()
        {
            base.Draw();
            Engine.Instance.SpriteBatch.DrawString(Font, Title, new Vector2(Engine.Instance.Width / 2 - Font.MeasureString(Title).X / 2, 40), Color.White);
        }
    }
}