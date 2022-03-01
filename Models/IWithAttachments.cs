using System.Collections.Generic;

namespace EventPlanner.Models
{
    public interface IWithAttachments
    {
        IList<Attachment> Attachments { get; }
    }
}
