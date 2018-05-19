using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MinesweeperWPF
{
    class MinesweeperButton : Button
    {
        public bool IsMine { get; private set; }

        public MinesweeperButton()
        {
            this.IsMine = false;
        }

        public void SetMineOnButton()
        {
            this.IsMine = true;
            this.Content = "*";
        }
    }
}
