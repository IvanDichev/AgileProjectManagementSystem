namespace DataModels.Models.Board.Dtos
{
    public class BoardColumnAllNamePositionDto
    {
        public int Id { get;set; }

        public string ColumnName { get; set; }

        public byte ColumnOrder { get; set; }

        public ushort MaxItems { get; set; }
    }
}
