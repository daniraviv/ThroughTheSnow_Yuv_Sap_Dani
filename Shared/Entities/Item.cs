using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThroughTheSnow_Yuv_Sap_Dani.Shared.Entities
{
   public class Item
    {
        public int ID { get; set; }
        public string ItemType { get; set; }


public string ItemContent { get; set; }

        public bool IsCorrect { get; set; }
        public int GameID { get; set; }
        public Game ItemGame { get; set; }
    }
}
