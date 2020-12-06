namespace DataModels.Models.Board.Dtos
{
    public class BoardColumnAllNamePositionDto
    {
        public int Id { get;set; }

        public string KanbanBoardColumnOptionColumnName { get; set; }

        public ushort KanbanBoardColumnOptionMaxItems { get; set; }

        public byte KanbanBoardColumnOptionPositionLTR { get; set; }
    }
}
