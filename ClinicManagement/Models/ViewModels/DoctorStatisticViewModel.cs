namespace ClinicManagement.Models.ViewModels;

public class DoctorStatisticViewModel
{
    public int DoctorId { get; set; }
    public string FullName { get; set; }
    public int TotalAppointmentResults { get; set; }
    public decimal AverageRadiationDose { get; set; }
    public string MostCommonDiagnosis { get; set; }
}