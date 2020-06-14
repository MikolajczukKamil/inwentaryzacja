using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
    /// <summary>
    /// Prototyp aktualizacji skanu
    /// </summary>
    public class ScanUpdatePropotype
    {
        /// <summary>
        /// Numer id skanu
        /// </summary>
        public int id;
        
        /// <summary>
        /// Lista aktualizacji srodkow trwalych w skanie
        /// </summary>
        public ScanPositionPropotype[] positions;

        /// <summary>
        /// Konstruktor prototypu aktualizacji skanu
        /// </summary>
        /// <param name="id">Numer id skanu</param>
        /// <param name="positions">Lista aktualizacji srodkow trwalych w skanie</param>
        public ScanUpdatePropotype(int id, ScanPositionPropotype[] positions)
        {
            this.id = id;
            this.positions = positions;
        }
    }

    /// <summary>
    /// Prototyp aktualizacji srodka trwalego w skanie
    /// </summary>
    public class ScanPositionPropotype
    {
        /// <summary>
        /// Srodek trwaly
        /// </summary>
        public int asset;
        
        /// <summary>
        /// Stan srodka trwalego
        /// </summary>
        public int state;

        /// <summary>
        /// Konstruktor prototypu aktualizacji srodka trwalego w skanie
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="state"></param>
        public ScanPositionPropotype(int asset, int state)
        {
            this.asset = asset;
            this.state = state;
        }
    }
}
