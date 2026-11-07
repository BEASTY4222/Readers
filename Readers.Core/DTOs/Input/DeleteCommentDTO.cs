using System;
using System.Collections.Generic;
using System.Text;

namespace Readers.Core.DTOs.Input
{
    public class DeleteCommentDTO
    {
        public Guid CommentId { get; set; }
        public string UserId { get; set; }
    }
}
