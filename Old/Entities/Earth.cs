using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTesting.Entities
{
    public class Earth
    {
        public static Earth Get
        {
            get
            {
                Earth earth = new Earth()
                {
                    Mountain = "Everest",
                    Ocean = "Big Ocean",
                    Sea = "Black Sea",
                    IsActive = true,
                    Property = "Mustafa Demirel"
                };
                return earth;
            }
        }

        public List<Earth> GetList
        {
            get
            {
                return
                    new List<Earth>() {
                        Get,Get
                    };
            }
            set { return; }
        }

        public string Property { get; set; }
        public string Mountain { get; set; }
        public string Sea { get; set; }
        public string Ocean { get; set; }
        public bool IsActive { get; set; }
        public DateTime CacheAdded { get; set; }
    }
}
