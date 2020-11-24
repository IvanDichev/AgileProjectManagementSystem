﻿using Ganss.XSS;

namespace DataModels.Models.Comments
{
    public class CommentInputModel
    {
        private HtmlSanitizer sanitizer;
        public CommentInputModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        public string Description { get; set; }

        public string SanitizedDescription => this.sanitizer.Sanitize(this.Description);

        public int UserStoryId { get; set; }

        public int AddedById { get; set; }
    }
}