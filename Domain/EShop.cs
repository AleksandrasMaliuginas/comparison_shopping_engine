using System.Collections.Generic;

namespace Domain
{
    public class EShop
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public ICollection<RealProduct> RealProducts { get; set; }
    }
}
