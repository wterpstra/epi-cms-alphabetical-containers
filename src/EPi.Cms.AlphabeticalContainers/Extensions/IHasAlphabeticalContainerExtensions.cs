using System;
using EPi.Cms.AlphabeticalContainers.Model;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace EPi.Cms.AlphabeticalContainers.Extensions
{
    public static class IHasAlphabeticalContainerExtensions
    {
        private static Injected<IContentRepository> ContentRepository { get; set; }

        public static T ConstructAlphabeticalContainerPage<T>(this IHasAlphabeticalContainer content, ContentReference parent, string name) where T : IAlphabeticalContainer
        {
            var alphabeticalContainerPage = ContentRepository.Service.GetDefault<T>(parent);
            alphabeticalContainerPage.PageName = name;
            alphabeticalContainerPage.URLSegment = UrlSegment.CreateUrlSegment(alphabeticalContainerPage);
            return alphabeticalContainerPage;
        }
    }
}
