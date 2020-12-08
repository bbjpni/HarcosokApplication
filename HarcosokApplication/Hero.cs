using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarcosokApplication
{
    class Hero
    {
        DateTime letrehozas;
        string nev;
        int id;

        public Hero(string datum, string nev, int id)
        {
            this.id = id;
            this.nev = nev;
            this.letrehozas = DateTime.Parse(datum);
        }

        public DateTime Letrehozas { get => this.letrehozas; }

        public int Id { get => this.id; }

        public override string ToString()
        {
            return this.nev;
        }
    }
}
