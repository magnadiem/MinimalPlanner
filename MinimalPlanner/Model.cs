using Microsoft.EntityFrameworkCore;

namespace MinimalPlanner
{
    /// <summary>
    /// Класс, описывающий ивент
    /// </summary>
    public class Entry
    {
        /// <summary>
        /// ID ивента для EF & дерганья ивента с фронта
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Описание ивента
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Дедлайн ивента
        /// </summary>
        public DateTime TimeOfEvent { get; set; }
    }

    /// <summary>
    /// Контекст для EF
    /// </summary>
    public class DBContext : DbContext
    {
        public DbSet<Entry> Entries => Set<Entry>();
        public DBContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=planner.db");
        }
    }

}
