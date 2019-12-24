using System.Collections.Generic;

namespace BLL.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AdDTO> Ads { get; set; }
        public CategoryDTO()
        {
            Ads = new List<AdDTO>();
        }
    }
}
