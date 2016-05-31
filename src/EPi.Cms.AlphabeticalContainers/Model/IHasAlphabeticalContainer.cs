using EPiServer.Core;

namespace EPi.Cms.AlphabeticalContainers.Model
{
    /// <summary>
    /// Marker interface for content types that should be placed in alphabetical containers
    /// </summary>
    public interface IHasAlphabeticalContainer : IContent
    {
        IAlphabeticalContainer ConstructAlphabeticalContainer(ContentReference parent, string name);
    }
}
