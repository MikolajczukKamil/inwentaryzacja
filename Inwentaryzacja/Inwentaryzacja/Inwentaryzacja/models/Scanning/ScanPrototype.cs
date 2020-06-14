using System;

namespace Inwentaryzacja.Models
{
    /// <summary>
    /// Prototyp skanu
    /// </summary>
    public class ScanPrototype
    {
        /// <summary>
        /// Id pokoju w ktorym zostal wykonany skan
        /// </summary>
        public int room;

        /// <summary>
        /// Konstruktor prototypu skanu
        /// </summary>
        /// <param name="room">Id pokoju w ktorym zostal wykonany skan</param>
        public ScanPrototype(int roomId)
        {
            this.room = roomId;
        }
    }
}
