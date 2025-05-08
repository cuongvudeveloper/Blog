namespace Blog.Domain.Events;

public class DepartmentCreatedEvent : BaseEvent
{
    public DepartmentCreatedEvent(Department item)
    {
        Item = item;
    }

    public Department Item { get; }
}
