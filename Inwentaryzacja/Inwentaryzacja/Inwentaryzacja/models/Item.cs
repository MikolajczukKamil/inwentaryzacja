using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.models
{
    public class Item
    {
        public string Name { get; set; }
        public int Item_Id { get; set; }
        public int Room { get; set; }
        public string PictureUrl { get; set; }

        public Item(string name, int itemId, int room, string pictureUrl)
        {
            this.Name = name;
            this.Item_Id = itemId;
            this.Room = room;
            this.PictureUrl = pictureUrl;
        }
        public string ItemData
        {
            get
            {
                return string.Format("{0} (id:{1})", Name, Item_Id);
            }
        }
    }
}
