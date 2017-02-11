using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class GradeComment
    {
        public string ID { get; }
        public string AuthorID { get; }
        public string GradeID { get; }
        public string Text { get; }

        public GradeComment(string id, string authorId, string gradeId, string text)
        {
            this.ID = id;
            this.AuthorID = authorId;
            this.GradeID = gradeId;
            this.Text = text;
        }
    }
}
