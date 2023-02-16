using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE1611_Group1_A2;

internal class CartDTO
{
    public int RecordId { get; set; }
    public string CartId { get; set; } = null!;
    public int AlbumId { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    public DateTime DateCreated { get; set; }
}
