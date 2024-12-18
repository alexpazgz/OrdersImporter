using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Businnes.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<OnlineOrder> OnlineOrder { get; set; }
        public DbSet<Link> Link { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
