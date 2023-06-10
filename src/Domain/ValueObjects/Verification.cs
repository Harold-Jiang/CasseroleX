namespace CasseroleX.Domain.ValueObjects;

public class Verification : ValueObject
{
    public bool Mobile { get; private set; }
    public bool Email { get; private set; }
  
    public Verification() { }

    public Verification(bool mobile, bool email)
    {
        Mobile = mobile;
        Email = email;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        // Using a yield return statement to return each element one at a time
        yield return Mobile;
        yield return Email;
    }
}
