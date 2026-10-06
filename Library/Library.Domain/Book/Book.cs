namespace Library.Domain.Book;

// Entity because has Identity (Id)
// Also the aggregate root
// Some parts can start as properties but may evolve into full-fledged value objects if needed
// Properties have no setters or have private setters - so that the change can be done only through allowed methods
// Invariants (business rules) are enforced through methods rather than allowing direct property modification

public class Book
{
    public Guid Id { get;}
    public string Title { get;  }
    public BookStatus Status { get; private set; }
    public Isbn Isbn { get; }

    public Book(string isbn, string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or empty", nameof(title));
        }

        Isbn = new Isbn(isbn ?? throw new ArgumentNullException(nameof(isbn)));
        Title = title;
        Status = BookStatus.Available;
    }

    public void MarkAsOnLoan()
    {
        if(Status == BookStatus.OnLoan)
        {
            throw new InvalidOperationException("Book is already on loan.");
        }
        Status = BookStatus.OnLoan;
    }

    public void MarkAsAvailable()
    {
        if(Status == BookStatus.Available)
        {
            throw new InvalidOperationException("Book is already available.");
        }
        Status = BookStatus.Available;
    }
}