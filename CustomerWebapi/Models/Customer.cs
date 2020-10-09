using System;

namespace Models
{
    public class Customer
    {
        public int Id {get; set; }

        public String FirstName {get; set; }

        public String LastName {get; set; }

        public DateTime DateBecameCustomer {get; set; }

        public DateTime DateLastModified {get; set;}

    }
}
