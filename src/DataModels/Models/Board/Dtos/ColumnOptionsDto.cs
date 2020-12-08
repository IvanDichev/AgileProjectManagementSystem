namespace DataModels.Models.Board.Dtos
{
    public class ColumnOptionsDto
    {
        public int Id { get;set; }

        public string ColumnName { get; set; }

        public ushort MaxItems { get; set; }

        public byte PositionLTR { get; set; }
    }
}
