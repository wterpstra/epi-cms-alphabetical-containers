using System;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Core;
using EPiServer;
using EPi.Cms.DateContainers.Model;
using EPiServer.DataAccess;
using EPiServer.Security;

namespace EPi.Cms.DateContainers
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DateContainerInitialization : IInitializableModule
    {
        private IContentRepository _contentRepository;

        public void Initialize(InitializationEngine context)
        {
            _contentRepository = context.Locate.ContentRepository();
            context.Locate.ContentEvents().CreatingContent += Instance_CreatingPage;
        }

        void Instance_CreatingPage(object sender, ContentEventArgs e)
        {
            var page = e.Content as PageData;
            var hasDateContainer = page as IHasDateContainer;

            if (e.Content == null || hasDateContainer == null)
                return;

            DateTime startPublish = page.StartPublish;
            PageData parentPage = _contentRepository.Get<PageData>(page.ParentLink);

            if (parentPage is IDateContainerRoot)
            {
                page.ParentLink = GetDatePageRef(hasDateContainer, parentPage, startPublish, _contentRepository);
            }
        }

        protected virtual PageReference GetDatePageRef(IHasDateContainer hasDateContainerPage, PageData dateContainerRoot, DateTime published, IContentRepository contentRepository)
        {
            foreach (var yearContainer in contentRepository.GetChildren<PageData>(dateContainerRoot.ContentLink))
            {
                if (yearContainer.Name == published.Year.ToString())
                {
                    PageReference result;
                    foreach (PageData monthContainer in contentRepository.GetChildren<PageData>(yearContainer.ContentLink))
                    {
                        if (monthContainer.Name == published.Month.ToString())
                        {
                            result = monthContainer.PageLink;
                            return result;
                        }
                    }

                    result = CreateDateContainer(hasDateContainerPage, yearContainer.PageLink, published.Month.ToString(), new DateTime(published.Year, published.Month, 1));
                    return result;
                }
            }
            PageReference parent = CreateDateContainer(hasDateContainerPage, dateContainerRoot.ContentLink, published.Year.ToString(), new DateTime(published.Year, 1, 1));
            return CreateDateContainer(hasDateContainerPage, parent, published.Month.ToString(), new DateTime(published.Year, published.Month, 1));
        }
        
        protected virtual PageReference CreateDateContainer(IHasDateContainer hasDateContainerPage, ContentReference parent, string name, DateTime startPublish)
        {
            var dateContainer = hasDateContainerPage.ConstructDateContainer(parent, name, startPublish);
            return _contentRepository.Save(dateContainer, SaveAction.Publish, AccessLevel.Publish).ToPageReference();
        }

        public void Preload(string[] parameters) { }

        public void Uninitialize(InitializationEngine context)
        {
            context.Locate.ContentEvents().CreatingContent -= Instance_CreatingPage;
        }
    }   
}