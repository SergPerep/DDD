namespace Library.Domain.Book;

// Why it is not just a string?
// - It has business rules - must be of a valid format
public class Isbn
{
    public string Value { get; }
    
    public Isbn(string value)
    {
        // TODO: Validate the ISBN format
        Value = value;
    }
}