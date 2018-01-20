using System;
using System.ComponentModel.DataAnnotations;


namespace DAL
{
    public class Note
    {
        // favorite_id er primary key fordi der kun er 1 note per favorite og dermed er note_id overflødig.
        [Key]
        public int favorite_id
        {
            get;
            set;
        }

        // Nullable bør fjernes, da den altid programelt sættes i DataService.cs
        public Nullable<DateTime> created_timestamp
        {
            get;
            set;
        }

        public string body
        {
            get;
            set;
        }



    }
}
