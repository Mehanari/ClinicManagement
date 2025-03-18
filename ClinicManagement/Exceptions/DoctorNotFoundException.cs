namespace ClinicManagement.Exceptions;

public class DoctorNotFoundException : Exception
{
    public DoctorNotFoundException(string message) : base(message)
    {
        
    }
}