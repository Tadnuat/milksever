﻿namespace MilkStore.API.Models.AgeRangeModel
{
    public class RequestUpdateAgeRangeModel
    {
        //public int AgeRangeID { get; set; }
        public string Baby { get; set; }
        public string Mama { get; set; }
        public int? ProductItemID { get; set; }
    }
}
