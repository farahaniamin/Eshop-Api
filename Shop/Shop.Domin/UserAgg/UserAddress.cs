using Common.Domain;

namespace Shop.Domain.UserAgg
{
    public class UserAddress : BaseEntity
    {
        public long UserId { get; internal set; }
    }

}
