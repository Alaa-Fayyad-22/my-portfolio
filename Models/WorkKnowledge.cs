﻿using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class WorkKnowledge
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string Tools { get; set; }
    }
}
