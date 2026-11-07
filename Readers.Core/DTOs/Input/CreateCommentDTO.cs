using System;
using System.Collections.Generic;
using System.Text;

namespace Readers.Core.DTOs.Input
{
    public class CreateCommentDTO
    {
        public string BookId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}
