namespace StudentEnrollment.Data.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string name, object key) : base($"{name} with id ({key}) was not found")
    {

    }
}

