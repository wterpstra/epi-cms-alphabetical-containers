using EPi.Cms.DateContainers.Model;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using System;

namespace EPi.Cms.DateContainers.Extensions
{
    public static class IHasDateContainerExtensions
    {
        private static Injected<IContentRepository> ContentRepository { get; set; }

        public static T ConstructDateContainerPage<T>(this IHasDateContainer content, ContentReference parent, string name, DateTime startPublish) where T : IDateContainer
        {
            var dateContainerPage = ContentRepository.Service.GetDefault<T>(parent);
            dateContainerPage.PageName = name;
            dateContainerPage.StartPublish = startPublish;
            dateContainerPage.URLSegment = UrlSegment.CreateUrlSegment(dateContainerPage);
            return dateContainerPage;
        }
    }
}
