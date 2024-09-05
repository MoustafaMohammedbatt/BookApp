﻿namespace Domain.Entites
{
    public class BaseModel
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }=DateTime.Now;
        public DateTime? UpdatedOn { get; set;}
    }
}
