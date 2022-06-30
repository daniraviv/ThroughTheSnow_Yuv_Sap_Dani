using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThroughTheSnow_Yuv_Sap_Dani.Shared.Entities
{
   public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Game> UserGames { get; set; }
    }
}
