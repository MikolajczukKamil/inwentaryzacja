using Inwentaryzacja.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
    /// <summary>
    /// Prototyp srodka trwalego w raporcie
    /// </summary>
    public class ReportPositionPrototype
    {
        /// <summary>
        /// Id srodka trwalego
        /// </summary>
        public int id;
        
        /// <summary>
        /// Id pokoju w ktorym poprzednio znajdowal sie srodek trwaly
        /// </summary>
        public int? previous = null;
        
        /// <summary>
        /// Czy srodek trwaly powinien znajdowac sie w tym pokoju
        /// </summary>
        public bool present;

        /// <summary>
        /// Konstuktor prototypu srodka trwalego w raporcie
        /// </summary>
        /// <param name="asset">Srodek trwaly</param>
        /// <param name="previous">Poprzedni pokoj srodka trwalego</param>
        /// <param name="present">Czy srodek trwaly powinien znajdowac sie w tym pokoju</param>
        public ReportPositionPrototype(AssetEntity asset, RoomEntity previous, bool present)
        {
            this.id = asset.id;
            if (previous != null)
                this.previous = previous.id;
            this.present = present;
        }
    }
}
