using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    class Celija
    {
        private bool bomba;
        private bool otkrivenaCelija;

        public Celija()
        {

        }

        public void setOtkrivenaCelija()
        {
            this.otkrivenaCelija = true;
        }

        public void celijaJeBomba()
        {
            this.bomba = true;
        }

    }
}
