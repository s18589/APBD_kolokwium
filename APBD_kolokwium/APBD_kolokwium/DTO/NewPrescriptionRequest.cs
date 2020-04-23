using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace APBD_kolokwium.DTO
{
    public class NewPrescriptionRequest
    {
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public int IdPatient { get; set; }
        public int IdDoctor { get; set; }
    }
}

