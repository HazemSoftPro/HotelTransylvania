using Ardalis.Specification;

namespace InnHotel.Core.NotificationAggregate.Specifications;

public class NotificationByUserIdSpec : Specification<Notification>
{
    public NotificationByUserIdSpec(int userId, bool unreadOnly = false, int skip = 0, int take = 50)
    {
        Query.Where(n => n.UserId == userId);

        if (unreadOnly)
        {
            Query.Where(n => n.Status != NotificationStatus.Read);
        }

        Query
            .OrderByDescending(n => n.CreatedAt)
            .Skip(skip)
            .Take(take);
    }
}