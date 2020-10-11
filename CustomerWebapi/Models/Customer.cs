using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public String LastName { get; set; }

        public DateTime DateBecameCustomer { get; set; }

        public DateTime? DateLastModified { get; set; }

    }
}
