using System;
using System.Linq;
using EPi.Cms.AlphabeticalContainers.Model;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;

namespace EPi.Cms.AlphabeticalContainers
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class AlphabeticalContainerInitialization : IInitializableModule
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
            var hasAlphabeticalContainer = page as IHasAlphabeticalContainer;

            if (e.Content == null || hasAlphabeticalContainer == null) return;

            var name = ResolveDefaultContainerName(page);
            var parentPage = _contentRepository.Get<PageData>(page.ParentLink);

            if (parentPage is IAlphabeticalContainerRoot)
                page.ParentLink = GetAlphabeticalPageRef(hasAlphabeticalContainer, parentPage, name, _contentRepository);
        }

        protected virtual string ResolveDefaultContainerName(PageData page)
        {
            var firstChar = page.Name[0];

            if (!char.IsLetter(firstChar)) return "0-9";

            return firstChar.ToString().ToUpper();
        }

        protected virtual PageReference GetAlphabeticalPageRef(IHasAlphabeticalContainer hasAlphabeticalContainerPage, PageData alphabeticalContainerRoot, string name, IContentRepository contentRepository)
        {
            var alphabeticalContainer = contentRepository.GetChildren<PageData>(alphabeticalContainerRoot.ContentLink)
                .FirstOrDefault(container => container.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (alphabeticalContainer != null) return alphabeticalContainer.PageLink;

            return CreateAlphabeticalContainer(hasAlphabeticalContainerPage, alphabeticalContainerRoot.PageLink, name);
        }
        
        protected virtual PageReference CreateAlphabeticalContainer(IHasAlphabeticalContainer hasAlphabeticalContainerPage, ContentReference parent, string name)
        {
            var alphabeticalContainer = hasAlphabeticalContainerPage.ConstructAlphabeticalContainer(parent, name);
            return _contentRepository.Save(alphabeticalContainer, SaveAction.Publish, AccessLevel.Publish).ToPageReference();
        }

        public void Preload(string[] parameters) { }

        public void Uninitialize(InitializationEngine context)
        {
            context.Locate.ContentEvents().CreatingContent -= Instance_CreatingPage;
        }
    }   
}