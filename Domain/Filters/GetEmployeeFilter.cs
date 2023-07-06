namespace Domain.Filters;

public class GetEmployeeFilter : PaginationFilter
{
    public string? Title { get; set; }

    public GetEmployeeFilter() : base()
    {
    }

    public GetEmployeeFilter(int pageNumber, int pageSize) : base(pageNumber, pageSize)
    {
    }
    
}