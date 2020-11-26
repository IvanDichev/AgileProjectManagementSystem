namespace Data.Models.Base
{
    public interface IBaseEntity<T>
    {
        public T Id { get; set; }
    }
}
