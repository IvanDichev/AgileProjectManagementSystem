﻿using Ganss.XSS;

namespace DataModels.Models.Comments
{
    public class CommentInputModel
    {
        private readonly HtmlSanitizer sanitizer;
        public CommentInputModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => this.sanitizer.Sanitize(this.Description);

        public int UserStoryId { get; set; }

        public int AddedById { get; set; }
    }
}
