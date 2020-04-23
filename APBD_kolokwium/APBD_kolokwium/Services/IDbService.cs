using APBD_kolokwium.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD_kolokwium.Services
{
    public interface IDbService
    {
        List<PrescriptionResponse> GetPrescription(int id);
    }
}
