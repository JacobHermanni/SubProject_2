using System;
using Xunit;
using DAL;

namespace SOVATestSuite
{
    public class DALTest
    {
        [Fact]
        public void GetPost_ValidId_ReturnsPostObjectWithProperID()
        {
            var service = new DataService();
            var post = service.GetPost(71); // we know from the databse, that 71 is a valid id.
            Assert.Equal(71, post.post_id);
        }
    }
}
