using APBD_kolokwium.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APBD_kolokwium.Services
{
    public class SQLServerController : IDbService
    {
        public NewPrescriptionResponse CreatePrescription(NewPrescriptionRequest request)
        {
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18589;Integrated Security=true"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                if(DateTime.Compare(request.DueDate,request.Date) > 0)
                {
                    transaction.Rollback();
                    throw new ArgumentException("duedate jest pozniejsze niz date");
                }
                
                command.Parameters.AddWithValue("duedate", request.DueDate);
                command.Parameters.AddWithValue("date", request.Date);
                command.Parameters.AddWithValue("idpatient", request.IdPatient);
                command.Parameters.AddWithValue("iddoctor", request.IdDoctor);

                command.CommandText = "insert into Prescription(date, duedate, iddoctor, idpatient) values(@date, @duedate, @iddoctor, @idpatient)";

                var dr1 = command.ExecuteReader();

                dr1.Close();

                command.CommandText = "select max(idprescription) from prescription";
                var dr2 = command.ExecuteReader();
                dr2.Read();
                int idprescription = dr2.GetInt32(0);

                dr2.Close();
                transaction.Commit();

                
                return new NewPrescriptionResponse
                {
                    Id = idprescription
                };
            }
        }
        public List<PrescriptionResponse> GetPrescription(int id)
        {
            using(var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18589;Integrated Security=true"))
            using(var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                List<PrescriptionResponse> response = new List<PrescriptionResponse>();

                command.Parameters.AddWithValue("prescriptionid", id);

                command.CommandText = "select date, duedate,idpatient,iddoctor, dose,details,name,description,type from prescription p inner join prescription_medicament pm on p.idprescription = pm.idmedicament inner join medicament m on pm.idmedicament = m.idmedicament where p.idprescription = @prescriptionid";

                var dr = command.ExecuteReader();

                if (!dr.HasRows)
                {
                    transaction.Rollback();
                    throw new ArgumentException("nie ma takiej recepty w bazce");
                }

                while (dr.Read())
                {
                    response.Add(new PrescriptionResponse
                    {
                        Date = dr.GetDateTime(0),
                        DueDate = dr.GetDateTime(1),
                        IdPatient = dr.GetInt32(2),
                        IdDoctor = dr.GetInt32(3),
                        Dose = dr.GetInt32(4),
                        Details = dr.GetString(5),
                        Name = dr.GetString(6),
                        Description = dr.GetString(7),
                        Type = dr.GetString(8)
                    });
                }
                dr.Close();
                transaction.Commit();
                return response;
            }
        }
    }
}
