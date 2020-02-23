using SpaceInvaders.SpaceInvaders.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders.Infrastructure.ObjectModel.Menues.MenuItems
{
    public class MultiItem : MenuItem
    {
        protected string m_MainText;
        public string MainText
        {
            get { return m_MainText; }
            set { m_MainText = value; CurrenOption = CurrenOption; }
        }

        protected string m_CurrenOption;
        public string CurrenOption
        {
            get { return m_CurrenOption; }
            set
            {
                m_CurrenOption = value;
                Text = string.Format("{0} {1}", MainText, m_CurrenOption);
            }
        }
        protected int m_CurrenOptionIdx;
        public int CurrenOptionIdx
        {
            get { return m_CurrenOptionIdx; }
            set {
                PreviousOptionIdx = m_CurrenOptionIdx;
                m_CurrenOptionIdx = value  < 0 ? 0 : value;
                m_CurrenOptionIdx = m_CurrenOptionIdx == TextOptions.Count ? m_CurrenOptionIdx - 1 : m_CurrenOptionIdx;
                CurrenOption = TextOptions[CurrenOptionIdx];
            }
        }

        protected int m_PreviousOptionIdx;
        public int PreviousOptionIdx
        {
            get { return m_PreviousOptionIdx; }
            set { m_PreviousOptionIdx = value;}
        }
        Dictionary<int, string> m_TextOptions;
        protected Dictionary<int, string> TextOptions
        {
            get { return m_TextOptions; }
            set { m_TextOptions = value; }
        }
        private bool m_IsLooped;
        public bool IsLooped
        {
            get { return m_IsLooped; }
            set { m_IsLooped = value; }
        }

        public MultiItem(GameStructure i_Game, Action i_FunctionToExecute, params string[] i_TextOptions)
            : base(i_Game, i_FunctionToExecute)
        {
            TextOptions = new Dictionary<int, string>();
            IsLooped = true;
            InitTextOptions(i_TextOptions);
        }

        public MultiItem(GameStructure i_Game, params string[] i_TextOptions)
            : base(i_Game)
        {
            TextOptions = new Dictionary<int, string>();
            IsLooped = true;
            InitTextOptions(i_TextOptions);
        }


        public MultiItem(GameStructure i_Game) : base(i_Game)
        {

        }

        protected override bool Test()
        {
            return InputManager.KeyPressed(Keys.PageUp) || InputManager.KeyPressed(Keys.PageDown);
        }

        protected override void ActicvateIfTestIsTrue()
        {
            if (InputManager.KeyPressed(Keys.PageUp))
            {
                CurrenOptionIdx = (CurrenOptionIdx == TextOptions.Count - 1) && IsLooped ? 0 : CurrenOptionIdx + 1;
            }
            else if (InputManager.KeyPressed(Keys.PageDown))
            {
                CurrenOptionIdx = (CurrenOptionIdx == 0) && IsLooped ? (TextOptions.Count - 1) : CurrenOptionIdx - 1;
            }
            base.ActicvateIfTestIsTrue();
        }

        private void InitTextOptions(string[] i_TextOptions)
        {
            int i = 0;
            foreach (string option in i_TextOptions)
            {
                TextOptions.Add(i, option);
                i++;
            }
           // CurrenOption = TextOptions[0];
            PreviousOptionIdx = CurrenOptionIdx = 0;
        }

    }
}

