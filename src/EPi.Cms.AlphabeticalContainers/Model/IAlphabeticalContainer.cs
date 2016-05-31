using EPiServer.Core;

namespace EPi.Cms.AlphabeticalContainers.Model
{
    public interface IAlphabeticalContainer : IContent
    {
        string PageName { get; set; }
        string URLSegment { get; set; }
    }
}