using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib;

namespace Invaders
{
    internal class Credit
    {
        private bool m_IsTitle;

        public bool IsTitle
        {
            get { return m_IsTitle; }
            set { m_IsTitle = value; }
        }
        private string m_Person;

        public string Person
        {
            get { return m_Person; }
            set { m_Person = value; }
        }
        private Vector2 m_Location;

        public Vector2 Location
        {
            get { return m_Location; }
            set { m_Location = value; }
        }

        public Credit(bool isTitle, string person)
        {
            IsTitle = isTitle;
            Person = person;
        }
    }

    public class Credits : IActiveState
    {
        #region Members
        IActiveState Parent;
        private SpriteFont m_Font;
        private SpriteFont Font
        {
            get { return m_Font; }
            set { m_Font = value; }
        }
        private SpriteFont m_FontTitle;
        private SpriteFont FontTitle
        {
            get { return m_FontTitle; }
            set { m_FontTitle = value; }
        }

        private Color m_FontColor = Color.Gold;
        private Color FontColor
        {
            get { return m_FontColor; }
            set { m_FontColor = value; }
        }

        private List<Credit> m_AllCredits = new List<Credit>();
        private List<Credit> AllCredits
        {
            get { return m_AllCredits; }
            set { m_AllCredits = value; }
        }
        #endregion

        public Credits(IActiveState parent)
        {
            Parent = parent;

            Font = Common.str2Font("Credit");
            FontTitle = Common.str2Font("CreditTitle");
            AllCredits.Add(new Credit(true, "Invaders XNA"));
            AllCredits.Add(new Credit(true, "Created on August 11th 2011 (single sit-through)"));
            AllCredits.Add(new Credit(true, ""));
            AllCredits.Add(new Credit(true, ""));
            AllCredits.Add(new Credit(true, "Programming & Design"));
            AllCredits.Add(new Credit(false, "C.@.r*l*o v@o@n R@nz0vv"));
            AllCredits.Add(new Credit(true, ""));
            AllCredits.Add(new Credit(true, "Audio"));
            AllCredits.Add(new Credit(false, "sandyrb (www.freesoundproject.org) - Impact sound."));
            AllCredits.Add(new Credit(false, "grunz (www.freesound.org) - level up sound"));
            
            AllCredits.Add(new Credit(true, ""));
            AllCredits.Add(new Credit(true, "Graphics"));
            AllCredits.Add(new Credit(false, "ac3raven (www.opengameart.org) - Space Backgrounds"));
            AllCredits.Add(new Credit(false, "Lamoot (www.opengameart.org) - Player sprite"));
            AllCredits.Add(new Credit(false, "CruzR (www.opengameart.org) - Laser sprites"));
            AllCredits.Add(new Credit(false, "C.@.r*l*o v@o@n R@nz0vv - Blocks"));
            AllCredits.Add(new Credit(true, ""));
            AllCredits.Add(new Credit(true, "Special Thanks & Tools"));
            AllCredits.Add(new Credit(false, "The Gimp"));

            int y = Engine.Instance.Height;
            foreach (Credit credit in AllCredits)
            {
                SpriteFont measureFont;
                if (credit.IsTitle)
                    measureFont = FontTitle;
                else
                    measureFont = Font;
                credit.Location = Common.CenterStringX(measureFont, credit.Person, Engine.Instance.Width, y);
                y += (int)measureFont.MeasureString(Common.MeasureString).Y;
            }
        }

        public void Update(GameTime gameTime)
        {
            // Scroll down
            foreach (Credit credit in AllCredits)
                credit.Location = new Vector2(credit.Location.X, credit.Location.Y - 1);

            // Input
            if (InputMgr.Instance.IsPressed(null,InputMgr.Instance.DefaultConfirmKey,InputMgr.Instance.DefaultConfirmButton) ||
                InputMgr.Instance.IsPressed(null,InputMgr.Instance.DefaultCancelKey,InputMgr.Instance.DefaultCancelButton))
            {
                Engine.Instance.ActiveState = new MainMenu();
            }
        }

        public void Draw()
        {
            Engine.Instance.Game.GraphicsDevice.Clear(Color.Black);
            foreach (Credit credit in AllCredits)
            {
                if (credit.IsTitle)
                    Engine.Instance.SpriteBatch.DrawString(FontTitle, credit.Person, credit.Location, Color.Yellow);
                else
                    Engine.Instance.SpriteBatch.DrawString(Font, credit.Person, credit.Location, Color.Yellow);
            }
        }
    }
}
