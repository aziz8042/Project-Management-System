using System.ComponentModel.DataAnnotations;
using WorkCleverSolution.Attributes.Validation;

namespace WorkCleverSolution.Dto.Project.Board;

public class CreateBoardViewInput
{
    [Required]
    [ValidBoardId]
    public int BoardId { get; set; }
    
    [Required] public string Type { get; set; }
    
    [Required] public string Name { get; set; }
}