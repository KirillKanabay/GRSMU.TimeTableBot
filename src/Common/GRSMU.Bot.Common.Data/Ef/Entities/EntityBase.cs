namespace GRSMU.Bot.Common.Data.Ef.Entities
{
    public abstract class EntityBase
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set;}
    }
}
