using EPiServer.Core;
using System;

namespace EPi.Cms.DateContainers.Model
{
    public interface IDateContainer : IContent
    {
        string PageName { get; set; }
        DateTime StartPublish { get; set; }
        string URLSegment { get; set; }
    }
}
