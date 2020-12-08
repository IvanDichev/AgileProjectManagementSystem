using DataModels.Models.Board.Dtos;
using DataModels.Models.Sprints;
using System.Collections.Generic;

namespace DataModels.Models.Board
{
    public class BoardColumnAllViewModel
    {
        public ICollection<BoardColumnAllDto> BoardColumnAllDto { get; set; }
        public ICollection<SprintDropDownModel> SprintDropDown { get; set; }
    }
}
