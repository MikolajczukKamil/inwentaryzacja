using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
    /// <summary>
    /// Prototyp budynku
    /// </summary>
    public class BuildingPrototype
    {
        /// <summary>
        /// Nazwa budynku
        /// </summary>
        public string name;

        /// <summary>
        /// Kontruktor prototypu budynku
        /// </summary>
        /// <param name="name">Nazwa budynku</param>
        public BuildingPrototype(string name)
        {
            this.name = name;
        }
    }
}
