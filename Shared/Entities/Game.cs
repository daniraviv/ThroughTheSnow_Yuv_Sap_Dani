using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThroughTheSnow_Yuv_Sap_Dani.Shared.Entities
{
   public class Game
    {
        public int ID { get; set; }

        public int GameCode { get; set; }
        public string GameName { get; set; }

        public string GameInstruction { get; set; }

        public bool IsPublish { get; set; }
        public int UserID { get; set; }
        public List<Item> GameItems { get; set; }
        public User GameUser { get; set; }
    }
}
