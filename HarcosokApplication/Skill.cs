using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarcosokApplication
{
    class Skill
    {
        private int id;
        private string nev;
        private string leiras;

        public Skill(int id, string nev, string szoveg)
        {
            this.id = id;
            this.nev = nev;
            this.leiras = szoveg;
        }

            public string Leiras { get => this.leiras; set => this.leiras = value; }

        public int Id { get => this.id; }

        public override string ToString()
        {
            return this.nev;
        }
    }
}
