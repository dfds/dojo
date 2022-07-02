namespace Demo.Domain.Common;
public abstract class BaseEntity
{
    /// <summary>
    /// This is a common property because posts and categories are the entities. 
    /// They have unique ids which differ them from value objects
    /// </summary>
    public int Id { get; set; }
}