using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
    public class Select_Item
    {
        public int id { get; set; }
        public string value { get; set; }
        public bool selected { get; set; }
        public Select_Item(int id , string value , bool selected)
        {
            this.id = id ; this.value = value ; this.selected = selected;
        }
    }
}