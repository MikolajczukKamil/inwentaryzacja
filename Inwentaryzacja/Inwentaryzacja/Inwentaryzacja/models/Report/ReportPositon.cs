using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
    /// <summary>
    /// Srodek trwaly w raporcie
    /// </summary>
    public class ReportPosition
    {
        /// <summary>
        /// Srodek trwaly
        /// </summary>
        public Asset Asset { get; private set; }
        
        /// <summary>
        /// Czy srodek trwaly powinien znajdowac sie w tym pokoju
        /// </summary>
        public bool Present { get; private set; }
        
        /// <summary>
        /// Pokoj w ktorym poprzednio znajdowal sie srodek trwaly
        /// </summary>
        public Room Previus { get; private set; }
        
        /// <summary>
        /// Etykieta srodka trwalego w raporcie
        /// </summary>
        public string Label
        {
            get { return $"{Asset.Type.Name} (id: {Asset.Id})"; }
        }

        /// <summary>
        /// Konstuktor srodka trwalego w raporcie
        /// </summary>
        /// <param name="asset">Srodek trwaly</param>
        /// <param name="previous">Poprzedni pokoj srodka trwalego</param>
        /// <param name="present">Czy srodek trwaly powinien znajdowac sie w tym pokoju</param>
        public ReportPosition(Asset asset, Room previus, bool present)
        {
            Asset = asset;
            Present = present;
            Previus = previus;
        }
    }
}
