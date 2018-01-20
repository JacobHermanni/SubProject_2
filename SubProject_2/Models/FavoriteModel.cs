using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    // extender ResultModel for at indeholde den samme relevante information.
    public class FavoriteModel : ResultModel
    {
        public NoteModel note { get; set; }

        public int favorite_id { get; set; }

        public int post_id { get; set; }
    }
}
