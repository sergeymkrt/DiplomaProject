namespace DiplomaProject.Domain.SeedWork;

public class Paginated<T>
{
    #region Properties
    public int PageNumber { get; private set; }
    public int PageSize { get; private set; }
    public int TotalRecords { get; private set; }
    public int TotalPages { get; private set; }
    public IEnumerable<T> Items { get; private set; }
    public bool HasPrevious => PageNumber == 1;
    public bool HasNext => PageNumber < TotalPages;
    #endregion

    #region Constructors
    public Paginated(IEnumerable<T> source, int totalRecords, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        Items = source;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
    }

    public Paginated()
    {
        Items = Array.Empty<T>();
    }
    #endregion
}