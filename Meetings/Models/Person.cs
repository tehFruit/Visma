using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meetings.Models
{
    public class Person
    {
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }

        public Person(string name, DateTime dateAdded)
        {   
            this.Name = name;
            this.DateAdded = dateAdded;
        }
    }
}
