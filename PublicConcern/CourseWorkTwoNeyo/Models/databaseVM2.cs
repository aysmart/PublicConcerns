using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace CourseWorkTwoNeyo.Models
{
    public class databaseVM2
    {
        public List<Causes> CausesStarted { get; set; }
        public IEnumerable<databaseVM2> CausesStarted2 { get; set; }

        public List<Causes> CausesWithUsers { get; set; }
        public IEnumerable<databaseVM2> CausesSigned2 { get; set; }
        public IEnumerable<databaseVM2> CausesUpdates { get; set; }
        public List<Causes> CausesSigned { get; set; }
        public Causes causeList { get; set; }
        public Users userList { get; set; }
        public Updates updatesList { get; set; }
        public Comments commentList { get; set; }
        public List<Users> userList2 { get; set; }
        public IEnumerable<databaseVM2> signatureList2 { get; set; }

        public Signatures signatureList { get; set; }
        public int? userID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RecordCount { get; set; }
    }
}

