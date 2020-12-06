namespace DataModels.Models.Board.Dtos
{
    public class BoardColumnAllOptoinsDto
    {
        public int Id { get; set; }

        public string ColumnName { get; set; }

        public ushort MaxItems { get; set; }

        public byte PositionLTR { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }
    }
}
