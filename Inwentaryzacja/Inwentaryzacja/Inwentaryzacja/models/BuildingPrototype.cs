using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.models
{
    public class BuildingPrototype
    {
        public string name { get; set; }
        public BuildingPrototype(string name)
        {
            this.name = name;
        }
    }
}
