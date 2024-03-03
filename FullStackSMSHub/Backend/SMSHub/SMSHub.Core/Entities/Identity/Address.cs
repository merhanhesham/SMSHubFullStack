using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSHub.Core.Entities.Identity
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }

        public string AppUserId { get; set; }//primary key of appuser as fk in address, pk of optional in mandatory

        //relationship of appuser, address 1 to 1
        public AppUser User { get; set; }

    }
}
