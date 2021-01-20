using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib;
using XNALib.Menu;

namespace Invaders
{
    public class GameOptions : ScrollMenu
    {
        private const string Title = "Set options";
        private readonly SpriteFont Font = Common.str2Font("MenuTitle");

        public GameOptions() :
            base(Engine.Instance.SpriteBatch,"MenuChoice",Engine.Instance.Width,Engine.Instance.Height,210,100)
        {
            AddChoice("play", "Play");
            AddChoice("playerCnt", "Players:", "1", "2");
            AddChoice("level", "Start at level: ", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "12", "15", "20");
            AddChoice("back", "Back");
            SelectChoice += new OnSelectChoice(GameOptions_SelectChoice);
        }

        ~GameOptions()
        {
            SelectChoice -= new OnSelectChoice(GameOptions_SelectChoice);
        }

        void GameOptions_SelectChoice(ScrollChoice choice)
        {
            switch (choice.Name)
            {
                case "play":
                    Level.Instance = new Level(int.Parse(GetChoiceByName("playerCnt").SelectedValue));
                    Level.Instance.NextLevel(int.Parse(GetChoiceByName("level").SelectedValue) - 1);
                    Engine.Instance.Audio.PlaySound("bonusHit");
                    Engine.Instance.ActiveState = Level.Instance;
                    break;
                case "back":
                    Engine.Instance.Audio.PlaySound("bonusHit");
                    Engine.Instance.ActiveState = new MainMenu();
                    break;
                default:
                    // Do nothing
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();
            Engine.Instance.SpriteBatch.DrawString(Font, Title, new Vector2(Engine.Instance.Width / 2 - Font.MeasureString(Title).X / 2, 40), Color.White);
        }
    }
}