﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    public class AnswerModel
    {
        public int answer_id { get; set; }

        public int post_id { get; set; }

        public int parent_Id { get; set; }
    }
}
