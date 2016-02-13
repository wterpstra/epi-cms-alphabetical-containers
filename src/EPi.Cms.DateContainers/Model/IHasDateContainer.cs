using EPiServer.Core;
using System;

namespace EPi.Cms.DateContainers.Model
{
    /// <summary>
    /// Marker interface for content types that should be placed in data containers
    /// </summary>
    public interface IHasDateContainer : IContent
    {
        IDateContainer ConstructDateContainer(ContentReference parent, string name, DateTime startPublish);
    }
}
