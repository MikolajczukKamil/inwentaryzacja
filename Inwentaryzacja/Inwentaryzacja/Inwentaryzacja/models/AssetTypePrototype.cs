using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.models
{
    class AssetTypePrototype
    {
        public string name { get; set; }
        public string letter { get; set; }

        public AssetTypePrototype(string name, string letter)
        {
            this.name = name;
            this.letter = letter;
        }
    }
}
