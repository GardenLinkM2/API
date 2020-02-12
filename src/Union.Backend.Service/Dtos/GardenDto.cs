using System;
using System.Collections.Generic;

namespace Union.Backend.Service.Dtos
{
    public class GardenDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public bool Reserve { get; set; }
        public string Type { get; set; }
        public int MinUse { get; set; }
        public UserDto Owner { get; set; }
        public ValidationDto Validation { get; set; }
        public CriteriaDto Criteria { get; set; } // TODO : changer le type dans la liste
        public List<PhotoDto> Photos { get; set; }
    }
}
